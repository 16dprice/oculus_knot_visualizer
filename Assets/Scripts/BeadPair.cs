public class BeadPair
{
    public int componentIndex;
    
    private readonly Bead _first;
    private readonly Bead _second;
    
    public BeadPair(Bead first, Bead second)
    {
        _first = first;
        _second = second;

        componentIndex = first.componentIndex;
    }
    
    public bool DoesIntersectOtherBeadPair(BeadPair other, int numBeadsInThisComponent)
    {
        if (IsBeadPairAdjacent(other, numBeadsInThisComponent)) return false;
        
        var x1 = _first.position.x;
        var y1 = _first.position.y;
        var x3 = _second.position.x;
        var y3 = _second.position.y;
        
        var x2 = other._first.position.x;
        var y2 = other._first.position.y;
        var x4 = other._second.position.x;
        var y4 = other._second.position.y;

        var slopeOfSegmentA = (y3 - y1) / (x3 - x1);
        var slopeOfSegmentB = (y4 - y2) / (x4 - x2);

        var intersectionXValue = (y2 - slopeOfSegmentB * x2 - y1 + slopeOfSegmentA * x1) /
                                 (slopeOfSegmentA - slopeOfSegmentB);

        var segmentAParameterizationValue = (intersectionXValue - x1) / (x3 - x1);
        var segmentBParameterizationValue = (intersectionXValue - x2) / (x4 - x2);
        
        var isOnSegmentA = false;
        var isOnSegmentB = false;

        if (0 < segmentAParameterizationValue && segmentAParameterizationValue < 1) isOnSegmentA = true;
        if (0 < segmentBParameterizationValue && segmentBParameterizationValue < 1) isOnSegmentB = true;

        return isOnSegmentA && isOnSegmentB;
    }

    private bool IsBeadPairAdjacent(BeadPair other, int numBeadsInThisComponent)
    {
        if (
            _first.IsBeadAdjacent(other._first, numBeadsInThisComponent) ||
            _first.IsBeadAdjacent(other._second, numBeadsInThisComponent) ||
            _second.IsBeadAdjacent(other._first, numBeadsInThisComponent) ||
            _second.IsBeadAdjacent(other._second, numBeadsInThisComponent)
        ) return true;

        return false;
    }
}