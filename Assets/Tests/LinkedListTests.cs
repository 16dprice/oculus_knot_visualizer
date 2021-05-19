using System.Collections.Generic;
using BeadsProviders;
using NUnit.Framework;
using PDCodeTEMP;
using UnityEngine;

namespace Tests
{
    public class LinkedListTests
    {
        [Test]
        public void LinkedListTest()
        {
            var beadsProvider = new DefaultFileBeadsProvider(3, 2);
            var beads = beadsProvider.GetLinkComponents()[0].BeadList;
            var pdCodeComponent = new PDCodeComponent(beads);
            var pdCodeLink = new PDCodeLink(new List<PDCodeComponent> {pdCodeComponent});
            
            foreach (var component in pdCodeLink.componentList)
            {
                var beadList = component.ToList();
                foreach (var bead in beadList)
                {
                    Debug.Log($"{bead.bead.position}");
                }
            }
            
            pdCodeLink.InsertExtraBeads();
            Debug.Log("--------------------------------\n");
            
            foreach (var component in pdCodeLink.componentList)
            {
                var beadList = component.ToList();
                foreach (var bead in beadList)
                {
                    Debug.Log($"{bead.bead.position}");
                }
            }
            
            

            Assert.That(true);
        }
    }
}