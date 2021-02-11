using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBeadsProvider: ILinkBeadsProvider
{
    public Vector3[] Knot;

    public DrawBeadsProvider(List<Vector3> k)
    {
        Knot = k.ToArray();
    }

    public List<Vector3[]> GetBeadsList()
    {
        return new List<Vector3[]>(){Knot};
    }
}
