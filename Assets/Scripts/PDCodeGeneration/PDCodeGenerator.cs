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
            int crossingNumber = 6;
            int ordering = 3;
            int numComponents = 2;

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
            Debug.Log(GetNumCrossings());
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

        private int GetNumCrossings()
        {
            int numCrossings = 0;
            int currentStrand = 1;
            var crossingPairs = new List<CrossingPair>();

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

            return numCrossings;
        }

        [CanBeNull]
        private CrossingPair GetCrossingPair(
            PDCodeBeadPair start,
            List<PDCodeBeadPair> allBeadPairs,
            int numBeadsInStartComponent
        )
        {
            foreach (var beadPair in allBeadPairs)
            {
                if (start.DoesIntersectOtherBeadPair(beadPair, numBeadsInStartComponent))
                {
                    return new CrossingPair(start, beadPair);
                }
            }

            return null;
        }

        private List<PDCodeBeadPair> GetBeadPairs()
        {
            var pairsList = new List<PDCodeBeadPair>();

            foreach (var component in _componentList)
            {
                pairsList = pairsList.Concat(component.BeadPairs).ToList();
            }

            return pairsList;
        }

        // private int GetNumCrossings()
        // {
        //     var flatList = GetBeadPairs();
        //
        //     var numCrossings = 0;
        //     var currentStrand = 1;
        //     var crossingList = new List<CrossingPair>();
        //     foreach (var firstPair in flatList)
        //     {
        //         firstPair.first.strand = currentStrand;
        //
        //         foreach (var secondPair in flatList)
        //         {
        //             if (firstPair.DoesIntersectOtherBeadPair(secondPair, beadsList[firstPair.componentIndex].Count))
        //             {
        //                 numCrossings++;
        //                 currentStrand++;
        //
        //                 crossingList.Add(new CrossingPair(firstPair, secondPair));
        //             }
        //         }
        //     }
        //
        //     numCrossings /= 2;
        //
        //     foreach (var pair in flatList)
        //     {
        //         if (pair.first.strand > 2 * numCrossings)
        //         {
        //             pair.first.strand -= 2 * numCrossings;
        //         }
        //
        //         Debug.Log(pair.first.strand);
        //     }
        //
        //     foreach (var crossingPair in crossingList)
        //     {
        //         var pdCodeCrossing = crossingPair.GetPDCodeCrossing();
        //         Debug.Log(
        //             $"{pdCodeCrossing.strand1}" +
        //             $"{pdCodeCrossing.strand2}" +
        //             $"{pdCodeCrossing.strand3}" +
        //             $"{pdCodeCrossing.strand4}"
        //         );
        //     }
        //
        //     // return numCrossings;
        //     return -1;
        // }
    }
}