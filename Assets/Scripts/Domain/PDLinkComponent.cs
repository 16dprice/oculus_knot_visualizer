using System.Collections.Generic;

namespace Domain
{
    public class PDLinkComponent
    {
        public readonly List<Bead> BeadList;

        public List<int> CrossingBeads;
        
        public PDLinkComponent(List<int> crossingBeads)
        {
            CrossingBeads = crossingBeads;
        }
    }
}