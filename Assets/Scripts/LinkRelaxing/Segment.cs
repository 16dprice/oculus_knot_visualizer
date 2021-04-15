using Domain;
using UnityEngine;

namespace LinkRelaxing
{
    public class Segment
    {
        public Bead P0;
        public Bead P1;

        public LinkRelaxingBead firstBead;
        public LinkRelaxingBead secondBead;

        public Segment(LinkRelaxingBead firstBead, LinkRelaxingBead secondBead)
        {
            P0 = firstBead.bead;
            P1 = secondBead.bead;

            this.firstBead = firstBead;
            this.secondBead = secondBead;
        }

        public bool IsSegmentAdjacent(Segment other)
        {
            if (firstBead.IsSameBead(other.firstBead)) return true;
            if (firstBead.IsSameBead(other.secondBead)) return true;
            if (secondBead.IsSameBead(other.firstBead)) return true;
            if (secondBead.IsSameBead(other.secondBead)) return true;

            return false;
        }
    }
}