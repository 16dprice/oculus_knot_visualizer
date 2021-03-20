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

        public static List<LinkComponent> SimplifyLink(
            List<LinkComponent> linkComponents,
            float H,
            float K,
            float alpha,
            float beta
        )
        {
            var linkRelaxingBeads = GetLinkRelaxingBeads(linkComponents);
            var forces = CalculateForces(linkRelaxingBeads, H, K, alpha, beta);

            var beadList = new List<Bead>();
            for (int i = 0; i < forces.Count; i++)
            {
                linkRelaxingBeads[i].bead.position += forces[i];
                beadList.Add(linkRelaxingBeads[i].bead);
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
            for(int i = 0; i < linkRelaxingBeads.Count; i++) forces.Add(new Vector3());

            ApplyMechanicalForces(linkRelaxingBeads, forces, H, beta);
            ApplyElectricalForces(linkRelaxingBeads, forces, K, alpha);
            ApplyForceLimit(forces);

            return forces;
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

                        var forceMagnitude = -K * (float) Math.Pow(forceDirection.magnitude, -alpha - 2);

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
                    forces[i].Normalize();
                    forces[i] = D_MAX * forces[i];
                }
            }
        }
    }
}