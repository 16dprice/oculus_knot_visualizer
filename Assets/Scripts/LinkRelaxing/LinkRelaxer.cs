using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using UnityEngine;

namespace LinkRelaxing
{
    public class LinkRelaxer
    {
        // D_MAX must be less than D_CLOSE
        private const float D_MAX = 0.0005f;
        private const float D_CLOSE = 0.001f;

        private readonly List<LinkRelaxingBead> _linkRelaxingBeads;
        private readonly int[] _componentStartIndices;
        private readonly Segment[] _segments;
        
        public LinkRelaxer(List<LinkComponent> linkComponents)
        {
            _linkRelaxingBeads = GetLinkRelaxingBeads(linkComponents);
            _componentStartIndices = GetComponentStartIndices(linkComponents.Count);
            _segments = GetSegments();
        }

        public List<LinkComponent> SimplifyLink(float H, float K, float alpha, float beta)
        {
            var forces = CalculateForces(H, K, alpha, beta);
            var beadList = new List<Bead>();
            var linkComponents = new List<LinkComponent>();
            var componentIndex = 1;

            for (var beadIndex = 0; beadIndex < forces.Length; beadIndex++)
            {
                if (IsBeadInSafePosition(beadIndex)) _linkRelaxingBeads[beadIndex].bead.position += forces[beadIndex];
                
                beadList.Add(_linkRelaxingBeads[beadIndex].bead);
                if (componentIndex == _componentStartIndices.Length)
                {
                    if (beadIndex == _linkRelaxingBeads.Count - 1)
                    {
                        linkComponents.Add(new LinkComponent(beadList));
                    }
                }
                else if(beadIndex == _componentStartIndices[componentIndex] - 1)
                {
                    linkComponents.Add(new LinkComponent(beadList));
                    componentIndex++;
                    beadList = new List<Bead>();
                }
            }

            return linkComponents;
        }
        
        private Vector3[] CalculateForces(float H, float K, float alpha, float beta)
        {
            var forces = new Vector3[_linkRelaxingBeads.Count];

            ApplyMechanicalForces(forces, H, beta);
            ApplyElectricalForces(forces, K, alpha);
            ApplyForceLimit(forces);

            return forces;
        }
        
        private void ApplyMechanicalForces(Vector3[] forces, float H, float beta)
        {
            for (var currentBeadIndex = 0; currentBeadIndex < _linkRelaxingBeads.Count; currentBeadIndex++)
            {
                var currentBead = _linkRelaxingBeads[currentBeadIndex];
                var (firstIndex, secondIndex) = AdjacentBeadIndices(currentBeadIndex);

                var firstAdjacentBead = _linkRelaxingBeads[firstIndex];
                var secondAdjacentBead = _linkRelaxingBeads[secondIndex];

                forces[currentBeadIndex] += MechanicalForce(H, beta, currentBead, firstAdjacentBead);
                forces[currentBeadIndex] += MechanicalForce(H, beta, currentBead, secondAdjacentBead);
            }
        }

        private Vector3 MechanicalForce(float H, float beta, LinkRelaxingBead firstBead, LinkRelaxingBead secondBead)
        {
            var forceDirection = (secondBead.bead.position - firstBead.bead.position).normalized;
            var forceMagnitude = H * (float) Math.Pow((secondBead.bead.position - firstBead.bead.position).magnitude, beta + 1);
            var mechanicalForce = forceMagnitude * forceDirection;

            return mechanicalForce;
        }

        private void ApplyElectricalForces(Vector3[] forces, float K, float alpha)
        {
            for (var currentBeadIndex = 0; currentBeadIndex < _linkRelaxingBeads.Count; currentBeadIndex++)
            {
                var adjacentBeadIndices = AdjacentBeadIndices(
                    currentBeadIndex,
                    _componentStartIndices[_linkRelaxingBeads[currentBeadIndex].componentIndex],
                    _linkRelaxingBeads[currentBeadIndex].numBeadsInThisComponent
                );
                
                var currentBead = _linkRelaxingBeads[currentBeadIndex];

                for (var otherBeadIndex = 0; otherBeadIndex < _linkRelaxingBeads.Count; otherBeadIndex++)
                {
                    if (
                        otherBeadIndex == currentBeadIndex ||
                        otherBeadIndex == adjacentBeadIndices.Item1 ||
                        otherBeadIndex == adjacentBeadIndices.Item2
                    ) continue;

                    var otherBead = _linkRelaxingBeads[otherBeadIndex];
                    forces[currentBeadIndex] += ElectricalForce(K, alpha, currentBead, otherBead);
                }
            }
        }

        private Vector3 ElectricalForce(float K, float alpha, LinkRelaxingBead firstBead, LinkRelaxingBead secondBead)
        {
            var forceDirection = (secondBead.bead.position - firstBead.bead.position).normalized;
            var forceMagnitude = -K * (float) Math.Pow((secondBead.bead.position - firstBead.bead.position).magnitude, -2 - alpha);
            var electricalForce = forceMagnitude * forceDirection;

            return electricalForce;
        }

        private bool IsBeadInSafePosition(int beadIndex)
        {
            var (firstIndex, secondIndex) = AdjacentBeadIndices(beadIndex);
            var firstSegment = new Segment(_linkRelaxingBeads[firstIndex], _linkRelaxingBeads[beadIndex]);
            var secondSegment = new Segment(_linkRelaxingBeads[beadIndex], _linkRelaxingBeads[secondIndex]);

            foreach (var segment in _segments)
            {
                if (!firstSegment.IsSegmentAdjacent(segment)) 
                    if (SegmentDistanceCalculator.SegmentDistance(firstSegment, segment) < D_CLOSE) 
                        return false;
                
                if(!secondSegment.IsSegmentAdjacent(segment))
                    if (SegmentDistanceCalculator.SegmentDistance(secondSegment, segment) < D_CLOSE)
                        return false;
            }

            return true;
        }
        
        private static void ApplyForceLimit(Vector3[] forces)
        {
            for (var beadIndex = 0; beadIndex < forces.Length; beadIndex++)
                if (forces[beadIndex].magnitude > D_MAX)
                    forces[beadIndex] = D_MAX * forces[beadIndex].normalized;
        }

        private static List<LinkRelaxingBead> GetLinkRelaxingBeads(List<LinkComponent> linkComponents)
        {
            var linkRelaxingBeads = new List<LinkRelaxingBead>();

            for (int componentIndex = 0; componentIndex < linkComponents.Count; componentIndex++)
            {
                int numBeadsInThisComponent = linkComponents[componentIndex].BeadList.Count;

                for (int order = 0; order < numBeadsInThisComponent; order++)
                    linkRelaxingBeads.Add(
                        new LinkRelaxingBead(
                            linkComponents[componentIndex].BeadList[order],
                            componentIndex,
                            order,
                            numBeadsInThisComponent
                        )
                    );
            }

            return linkRelaxingBeads;
        }
        
        private int[] GetComponentStartIndices(int numComponents)
        {
            var startIndices = new int[numComponents];
            startIndices[0] = 0;

            var componentIndex = 1;
            for (int beadIndex = 1; beadIndex < _linkRelaxingBeads.Count; beadIndex++)
            {
                if (_linkRelaxingBeads[beadIndex].componentIndex != _linkRelaxingBeads[beadIndex - 1].componentIndex)
                {
                    startIndices[componentIndex] = beadIndex;
                    componentIndex++;
                }
            }

            return startIndices;
        }

        private Segment[] GetSegments()
        {
            var segments = new Segment[_linkRelaxingBeads.Count];

            for (var beadIndex = 0; beadIndex < segments.Length; beadIndex++)
            {
                var nextBeadIndex = AdjacentBeadIndices(beadIndex).Item2;
                segments[beadIndex] = new Segment(_linkRelaxingBeads[beadIndex], _linkRelaxingBeads[nextBeadIndex]);
            }

            return segments;
        }

        private (int, int) AdjacentBeadIndices(int beadIndex)
        {
            return AdjacentBeadIndices(
                beadIndex,
                _componentStartIndices[_linkRelaxingBeads[beadIndex].componentIndex],
                _linkRelaxingBeads[beadIndex].numBeadsInThisComponent
            );
        }

        private static (int, int) AdjacentBeadIndices(int currentBeadIndex, int startIndex, int totalBeadsInComponent)
        {
            var firstIndexOffset = (currentBeadIndex - startIndex + totalBeadsInComponent - 1) % totalBeadsInComponent;
            var secondIndexOffset = (currentBeadIndex - startIndex + totalBeadsInComponent + 1) % totalBeadsInComponent;

            var firstIndex = startIndex + firstIndexOffset;
            var secondIndex = startIndex + secondIndexOffset;

            return (firstIndex, secondIndex);
        }
    }
}