using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PDCodeTEMP
{
    public class PDCodeLink
    {
        public List<PDCodeComponent> componentList;

        public PDCodeLink(List<PDCodeComponent> componentList)
        {
            this.componentList = componentList;
            SetComponentsWithStrandsSet();
        }

        public List<CrossingPair> GetCrossingPairs()
        {
            var crossingPairs = new List<CrossingPair>();
            
            var beadPairs = GetAllPDCodeBeadPairs();
            SetBeadPairIntersectingPairs(beadPairs);
            
            foreach (var beadPair in beadPairs)
            {
                if (beadPair.pairsThatIntersect.Count == 1)
                {
                    crossingPairs.Add(new CrossingPair(beadPair, beadPair.pairsThatIntersect[0]));
                    beadPair.pairsThatIntersect[0].pairsThatIntersect = new List<PDCodeBeadPair>();
                }
            }

            foreach (var pair in crossingPairs)
            {
                Debug.Log(pair.GetPrintString());
            }
            
            return crossingPairs;
        }

        private void SetComponentsWithStrandsSet()
        {
            InsertExtraBeads();

            var components = new List<PDCodeComponent>();
            var beadPairs = GetAllPDCodeBeadPairs();
            var startStrandNumber = 1;
            var currentStrand = startStrandNumber;
            var maxStrandNumber = startStrandNumber;

            SetBeadPairIntersectingPairs(beadPairs);

            foreach (var component in componentList)
            {
                var currentBead = component.firstBead;
                for (var beadIndex = 0; beadIndex < component.length; beadIndex++, currentBead = currentBead.GetNext())
                {
                    var currentBeadPair = beadPairs.Where(
                        beadPair => beadPair.first == currentBead && beadPair.second == currentBead.GetNext()
                    );

                    if (currentBeadPair.First().pairsThatIntersect.Count == 1) maxStrandNumber++;
                }

                maxStrandNumber--;

                currentBead = component.firstBead;
                for (var beadIndex = 0; beadIndex < component.length; beadIndex++, currentBead = currentBead.GetNext())
                {
                    currentBead.strand = currentStrand;

                    var currentBeadPair = beadPairs.First(
                        beadPair =>
                            beadPair.first == currentBead && beadPair.second == currentBead.GetNext() ||
                            beadPair.first == currentBead.GetNext() && beadPair.second == currentBead
                    );

                    if (currentBeadPair.pairsThatIntersect.Count == 1)
                    {
                        if (currentStrand == maxStrandNumber) currentStrand = startStrandNumber;
                        else currentStrand++;
                    }
                }

                startStrandNumber = maxStrandNumber + 1;
                currentStrand = startStrandNumber;
                maxStrandNumber = startStrandNumber;
                
                components.Add(component);
            }

            componentList = components;
        }

        private void InsertExtraBeads()
        {
            var beadPairs = GetAllPDCodeBeadPairs();
            SetBeadPairIntersectingPairs(beadPairs);

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

                beadPairs = GetAllPDCodeBeadPairs();
                SetBeadPairIntersectingPairs(beadPairs);
            }
        }

        private bool NeedsExtraBeads(List<PDCodeBeadPair> beadPairs) =>
            beadPairs.Any(beadPair => beadPair.pairsThatIntersect.Count > 1);

        private void SetBeadPairIntersectingPairs(List<PDCodeBeadPair> allBeadPairs)
        {
            for (var i = 0; i < allBeadPairs.Count; i++)
            {
                allBeadPairs[i].ClearIntersectingPairs();

                for (var j = 0; j < allBeadPairs.Count; j++)
                {
                    if (i == j) continue;
                    if (allBeadPairs[i].DoesIntersectOtherBeadPair(allBeadPairs[j])) 
                        allBeadPairs[i].pairsThatIntersect.Add(allBeadPairs[j]);
                }
            }
        }

        private List<PDCodeBeadPair> GetAllPDCodeBeadPairs()
        {
            var allBeadPairs = new List<PDCodeBeadPair>();
            foreach (var component in componentList) allBeadPairs.AddRange(component.GetAllPDCodeBeadPairs());
            return allBeadPairs;
        }
    }
}