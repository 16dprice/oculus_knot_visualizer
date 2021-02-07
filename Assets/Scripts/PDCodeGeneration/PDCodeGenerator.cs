using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace PDCodeGeneration
{
    public class PDCodeGenerator
    {
        public static void Main()
        {
            int crossingNumber = 8;
            int ordering = 21;
            int numComponents = 1;

            var generator = new PDCodeGenerator(new DefaultFileBeadsProvider(crossingNumber, ordering, numComponents));
            generator.PrintInfo();
        }

        private readonly ILinkBeadsProvider _beadsProvider;
        private List<Component> _componentList;

        public PDCodeGenerator(ILinkBeadsProvider provider)
        {
            SetComponentList(provider);
        }

        public void PrintInfo()
        {
            Debug.Log(GetPDList());
        }

        private void SetComponentList(ILinkBeadsProvider provider)
        {
            var beadsList = provider.GetBeadsList();
            var componentList = new List<Component>();

            for (int componentIndex = 0; componentIndex < beadsList.Count; componentIndex++)
            {
                componentList.Add(
                    new Component(
                        beadsList[componentIndex].Select(beadVector => new Bead(beadVector)).ToList(),
                        componentIndex
                    )
                );
            }

            _componentList = componentList;
        }

        private List<CrossingPair> GetPDList()
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

            foreach (var crossingPair in crossingPairs)
            {
                Debug.Log(crossingPair.GetPrintString());
            }

            return crossingPairs;
        }

        private void SetBeadStrands()
        {
            int initialStrandNumber = 1;
            foreach (var component in _componentList)
            {
                var exceptionList = new List<Component>() {component};

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
            {
                foreach (var beadPair in component.BeadPairs)
                {
                    if (beadPair.DoesHaveCrossing())
                    {
                        pairsList.Add(beadPair);
                    }
                }
            }

            return pairsList;
        }
    }
}