using System;
using System.Collections.Generic;
using UnityEngine;

namespace PDCodeGeneration
{
    public class PDCodeGenerator
    {
        public static void Main()
        {
            var generator = new PDCodeGenerator(new DefaultFileBeadsProvider(6, 1, 2));
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
            var flatList = new List<BeadPair>();

            foreach (var beadList in beadsList)
            {
                for (int i = 0; i < beadList.Count - 1; i++)
                {
                    flatList.Add(new BeadPair(beadList[i], beadList[i + 1]));
                }

                flatList.Add(new BeadPair(beadList[beadList.Count - 1], beadList[0]));
            }

            var numCrossings = 0;
            foreach (var firstPair in flatList)
            {
                foreach (var secondPair in flatList)
                {
                    if (firstPair.DoesIntersectOtherBeadPair(secondPair, beadsList[firstPair.componentIndex].Count))
                        numCrossings++;
                }
            }

            return numCrossings / 2;
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