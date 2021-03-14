using System;
using Domain;

namespace LinkRelaxing
{
    public class LinkRelaxingBead
    {
        public Bead bead;
        public readonly int componentIndex;

        private readonly int _order;

        public LinkRelaxingBead(Bead bead, int componentIndex, int order)
        {
            this.bead = bead;
            this.componentIndex = componentIndex;
            _order = order;
        }

        public bool IsBeadAdjacent(LinkRelaxingBead other, int numBeadsInThisComponent)
        {
            if (componentIndex != other.componentIndex) return false;

            if (_order == 0 && other._order == numBeadsInThisComponent - 1) return true;
            if (_order == numBeadsInThisComponent - 1 && other._order == 0) return true;
            if (Math.Abs(_order - other._order) == 1) return true;

            return false;
        }
    }
}