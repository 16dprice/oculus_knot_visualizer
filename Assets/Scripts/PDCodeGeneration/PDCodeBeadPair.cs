namespace PDCodeGeneration
{
    public class PDCodeBeadPair
    {
        public int componentIndex;

        public PDCodeBead first;
        public PDCodeBead second;

        public PDCodeBeadPair(PDCodeBead first, PDCodeBead second)
        {
            this.first = first;
            this.second = second;

            componentIndex = first.componentIndex;
        }

        public bool DoesIntersectOtherBeadPair(PDCodeBeadPair other, int numBeadsInThisComponent)
        {
            if (IsBeadPairAdjacent(other, numBeadsInThisComponent)) return false;

            var (thisSegmentValue, otherSegmentValue) = GetIntersectionParameterizationValues(other);

            var isOnThisSegment = false;
            var isOnOtherSegment = false;

            if (0 < thisSegmentValue && thisSegmentValue < 1) isOnThisSegment = true;
            if (0 < otherSegmentValue && otherSegmentValue < 1) isOnOtherSegment = true;

            return isOnThisSegment && isOnOtherSegment;
        }

        public (float thisSegmentValue, float otherSegmentValue) GetIntersectionParameterizationValues(
            PDCodeBeadPair other
        )
        {
            var x1 = first.position.x;
            var y1 = first.position.y;
            var x3 = second.position.x;
            var y3 = second.position.y;

            var x2 = other.first.position.x;
            var y2 = other.first.position.y;
            var x4 = other.second.position.x;
            var y4 = other.second.position.y;

            var slopeOfSegmentA = (y3 - y1) / (x3 - x1);
            var slopeOfSegmentB = (y4 - y2) / (x4 - x2);

            var intersectionXValue = (y2 - slopeOfSegmentB * x2 - y1 + slopeOfSegmentA * x1) /
                                     (slopeOfSegmentA - slopeOfSegmentB);

            var segmentAParameterizationValue = (intersectionXValue - x1) / (x3 - x1);
            var segmentBParameterizationValue = (intersectionXValue - x2) / (x4 - x2);

            return (segmentAParameterizationValue, segmentBParameterizationValue);
        }

        private bool IsBeadPairAdjacent(PDCodeBeadPair other, int numBeadsInThisComponent)
        {
            if (
                first.IsBeadAdjacent(other.first, numBeadsInThisComponent) ||
                first.IsBeadAdjacent(other.second, numBeadsInThisComponent) ||
                second.IsBeadAdjacent(other.first, numBeadsInThisComponent) ||
                second.IsBeadAdjacent(other.second, numBeadsInThisComponent)
            ) return true;

            return false;
        }
    }
}