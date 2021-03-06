using System.Collections.Generic;
using UnityEngine;

namespace BeadsProviders
{
    public interface ILinkBeadsProvider
    {
        List<Vector3[]> GetBeadsList();
    }
}