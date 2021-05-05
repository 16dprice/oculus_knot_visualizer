using System.Collections.Generic;
using Domain;
using UnityEngine;

namespace BeadsProviders
{
    public class DrawBeadsProvider: ILinkBeadsProvider
    {
        private readonly List<LinkComponent> _link;

        public DrawBeadsProvider(List<LinkComponent> l) => _link = l;

        public List<LinkComponent> GetLinkComponents() => _link;
    }
}
