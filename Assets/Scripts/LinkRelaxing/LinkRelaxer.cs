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

        private static List<Vector3> CalculateForces(
            List<LinkComponent> linkComponents,
            float H,
            float K,
            float alpha,
            float beta
        )
        {
            var linkRelaxingBeads = GetLinkRelaxingBeads(linkComponents);
            var forces = new List<Vector3>();

            return forces;
        }

        private static List<LinkRelaxingBead> GetLinkRelaxingBeads(List<LinkComponent> linkComponents)
        {
            var linkRelaxingBeads = new List<LinkRelaxingBead>();

            for (int componentIndex = 0; componentIndex < linkComponents.Count; componentIndex++)
            {
                for (int order = 0; order < linkComponents[componentIndex].BeadList.Count; order++)
                {
                    linkRelaxingBeads.Add(
                        new LinkRelaxingBead(
                            linkComponents[componentIndex].BeadList[order],
                            componentIndex,
                            order
                        )
                    );
                }
            }

            return linkRelaxingBeads;
        }
    }
}