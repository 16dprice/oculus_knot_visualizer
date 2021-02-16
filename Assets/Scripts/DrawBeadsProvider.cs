using System.Collections.Generic;
using UnityEngine;

public class DrawBeadsProvider: ILinkBeadsProvider
{
    private readonly List<Vector3[]> _link;

    public DrawBeadsProvider(List<Vector3[]> l) => _link = l;

    public List<Vector3[]> GetBeadsList() => _link;
}
