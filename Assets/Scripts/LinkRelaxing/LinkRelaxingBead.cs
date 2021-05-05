using System;
using Domain;

namespace LinkRelaxing
{
    public class LinkRelaxingBead
    {
        public Bead bead;
        public readonly int componentIndex;
        public readonly int numBeadsInThisComponent;

        private readonly int _order;

        public LinkRelaxingBead(Bead bead, int componentIndex, int order, int numBeadsInThisComponent)
        {
            this.bead = bead;
            this.componentIndex = componentIndex;
            this.numBeadsInThisComponent = numBeadsInThisComponent;
            _order = order;
        }
        
        public bool IsSameBead(LinkRelaxingBead other)
        {
            return componentIndex == other.componentIndex && _order == other._order;
        }

        public bool IsBeadAdjacent(LinkRelaxingBead other)
        {
            if(IsSameBead(other)) return false;
            if (componentIndex != other.componentIndex) return false;

            if (_order == 0 && other._order == numBeadsInThisComponent - 1) return true;
            if (_order == numBeadsInThisComponent - 1 && other._order == 0) return true;
            if (Math.Abs(_order - other._order) == 1) return true;

            return false;
        }
    }
}