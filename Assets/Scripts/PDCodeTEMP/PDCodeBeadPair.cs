using System.Collections.Generic;
using Domain;
using UnityEngine;

namespace PDCodeTEMP
{
    public class PDCodeBeadPair
    {
        public PDCodeBead first;
        public PDCodeBead second;
        public List<PDCodeBeadPair> pairsThatIntersect;

        public PDCodeBeadPair(PDCodeBead first, PDCodeBead second)
        {
            this.first = first;
            this.second = second;
        }

        public void ClearIntersectingPairs()
        {
            pairsThatIntersect = new List<PDCodeBeadPair>();
        }

        public bool DoesIntersectOtherBeadPair(PDCodeBeadPair other)
        {
            if (IsBeadPairAdjacent(other)) return false;
            
            var (thisSegmentValue, otherSegmentValue) = GetIntersectionParameterizationValues(other);

            var isOnThisSegment = 0 < thisSegmentValue && thisSegmentValue < 1;
            var isOnOtherSegment = 0 < otherSegmentValue && otherSegmentValue < 1;

            return isOnThisSegment && isOnOtherSegment;
        }

        public (float thisSegmentValue, float otherSegmentValue) GetIntersectionParameterizationValues(
            PDCodeBeadPair other
        )
        {
            var x1 = first.bead.position.x;
            var y1 = first.bead.position.y;
            var x3 = second.bead.position.x;
            var y3 = second.bead.position.y;
            var slopeOfSegmentA = (y3 - y1) / (x3 - x1);

            var x2 = other.first.bead.position.x;
            var y2 = other.first.bead.position.y;
            var x4 = other.second.bead.position.x;
            var y4 = other.second.bead.position.y;
            var slopeOfSegmentB = (y4 - y2) / (x4 - x2);

            var intersectionXValue = (y2 - slopeOfSegmentB * x2 - y1 + slopeOfSegmentA * x1) /
                                     (slopeOfSegmentA - slopeOfSegmentB);

            var segmentAParameterizationValue = (intersectionXValue - x1) / (x3 - x1);
            var segmentBParameterizationValue = (intersectionXValue - x2) / (x4 - x2);

            return (segmentAParameterizationValue, segmentBParameterizationValue);
        }

        public void InsertExtraBeads()
        {
            if (pairsThatIntersect.Count <= 1) return;

            var intersectionParameterizationValues = new List<float>();
            foreach (var intersectingPair in pairsThatIntersect)
            {
                var (thisSegmentValue, _) = GetIntersectionParameterizationValues(intersectingPair);
                intersectionParameterizationValues.Add(thisSegmentValue);
            }

            intersectionParameterizationValues.Sort();

            var currentBead = first;
            for (var i = 0; i < intersectionParameterizationValues.Count - 1; i++)
            {
                var nextBeadParameterizationValue =
                    (intersectionParameterizationValues[i] + intersectionParameterizationValues[i + 1]) / 2f;
                var nextBead = GetBeadAtValue(nextBeadParameterizationValue);
                
                currentBead.InsertAfter(nextBead);
                currentBead = nextBead;
            }
        }

        private PDCodeBead GetBeadAtValue(float t)
        {
            var newPosition = first.bead.position + t * (second.bead.position - first.bead.position);
            return new PDCodeBead(new Bead(newPosition), null, null);
        }

        private bool IsBeadPairAdjacent(PDCodeBeadPair other)
        {
            if (
                first == other.first ||
                first == other.second ||
                second == other.first ||
                second == other.second
            ) return true;

            return false;
        }
    }
}