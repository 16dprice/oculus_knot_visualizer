using System;
using Domain;
using UnityEngine;

namespace PDCodeGeneration
{
    public class PDCodeBead
    {
        public Vector3 position;
        public readonly int componentIndex;
        public int strand;
        
        private readonly int _order;
        
        public PDCodeBead(Bead bead, int componentIndex, int order)
        {
            this.componentIndex = componentIndex;
            position = bead.position;
            _order = order;
        }
        
        public bool IsBeadAdjacent(PDCodeBead other, int numBeadsInThisComponent)
        {
            if (componentIndex != other.componentIndex) return false;

            if (_order == 0 && other._order == numBeadsInThisComponent - 1) return true;
            if (_order == numBeadsInThisComponent - 1 && other._order == 0) return true;
            if (Math.Abs(_order - other._order) == 1) return true;

            return false;
        }
    }
}