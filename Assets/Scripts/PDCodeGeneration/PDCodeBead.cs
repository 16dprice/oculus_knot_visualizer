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
        
        public int order { get; }
        
        public PDCodeBead(Bead bead, int componentIndex, int order)
        {
            this.componentIndex = componentIndex;
            position = bead.position;
            this.order = order;
        }
        
        public bool IsBeadAdjacent(PDCodeBead other, int numBeadsInThisComponent)
        {
            if (componentIndex != other.componentIndex) return false;

            if (order == 0 && other.order == numBeadsInThisComponent - 1) return true;
            if (order == numBeadsInThisComponent - 1 && other.order == 0) return true;
            if (Math.Abs(order - other.order) == 1) return true;

            return false;
        }
    }
}