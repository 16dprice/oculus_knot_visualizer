using Domain;
using LinkRelaxing;

namespace LinkRelaxing
{
    public class Segment
    {
        public readonly Bead P0;
        public readonly Bead P1;

        private readonly LinkRelaxingBead _firstBead;
        private readonly LinkRelaxingBead _secondBead;

        public Segment(LinkRelaxingBead firstBead, LinkRelaxingBead secondBead)
        {
            P0 = firstBead.bead;
            P1 = secondBead.bead;

            _firstBead = firstBead;
            _secondBead = secondBead;
        }

        public bool IsSegmentAdjacent(Segment other)
        {
            if (_firstBead.IsSameBead(other._firstBead)) return true;
            if (_firstBead.IsSameBead(other._secondBead)) return true;
            if (_secondBead.IsSameBead(other._firstBead)) return true;
            if (_secondBead.IsSameBead(other._secondBead)) return true;

            return false;
        }
    }
}