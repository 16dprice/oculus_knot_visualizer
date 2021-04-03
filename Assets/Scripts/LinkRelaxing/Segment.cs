using UnityEngine;

namespace LinkRelaxing
{
    public class Segment
    {
        public Vector3 P0;
        public Vector3 P1;

        private LinkRelaxingBead firstBead;
        private LinkRelaxingBead secondBead;

        public Segment(LinkRelaxingBead firstBead, LinkRelaxingBead secondBead)
        {
            P0 = firstBead.bead.position;
            P1 = secondBead.bead.position;

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