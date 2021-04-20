using System.Collections.Generic;
using Domain;

namespace BeadsProviders
{
    public class StrandPassProvider : ILinkBeadsProvider
    {
        private List<LinkComponent> _linkComponents = null;
        
        //(int, int) tuple represents the component and segment number of the strands being passed
        public StrandPassProvider(List<LinkComponent> linkComponents, (int, int) FirstStrandSegment, (int, int) SecondStrandSegment)
        {

            _linkComponents = linkComponents;
        }

        public List<LinkComponent> GetLinkComponents()
        {
            return _linkComponents;
        }
    }
}
