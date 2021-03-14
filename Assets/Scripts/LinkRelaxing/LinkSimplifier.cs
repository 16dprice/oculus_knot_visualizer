using System.Collections.Generic;
using Domain;
using UnityEngine;

namespace LinkRelaxing
{
    public class LinkSimplifier
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
            var forces = new List<Vector3>();

            return forces;
        }
    }
}