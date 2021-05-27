using System.Collections;
using System.Collections.Generic;
using BeadsProviders;
using UnityEngine;
using PDCodeGeneration;
using Domain;

namespace StrandPassage
{
    public static class PerspectiveCrossingDetection
    {
        //TODO: change perspective based on camera position and rotation
        
        public static int[][] SortCrossingBeads(ILinkBeadsProvider beadsProvider, int numberOfComponents)
        {
            var generator = new PDCodeGenerator(beadsProvider);
            var crossingPairs = generator.GetPDList();
            
            var pdLinkComponents = new List<PDLinkComponent>(numberOfComponents);
            
            for (int i = 0; i < numberOfComponents; i++)
            {
                pdLinkComponents.Add(new PDLinkComponent(new List<int>()));
            }

            foreach (var crossingPair in crossingPairs)
            {
                var beadPair = crossingPair.over;
                pdLinkComponents[beadPair.componentIndex].CrossingBeads.Add(beadPair.first.order);
                pdLinkComponents[beadPair.componentIndex].CrossingBeads.Add(beadPair.second.order);
                
                beadPair = crossingPair.under;
                pdLinkComponents[beadPair.componentIndex].CrossingBeads.Add(beadPair.first.order);
                pdLinkComponents[beadPair.componentIndex].CrossingBeads.Add(beadPair.second.order);
            }
                
            var linkCrossingBeadIndexes = new int[numberOfComponents][];

            for (int i = 0; i < numberOfComponents; i++)
            {
                linkCrossingBeadIndexes[i] = pdLinkComponents[i].CrossingBeads.ToArray();
            }

            return linkCrossingBeadIndexes;
        }
    }
}