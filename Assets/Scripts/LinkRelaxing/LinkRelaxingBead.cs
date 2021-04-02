using System;
using Domain;

namespace LinkRelaxing
{
    public class LinkRelaxingBead
    {
        public Bead bead;
        public readonly int componentIndex;

        private readonly int _order;
        private readonly int _numBeadsInThisComponent;

        public LinkRelaxingBead(Bead bead, int componentIndex, int order, int numBeadsInThisComponent)
        {
            this.bead = bead;
            this.componentIndex = componentIndex;
            _order = order;
            _numBeadsInThisComponent = numBeadsInThisComponent;
        }

        public bool IsBeadAdjacent(LinkRelaxingBead other)
        {
            if (componentIndex == other.componentIndex && _order == other._order) return false;
            if (componentIndex != other.componentIndex) return false;

            if (_order == 0 && other._order == _numBeadsInThisComponent - 1) return true;
            if (_order == _numBeadsInThisComponent - 1 && other._order == 0) return true;
            if (Math.Abs(_order - other._order) == 1) return true;

            return false;
        }
    }
}