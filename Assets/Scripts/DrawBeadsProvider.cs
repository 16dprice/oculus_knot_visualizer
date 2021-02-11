using System.Collections.Generic;
using UnityEngine;

public class DrawBeadsProvider: ILinkBeadsProvider
{
    private List<Vector3[]> link;

    public DrawBeadsProvider(List<Vector3[]> l) => link = l;

    public List<Vector3[]> GetBeadsList() => link;
}
