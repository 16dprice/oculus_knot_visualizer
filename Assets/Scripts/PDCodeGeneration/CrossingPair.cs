using UnityEngine;

namespace PDCodeGeneration
{
    public class CrossingPair
    {
        public PDCodeBeadPair over;
        public PDCodeBeadPair under;

        public CrossingPair(PDCodeBeadPair pair1, PDCodeBeadPair pair2)
        {
            SetOverUnder(pair1, pair2);
        }

        public (int strand1, int strand2, int strand3, int strand4) GetPDCodeCrossing()
        {
            int strand1 = under.first.strand;
            int strand3 = under.second.strand;
            int strand2, strand4;

            Vector2 underStrandVec = under.second.position - under.first.position;
            Vector2 overStrandVec1 = over.first.position - under.first.position;
            Vector2 overStrandVec2 = over.second.position - under.first.position;

            if (Vector2.SignedAngle(underStrandVec, overStrandVec1) > 0)
            {
                strand4 = over.first.strand;
                strand2 = over.second.strand;
            }
            else
            {
                strand2 = over.first.strand;
                strand4 = over.second.strand;
            }

            return (strand1, strand2, strand3, strand4);
        }

        private void SetOverUnder(PDCodeBeadPair pair1, PDCodeBeadPair pair2)
        {
            var (pair1IntersectionParameterValue, pair2IntersectionParameterValue) = 
                pair1.GetIntersectionParameterizationValues(pair2);

            var firstSegmentZValAtCrossing = pair1.first.position.z + pair1IntersectionParameterValue *
                (pair1.second.position.z - pair1.first.position.z);
            var secondSegmentZValAtCrossing = pair2.first.position.z + pair2IntersectionParameterValue *
                (pair2.second.position.z - pair2.first.position.z);

            if (firstSegmentZValAtCrossing > secondSegmentZValAtCrossing)
            {
                over = pair1;
                under = pair2;
            }
            else
            {
                over = pair2;
                under = pair1;
            }
        }
    }
}