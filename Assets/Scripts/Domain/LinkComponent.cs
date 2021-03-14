using System.Collections.Generic;

namespace Domain
{
    public class LinkComponent
    {
        public readonly List<Bead> BeadList;

        public LinkComponent(List<Bead> beadList)
        {
            BeadList = beadList;
        }
    }
}