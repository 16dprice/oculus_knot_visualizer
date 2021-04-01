using System;
using System.Collections.Generic;
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
        
        public LinkRelaxer(List<LinkComponent> linkComponents)
        {
            _linkRelaxingBeads = GetLinkRelaxingBeads(linkComponents);
        }

        public List<LinkComponent> SimplifyLink(float H, float K, float alpha, float beta)
        {
            var forces = CalculateForces(_linkRelaxingBeads, H, K, alpha, beta);

            var beadList = new List<Bead>();
            for (int i = 0; i < forces.Count; i++)
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

        private static List<Vector3> CalculateForces(
            List<LinkRelaxingBead> linkRelaxingBeads,
            float H,
            float K,
            float alpha,
            float beta
        )
        {
            var forces = new List<Vector3>();
            for (int i = 0; i < linkRelaxingBeads.Count; i++) forces.Add(new Vector3());

            ApplyMechanicalForces(linkRelaxingBeads, forces, H, beta);
            ApplyElectricalForces(linkRelaxingBeads, forces, K, alpha);
            ApplyForceLimit(forces);

            return forces;
        }

        private static void ApplyMechanicalForces(
            List<LinkRelaxingBead> linkRelaxingBeads,
            List<Vector3> forces,
            float H,
            float beta
        )
        {
            for (int firstBeadIndex = 0; firstBeadIndex < linkRelaxingBeads.Count - 1; firstBeadIndex++)
            {
                var firstBead = linkRelaxingBeads[firstBeadIndex];
                for (int secondBeadIndex = firstBeadIndex + 1;
                    secondBeadIndex < linkRelaxingBeads.Count;
                    secondBeadIndex++)
                {
                    var secondBead = linkRelaxingBeads[secondBeadIndex];

                    if (firstBead.IsBeadAdjacent(secondBead))
                    {
                        var forceDirection = secondBead.bead.position - firstBead.bead.position;
                        forceDirection.Normalize();

                        var forceMagnitude = H * (float) Math.Pow(forceDirection.magnitude, beta + 1);

                        var mechanicalForce = forceMagnitude * forceDirection;

                        forces[firstBeadIndex] += mechanicalForce;
                        forces[secondBeadIndex] -= mechanicalForce;
                    }
                }
            }
        }

        private static void ApplyElectricalForces(
            List<LinkRelaxingBead> linkRelaxingBeads,
            List<Vector3> forces,
            float K,
            float alpha
        )
        {
            for (int firstBeadIndex = 0; firstBeadIndex < linkRelaxingBeads.Count - 1; firstBeadIndex++)
            {
                var firstBead = linkRelaxingBeads[firstBeadIndex];
                for (int secondBeadIndex = firstBeadIndex + 1;
                    secondBeadIndex < linkRelaxingBeads.Count;
                    secondBeadIndex++)
                {
                    var secondBead = linkRelaxingBeads[secondBeadIndex];

                    if (!firstBead.IsBeadAdjacent(secondBead))
                    {
                        var forceDirection = secondBead.bead.position - firstBead.bead.position;
                        forceDirection.Normalize();

                        var forceMagnitude = -K * (float) Math.Pow(forceDirection.magnitude, -2 - alpha);

                        var electricalForce = forceMagnitude * forceDirection;

                        forces[firstBeadIndex] += electricalForce;
                        forces[secondBeadIndex] -= electricalForce;
                    }
                }
            }
        }

        private static void ApplyForceLimit(List<Vector3> forces)
        {
            for (int i = 0; i < forces.Count; i++)
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