using System;
using System.Collections.Generic;
using UnityEngine;

namespace PDCodeGeneration
{
    public class PDCodeGenerator
    {
        public static void Main()
        {
            var generator = new PDCodeGenerator(new DefaultFileBeadsProvider(2, 1, 2));
            generator.PrintInfo();
        }

        private readonly ILinkBeadsProvider _beadsProvider;

        public PDCodeGenerator(ILinkBeadsProvider provider)
        {
            _beadsProvider = provider;
        }

        public void PrintInfo()
        {
            Debug.Log(GetNumCrossings());
        }

        private int GetNumCrossings()
        {
            var beadsList = GetBeadsList();
            var flatList = new List<PDCodeBeadPair>();

            foreach (var beadList in beadsList)
            {
                for (int i = 0; i < beadList.Count - 1; i++)
                {
                    flatList.Add(new PDCodeBeadPair(beadList[i], beadList[i + 1]));
                }

                flatList.Add(new PDCodeBeadPair(beadList[beadList.Count - 1], beadList[0]));
            }

            var numCrossings = 0;
            var currentStrand = 1;
            var crossingList = new List<CrossingPair>();
            foreach (var firstPair in flatList)
            {
                firstPair.first.strand = currentStrand;

                foreach (var secondPair in flatList)
                {
                    if (firstPair.DoesIntersectOtherBeadPair(secondPair, beadsList[firstPair.componentIndex].Count))
                    {
                        numCrossings++;
                        currentStrand++;

                        crossingList.Add(new CrossingPair(firstPair, secondPair));
                    }
                }
            }

            numCrossings /= 2;

            foreach (var pair in flatList)
            {
                if (pair.first.strand > 2 * numCrossings)
                {
                    pair.first.strand -= 2 * numCrossings;
                }

                Debug.Log(pair.first.strand);
            }

            foreach (var crossingPair in crossingList)
            {
                var pdCodeCrossing = crossingPair.GetPDCodeCrossing();
                Debug.Log(
                    $"{pdCodeCrossing.strand1}" +
                    $"{pdCodeCrossing.strand2}" +
                    $"{pdCodeCrossing.strand3}" +
                    $"{pdCodeCrossing.strand4}"
                );
            }

            return numCrossings;
        }

        private List<List<PDCodeBead>> GetBeadsList()
        {
            var beadsList = _beadsProvider.GetBeadsList();
            var newBeadsList = new List<List<PDCodeBead>>();

            for (int componentIndex = 0; componentIndex < beadsList.Count; componentIndex++)
            {
                var beadList = beadsList[componentIndex];
                var newBeadList = new List<PDCodeBead>();

                for (int beadIndex = 0; beadIndex < beadList.Length; beadIndex++)
                {
                    newBeadList.Add(
                        new PDCodeBead(new Bead(beadList[beadIndex]), componentIndex, beadIndex)
                    );
                }

                newBeadsList.Add(newBeadList);
            }

            return newBeadsList;
        }
    }
}