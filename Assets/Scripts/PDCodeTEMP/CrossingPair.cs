using UnityEngine;

namespace PDCodeTEMP
{
    public class CrossingPair
    {
        public PDCodeBeadPair over;
        public PDCodeBeadPair under;

        public CrossingPair(PDCodeBeadPair pair1, PDCodeBeadPair pair2)
        {
            SetOverUnder(pair1, pair2);
        }

        public string GetPrintString()
        {
            (var strand1, var strand2, var strand3, var strand4) = GetPDCodeCrossing();

            return $"X[{strand1}, {strand2}, {strand3}, {strand4}]";
        }

        public (int strand1, int strand2, int strand3, int strand4) GetPDCodeCrossing()
        {
            int strand1 = under.first.strand;
            int strand3 = under.second.strand;
            int strand2, strand4;

            Vector2 underStrandVec = under.second.bead.position - under.first.bead.position;
            Vector2 overStrandVec1 = over.first.bead.position - under.first.bead.position;

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

            var firstSegmentZValAtCrossing = pair1.first.bead.position.z + pair1IntersectionParameterValue *
                (pair1.second.bead.position.z - pair1.first.bead.position.z);
            var secondSegmentZValAtCrossing = pair2.first.bead.position.z + pair2IntersectionParameterValue *
                (pair2.second.bead.position.z - pair2.first.bead.position.z);

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