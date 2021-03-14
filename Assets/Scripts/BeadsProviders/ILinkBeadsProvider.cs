using System.Collections.Generic;
using Domain;

namespace BeadsProviders
{
    public interface ILinkBeadsProvider
    {
        List<LinkComponent> GetLinkComponents();
    }
}