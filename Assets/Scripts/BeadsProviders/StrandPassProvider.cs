using System.Collections.Generic;
using Domain;
using UnityEngine;

namespace BeadsProviders
{
    public class StrandPassProvider : ILinkBeadsProvider
    {
        private List<LinkComponent> _linkComponents = null;
        
        //(int, int) tuple represents the component and segment number of the strands being passed
        public StrandPassProvider(List<LinkComponent> linkComponents, (int, int) firstStrandSegment, (int, int) secondStrandSegment)
        {
            int componentNumber = firstStrandSegment.Item1 % linkComponents.Count;
            firstStrandSegment = (componentNumber,
                (firstStrandSegment.Item2 + 1) % linkComponents[componentNumber].BeadList.Count);
            
            componentNumber = secondStrandSegment.Item1 % linkComponents.Count;
            secondStrandSegment = (componentNumber,
                (secondStrandSegment.Item2 + 1) % linkComponents[componentNumber].BeadList.Count);
            
            
            if (firstStrandSegment.Item1 == secondStrandSegment.Item1)
            {
                if (firstStrandSegment.Item2 != secondStrandSegment.Item2)
                {
                    var component = firstStrandSegment.Item1;
                    
                    var segmentBeads = linkComponents[component].BeadList;
                    segmentBeads.Insert(firstStrandSegment.Item2, new Bead(Vector3.zero));
                    segmentBeads.Insert(secondStrandSegment.Item2, new Bead(Vector3.zero));
                    
                    linkComponents[component] = new LinkComponent(segmentBeads);
                }
            }
            else
            {
                var firstSegmentBeads = linkComponents[firstStrandSegment.Item1].BeadList;
                var secondSegmentBeads = linkComponents[secondStrandSegment.Item2].BeadList;
                            
                firstSegmentBeads.Insert(firstStrandSegment.Item2, new Bead(Vector3.zero));
                secondSegmentBeads.Insert(secondStrandSegment.Item2, new Bead(Vector3.zero));

                linkComponents[firstStrandSegment.Item1] = new LinkComponent(firstSegmentBeads);
                linkComponents[secondStrandSegment.Item1] = new LinkComponent(secondSegmentBeads);
            }
            

            _linkComponents = linkComponents;
        }

        public List<LinkComponent> GetLinkComponents()
        {
            return _linkComponents;
        }
    }
}
