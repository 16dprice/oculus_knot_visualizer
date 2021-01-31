using System.Collections.Generic;

namespace PDCodeGeneration
{
    public class Component
    {
        public readonly List<PDCodeBeadPair> BeadPairs;
        private List<PDCodeBead> _beadList;

        public Component(List<Bead> beadList, int componentIndex)
        {
            _beadList = GetPDCodeBeads(beadList, componentIndex);
            BeadPairs = GetBeadPairs();
        }

        public int GetNumBeads()
        {
            return _beadList.Count;
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
            {
                pdCodeBeadPairList.Add(new PDCodeBeadPair(_beadList[beadIndex], _beadList[beadIndex + 1]));
            }
            pdCodeBeadPairList.Add(new PDCodeBeadPair(_beadList[_beadList.Count - 1], _beadList[0]));

            return pdCodeBeadPairList;
        }
    }
}