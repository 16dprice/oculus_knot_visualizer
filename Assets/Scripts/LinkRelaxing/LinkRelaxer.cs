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
        private static float D_MAX = 0.1f;
        private static float D_CLOSE = 0.15f;

        private List<LinkRelaxingBead> _linkRelaxingBeads;
        private List<(LinkRelaxingBead, (LinkRelaxingBead, LinkRelaxingBead))> _adjacentBeads;
        private List<(LinkRelaxingBead, List<LinkRelaxingBead>)> _nonAdjacentBeads;

        public LinkRelaxer(List<LinkComponent> linkComponents)
        {
            _linkRelaxingBeads = GetLinkRelaxingBeads(linkComponents);
            _adjacentBeads = GetAdjacentBeads();
            _nonAdjacentBeads = GetNonAdjacentBeads();
        }

        private List<(LinkRelaxingBead, (LinkRelaxingBead, LinkRelaxingBead))> GetAdjacentBeads()
        {
            var adjacentBeadList = new List<(LinkRelaxingBead, (LinkRelaxingBead, LinkRelaxingBead))>();

            foreach (var firstBead in _linkRelaxingBeads)
            {
                var beadsAdjacentToFirstBead =
                    _linkRelaxingBeads.Where(secondBead => firstBead.IsBeadAdjacent(secondBead)).ToList();
                
                var adjacentBeadsTuple = (
                    beadsAdjacentToFirstBead[0],
                    beadsAdjacentToFirstBead[1]
                );

                var beadListItem = (firstBead, adjacentBeadsTuple);

                adjacentBeadList.Add(beadListItem);
            }

            return adjacentBeadList;
        }

        private List<(LinkRelaxingBead, List<LinkRelaxingBead>)> GetNonAdjacentBeads()
        {
            var nonAdjacentBeadList = new List<(LinkRelaxingBead, List<LinkRelaxingBead>)>();

            for (var firstBeadIndex = 0; firstBeadIndex < _linkRelaxingBeads.Count; firstBeadIndex++)
            {
                var firstBead = _linkRelaxingBeads[firstBeadIndex];
                var beadsNonAdjacentToFirstBead = new List<LinkRelaxingBead>();

                for (var secondBeadIndex = 0; secondBeadIndex < _linkRelaxingBeads.Count; secondBeadIndex++)
                {
                    if (secondBeadIndex == firstBeadIndex) continue;
                    if (firstBead.IsBeadAdjacent(_linkRelaxingBeads[secondBeadIndex])) continue;
                    
                    beadsNonAdjacentToFirstBead.Add(_linkRelaxingBeads[secondBeadIndex]);
                }
                
                var beadListItem = (firstBead, beadsNonAdjacentToFirstBead);
                
                nonAdjacentBeadList.Add(beadListItem);
            }

            return nonAdjacentBeadList;
        }

        public List<LinkComponent> SimplifyLink(float H, float K, float alpha, float beta)
        {
            var forces = CalculateForces(_linkRelaxingBeads, H, K, alpha, beta);

            var beadList = new List<Bead>();
            for (int i = 0; i < forces.Length; i++)
            {
                _linkRelaxingBeads[i].bead.position += forces[i];
                if (!IsBeadSafeToMove(_linkRelaxingBeads, i))
                {
                    _linkRelaxingBeads[i].bead.position -= forces[i];
                }

                beadList.Add(_linkRelaxingBeads[i].bead);
            }

            var linkComponent = new LinkComponent(beadList);

            return new List<LinkComponent> {linkComponent};
        }

        private Vector3[] CalculateForces(
            List<LinkRelaxingBead> linkRelaxingBeads,
            float H,
            float K,
            float alpha,
            float beta
        )
        {
            var forces = new Vector3[linkRelaxingBeads.Count];

            ApplyMechanicalForces(forces, H, beta);
            ApplyElectricalForces(forces, K, alpha);
            ApplyForceLimit(forces);

            return forces;
        }

        private void ApplyMechanicalForces(
            Vector3[] forces,
            float H,
            float beta
        )
        {
            for (var currentBeadIndex = 0; currentBeadIndex < _adjacentBeads.Count; currentBeadIndex++)
            {
                var adjacentBeadTuple = _adjacentBeads[currentBeadIndex];
                
                var currentBead = adjacentBeadTuple.Item1;

                var firstAdjacentBead = adjacentBeadTuple.Item2.Item1;
                var secondAdjacentBead = adjacentBeadTuple.Item2.Item2;

                forces[currentBeadIndex] += MechanicalForce(H, beta, currentBead, firstAdjacentBead);
                forces[currentBeadIndex] += MechanicalForce(H, beta, currentBead, secondAdjacentBead);
            }
        }

        private Vector3 MechanicalForce(float H, float beta, LinkRelaxingBead firstBead, LinkRelaxingBead secondBead)
        {
            var forceDirection = secondBead.bead.position - firstBead.bead.position;
            forceDirection.Normalize();
            
            var forceMagnitude = H * (float) Math.Pow(forceDirection.magnitude, beta + 1);
            
            var mechanicalForce = forceMagnitude * forceDirection;

            return mechanicalForce;
        }

        private void ApplyElectricalForces(
            Vector3[] forces,
            float K,
            float alpha
        )
        {
            for (var currentBeadIndex = 0; currentBeadIndex < _nonAdjacentBeads.Count; currentBeadIndex++)
            {
                var (currentBead, linkRelaxingBeads) = _nonAdjacentBeads[currentBeadIndex];

                foreach (var secondBead in linkRelaxingBeads)
                {
                    forces[currentBeadIndex] += ElectricalForce(K, alpha, currentBead, secondBead);                    
                }
            }
        }
        
        private Vector3 ElectricalForce(float K, float alpha, LinkRelaxingBead firstBead, LinkRelaxingBead secondBead)
        {
            var forceDirection = secondBead.bead.position - firstBead.bead.position;
            forceDirection.Normalize();
            
            var forceMagnitude = -K * (float) Math.Pow(forceDirection.magnitude, -2 - alpha);
            
            var electricalForce = forceMagnitude * forceDirection;

            return electricalForce;
        }

        private static void ApplyForceLimit(Vector3[] forces)
        {
            for (int i = 0; i < forces.Length; i++)
            {
                if (forces[i].magnitude > D_MAX)
                {
                    forces[i] = forces[i].normalized;
                    forces[i] = D_MAX * forces[i];
                }
            }
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

        private static bool IsBeadSafeToMove(List<LinkRelaxingBead> linkRelaxingBeads, int currentBeadIndex)
        {
            var currentBeadSegments = GetCurrentBeadSegments(linkRelaxingBeads, currentBeadIndex);
            var nonAdjacentBeadSegments = GetNonAdjacentBeadSegments(linkRelaxingBeads, currentBeadSegments);

            foreach (var currentBeadSegment in currentBeadSegments)
            {
                foreach (var nonAdjacentBeadSegment in nonAdjacentBeadSegments)
                {
                    var segmentDistance =
                        SegmentDistanceCalculator.SegmentDistance(currentBeadSegment, nonAdjacentBeadSegment);

                    if (segmentDistance < D_CLOSE)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static List<Segment> GetCurrentBeadSegments(
            List<LinkRelaxingBead> linkRelaxingBeads,
            int currentBeadIndex
        )
        {
            var currentBeadSegments = new List<Segment>();

            for (int otherBeadIndex = 0; otherBeadIndex < linkRelaxingBeads.Count; otherBeadIndex++)
            {
                if (otherBeadIndex == currentBeadIndex) continue;
                if (linkRelaxingBeads[otherBeadIndex].IsBeadAdjacent(linkRelaxingBeads[currentBeadIndex]))
                {
                    currentBeadSegments.Add(
                        new Segment(
                            linkRelaxingBeads[currentBeadIndex],
                            linkRelaxingBeads[otherBeadIndex]
                        )
                    );
                }
            }

            return currentBeadSegments;
        }

        private static List<Segment> GetNonAdjacentBeadSegments(
            List<LinkRelaxingBead> linkRelaxingBeads,
            List<Segment> currentBeadSegments
        )
        {
            var beadSegments = new List<Segment>();

            for (int firstBeadIndex = 0; firstBeadIndex < linkRelaxingBeads.Count - 1; firstBeadIndex++)
            {
                var firstBead = linkRelaxingBeads[firstBeadIndex];

                for (int secondBeadIndex = firstBeadIndex + 1;
                    secondBeadIndex < linkRelaxingBeads.Count;
                    secondBeadIndex++)
                {
                    var secondBead = linkRelaxingBeads[secondBeadIndex];
                    var newSegment = new Segment(firstBead, secondBead);

                    if (
                        !IsSegmentAdjacentToAnySegments(newSegment, currentBeadSegments) &&
                        firstBead.IsBeadAdjacent(secondBead)
                    )
                    {
                        beadSegments.Add(newSegment);
                    }
                }
            }

            return beadSegments;
        }

        private static bool IsSegmentAdjacentToAnySegments(Segment s, List<Segment> sList)
        {
            foreach (var segment in sList)
            {
                if (s.IsSegmentAdjacent(segment)) return true;
            }

            return false;
        }
    }
}