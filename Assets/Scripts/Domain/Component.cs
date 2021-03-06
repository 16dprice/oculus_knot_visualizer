using System.Collections.Generic;

namespace Domain
{
    public class Component
    {
        public readonly List<Bead> BeadList;

        public Component(List<Bead> beadList)
        {
            BeadList = beadList;
        }
    }
}