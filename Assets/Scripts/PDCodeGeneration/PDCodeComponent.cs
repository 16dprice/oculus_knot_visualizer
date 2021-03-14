using System.Collections.Generic;
using System.Linq;
using Domain;

namespace PDCodeGeneration
{
    public class PDCodeComponent
    {
        public readonly List<PDCodeBeadPair> BeadPairs;
        private readonly List<PDCodeBead> _beadList;

        public PDCodeComponent(LinkComponent linkComponent, int componentIndex)
        {
            _beadList = GetPDCodeBeads(linkComponent.BeadList, componentIndex);
            BeadPairs = GetBeadPairs();
        }

        public int GetMaxStrandNumber()
        {
            int maxStrandNumber = 1;
            
            foreach (var bead in _beadList)
            {
                if (bead.strand > maxStrandNumber)
                {
                    maxStrandNumber = bead.strand;
                }
            }

            return maxStrandNumber;
        }

        public void SetBeadStrands(int initialStrandNumber, List<PDCodeComponent> otherComponents)
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
                    if (beadPair.DoesIntersectOtherBeadPair(otherBeadPair, _beadList.Count))
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

            for (int beadIndex = 0; beadIndex < _beadList.Count - 1; beadIndex++)
                pdCodeBeadPairList.Add(new PDCodeBeadPair(_beadList[beadIndex], _beadList[beadIndex + 1]));
            pdCodeBeadPairList.Add(new PDCodeBeadPair(_beadList[_beadList.Count - 1], _beadList[0]));

            return pdCodeBeadPairList;
        }
    }
}