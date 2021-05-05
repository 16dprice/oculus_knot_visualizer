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
            
            Debug.Log(circularLinkedList.length);
            
            Assert.That(true);
        }
    }
}