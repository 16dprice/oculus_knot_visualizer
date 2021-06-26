using System;
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
        
        public static int[][] SortCrossingBeads(ILinkBeadsProvider beadsProvider, int numberOfComponents, Transform centerEyeAnchorTransform, Transform strandPassObjTransform)
        {
            var orthoLinkComponents = beadsProvider.GetLinkComponents();
            var perspectiveLinkComponents = new List<LinkComponent>();
            
            var cameraAntiRotation = Quaternion.Euler(-1* centerEyeAnchorTransform.rotation.eulerAngles);
            var cameraAntiPosition = -1* centerEyeAnchorTransform.position;
            
            foreach (var component in orthoLinkComponents)
            {
                var beadList = component.BeadList;
                var perspectiveBeadList = new List<Bead>();

                foreach (var bead in beadList)
                {
                    var movedBead = bead.position + cameraAntiPosition;
                    
                    var rotatedMovedBead = cameraAntiRotation * movedBead;

                    var increasingFactor = IncreasingFactor(rotatedMovedBead.z);

                    rotatedMovedBead.x *= increasingFactor;
                    rotatedMovedBead.y *= increasingFactor;

                    perspectiveBeadList.Add(new Bead(rotatedMovedBead));
                }
                
                perspectiveLinkComponents.Add(new LinkComponent(perspectiveBeadList));
            }

            var perspectiveBeadsProvider = new StrandPassProvider(perspectiveLinkComponents);
            
            var generator = new PDCodeGenerator(perspectiveBeadsProvider);
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

        private static float IncreasingFactor(float depth)
        {
            return Mathf.Atan(1 / depth);
        }
    }
}