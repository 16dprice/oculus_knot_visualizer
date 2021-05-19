using System.Collections.Generic;
using System.Linq;

namespace PDCodeTEMP
{
    public class PDCodeLink
    {
        public List<PDCodeComponent> componentList;
        
        public PDCodeLink(List<PDCodeComponent> componentList)
        {
            this.componentList = componentList;
        }

        public void InsertExtraBeads()
        {
            var beadPairs = SetBeadPairIntersectingPairs();
            while (NeedsExtraBeads(beadPairs))
            {
                foreach (var beadPair in beadPairs)
                {
                    if (beadPair.pairsThatIntersect.Count > 1)
                    {
                        beadPair.InsertExtraBeads();
                        break;
                    }
                }
                beadPairs = SetBeadPairIntersectingPairs();
            }
        }

        private bool NeedsExtraBeads(List<PDCodeBeadPair> beadPairs)
        {
            return beadPairs.Any(beadPair => beadPair.pairsThatIntersect.Count > 1);
        }

        public List<PDCodeBeadPair> SetBeadPairIntersectingPairs()
        {
            var allBeadPairs = GetAllPDCodeBeadPairs();

            for (var i = 0; i < allBeadPairs.Count; i++)
            {
                allBeadPairs[i].ClearIntersectingPairs();
                
                for (var j = 0; j < allBeadPairs.Count; j++)
                {
                    if (i == j) continue;
                    if (allBeadPairs[i].DoesIntersectOtherBeadPair(allBeadPairs[j]))
                    {
                        allBeadPairs[i].pairsThatIntersect.Add(allBeadPairs[j]);
                    }
                }
            }

            return allBeadPairs;
        }

        private List<PDCodeBeadPair> GetAllPDCodeBeadPairs()
        {
            var allBeadPairs = new List<PDCodeBeadPair>();
            foreach (var component in componentList) allBeadPairs.AddRange(component.GetAllPDCodeBeadPairs());
            return allBeadPairs;
        }
    }
}