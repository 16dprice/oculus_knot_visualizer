using System;
using UnityEngine;

namespace LinkRelaxing
{
    public class SegmentDistanceCalculator
    {
        private const float SMALL_NUM = 0.00001f;

        public static float SegmentDistance(Segment S1, Segment S2)
        {
            var u = S1.P1.position - S1.P0.position;
            var v = S2.P1.position - S2.P0.position;
            var w = S1.P0.position - S2.P1.position;

            var a = Vector3.Dot(u, u);
            var b = Vector3.Dot(u, v);
            var c = Vector3.Dot(v, v);
            var d = Vector3.Dot(u, w);
            var e = Vector3.Dot(v, w);

            var D = a * c - b * b;

            float sc, sN, sD = D;
            float tc, tN, tD = D;

            if (D < SMALL_NUM)
            {
                sN = 0.0f;
                sD = 1.0f;
                tN = e;
                tD = c;
            }
            else
            {
                sN = b * e - c * d;
                tN = a * e - b * d;
                if (sN < 0.0)
                {
                    sN = 0.0f;
                    tN = e;
                    tD = c;
                }
                else if (sN > sD)
                {
                    sN = sD;
                    tN = e + b;
                    tD = c;
                }
            }

            if (tN < 0.0)
            {
                tN = 0.0f;
                if (d >= 0.0) sN = 0.0f;
                else if (d <= a) sN = sD;
                else
                {
                    sN = -d;
                    sD = a;
                }
            }
            else if (tN > tD)
            {
                tN = tD;
                if (b < d) sN = 0;
                else if (b > a + d) sN = sD;
                else
                {
                    sN = b - d;
                    sD = a;
                }
            }

            sc = Math.Abs(sN) < SMALL_NUM ? 0.0f : sN / sD;
            tc = Math.Abs(tN) < SMALL_NUM ? 0.0f : tN / tD;

            Vector3 dP = w + (sc * u) - (tc * v);

            return dP.magnitude;
        }
    }
}