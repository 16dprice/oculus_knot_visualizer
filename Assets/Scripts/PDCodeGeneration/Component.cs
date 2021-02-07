using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PDCodeGeneration
{
    public class Component
    {
        public readonly List<PDCodeBeadPair> BeadPairs;
        public List<PDCodeBead> BeadList;

        public Component(List<Bead> beadList, int componentIndex)
        {
            BeadList = GetPDCodeBeads(beadList, componentIndex);
            BeadPairs = GetBeadPairs();
        }

        public int GetNumBeads()
        {
            return BeadList.Count;
        }

        public int GetMaxStrandNumber()
        {
            int maxStrandNumber = 1;
            
            foreach (var bead in BeadList)
            {
                if (bead.strand > maxStrandNumber)
                {
                    maxStrandNumber = bead.strand;
                }
            }

            return maxStrandNumber;
        }

        public void SetBeadStrands(int initialStrandNumber, List<Component> otherComponents)
        {
            var currentStrand = initialStrandNumber;
            var allBeadPairs = new List<PDCodeBeadPair>();
            
            allBeadPairs = allBeadPairs.Concat(BeadPairs).ToList();
            foreach (var component in otherComponents)
                allBeadPairs = allBeadPairs.Concat(component.BeadPairs).ToList();

            foreach (var beadPair in BeadPairs)
            {
                beadPair.first.strand = currentStrand;

                foreach (var otherBeadPair in allBeadPairs)
                {
                    if (beadPair.DoesIntersectOtherBeadPair(otherBeadPair, GetNumBeads()))
                    {
                        currentStrand++;
                        break;
                    }
                }
            }

            foreach (var beadPair in BeadPairs)
            {
                // current strand is now 1 greater than the number of strands
                // thus that strand should wrap back around to the initial strand number
                if (beadPair.first.strand == currentStrand)
                {
                    beadPair.first.strand = initialStrandNumber;
                }
            }
        }

        private List<PDCodeBead> GetPDCodeBeads(List<Bead> beadList, int componentIndex)
        {
            var pdCodeBeadList = new List<PDCodeBead>();

            for (int beadIndex = 0; beadIndex < beadList.Count; beadIndex++)
            {
                var pdCodeBead = new PDCodeBead(beadList[beadIndex], componentIndex, beadIndex);
                pdCodeBeadList.Add(pdCodeBead);
            }

            return pdCodeBeadList;
        }

        private List<PDCodeBeadPair> GetBeadPairs()
        {
            var pdCodeBeadPairList = new List<PDCodeBeadPair>();

            for (int beadIndex = 0; beadIndex < BeadList.Count - 1; beadIndex++)
            {
                pdCodeBeadPairList.Add(new PDCodeBeadPair(BeadList[beadIndex], BeadList[beadIndex + 1]));
            }
            pdCodeBeadPairList.Add(new PDCodeBeadPair(BeadList[BeadList.Count - 1], BeadList[0]));

            return pdCodeBeadPairList;
        }
    }
}