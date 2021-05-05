using System.Collections.Generic;
using System.Linq;
using BeadsProviders;
using Domain;

namespace PDCodeGeneration
{
    public class PDCodeGenerator
    {
        private readonly ILinkBeadsProvider _beadsProvider;
        private List<PDCodeComponent> _componentList;

        public PDCodeGenerator(ILinkBeadsProvider provider)
        {
            SetComponentList(provider);
        }

        private void SetComponentList(ILinkBeadsProvider provider)
        {
            var linkComponentList = provider.GetLinkComponents();
            var pdCodeComponentList = new List<PDCodeComponent>();

            for (int componentIndex = 0; componentIndex < linkComponentList.Count; componentIndex++)
            {
                pdCodeComponentList.Add(
                    new PDCodeComponent(
                        linkComponentList[componentIndex],
                        componentIndex
                    )
                );
            }

            _componentList = pdCodeComponentList;
        }

        public List<CrossingPair> GetPDList()
        {
            SetBeadStrands();

            var beadPairsWithCrossings = GetBeadPairsWithCrossings();
            var crossingPairs = new List<CrossingPair>();

            for (int i = 0; i < beadPairsWithCrossings.Count - 1; i++)
            {
                for (int j = i + 1; j < beadPairsWithCrossings.Count; j++)
                {
                    var firstBeadPair = beadPairsWithCrossings[i];
                    var secondBeadPair = beadPairsWithCrossings[j];

                    if (firstBeadPair.DoesIntersectOtherBeadPair(secondBeadPair))
                    {
                        crossingPairs.Add(new CrossingPair(firstBeadPair, secondBeadPair));
                        break;
                    }
                }
            }

            return crossingPairs;
        }

        private void SetBeadStrands()
        {
            int initialStrandNumber = 1;
            foreach (var component in _componentList)
            {
                var exceptionList = new List<PDCodeComponent>() {component};

                component.SetBeadStrands(
                    initialStrandNumber,
                    _componentList.Except(exceptionList).ToList()
                );

                initialStrandNumber = component.GetMaxStrandNumber() + 1;
            }
        }

        private List<PDCodeBeadPair> GetBeadPairsWithCrossings()
        {
            var pairsList = new List<PDCodeBeadPair>();

            foreach (var component in _componentList)
                foreach (var beadPair in component.BeadPairs)
                    if (beadPair.DoesHaveCrossing())
                        pairsList.Add(beadPair);

            return pairsList;
        }
    }
}