using System.Collections.Generic;
using BeadsProviders;
using NUnit.Framework;
using PDCodeGeneration;
using UnityEngine;

namespace Tests
{
    public class LinkedListTests
    {
        [Test]
        public void LinkedListTest()
        {
            var beadsProvider = new DefaultFileBeadsProvider(3, 1);
            var beads = beadsProvider.GetLinkComponents()[0].BeadList;

            var pdCodeBeads = new List<PDCodeBead>();
            for (int i = 0; i < beads.Count; i++)
            {
                pdCodeBeads.Add(new PDCodeBead(beads[i], 1, i));
            }

            var circularLinkedList = new CircularLinkedList(pdCodeBeads);
            var allPairs = circularLinkedList.GetAllPDCodeBeadPairs();
            
            Debug.Log(circularLinkedList.length);
            for (var i = 0; i < allPairs.Count; i++)
            {
                for (var j = i + 1; j < allPairs.Count; j++)
                {
                    if (allPairs[i].DoesIntersectOtherBeadPair(allPairs[j]))
                    {
                        Debug.Log($"{allPairs[i].first.position} & {allPairs[i].second.position}");
                        Debug.Log($"{allPairs[j].first.position} & {allPairs[j].second.position}\n");
                    }
                }
            }
            
            Assert.That(true);
        }
    }
}