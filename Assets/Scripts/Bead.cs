using System;
using UnityEngine;

public class Bead
{
    public Vector3 position;
    public int componentIndex;
    private int _order;
    
    public Bead(Vector3 position, int componentIndex, int order)
    {
        this.position = position;
        this.componentIndex = componentIndex;
        _order = order;
    }

    public bool IsBeadAdjacent(Bead other, int numBeadsInThisComponent)
    {
        if (componentIndex != other.componentIndex) return false;

        if (_order == 0 && other._order == numBeadsInThisComponent - 1) return true;
        if (_order == numBeadsInThisComponent - 1 && other._order == 0) return true;
        if (Math.Abs(_order - other._order) == 1) return true;

        return false;
    }
}