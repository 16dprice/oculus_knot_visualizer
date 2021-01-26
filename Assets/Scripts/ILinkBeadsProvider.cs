using System.Collections.Generic;
using UnityEngine;

public interface ILinkBeadsProvider
{
    List<Vector3[]> GetBeadsList();
}