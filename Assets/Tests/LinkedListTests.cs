using System.Collections.Generic;
using BeadsProviders;
using NUnit.Framework;
using PDCodeTEMP;

namespace Tests
{
    public class LinkedListTests
    {
        [Test]
        public void LinkedListTest()
        {
            var beadsProvider = new DefaultFileBeadsProvider(3, 2);

            var componentList = new List<PDCodeComponent>();
            foreach (var component in beadsProvider.GetLinkComponents())
            {
                var pdCodeComponent = new PDCodeComponent(component.BeadList);
                componentList.Add(pdCodeComponent);
            }
            
            var pdCodeLink = new PDCodeLink(componentList);
            
            // foreach (var component in pdCodeLink.componentList)
            // {
            //     var beadList = component.ToList();
            //     foreach (var bead in beadList)
            //     {
            //         Debug.Log($"{bead.bead.position}");
            //     }
            // }
            
            // pdCodeLink.InsertExtraBeads();
            // Debug.Log("--------------------------------\n");
            
            // foreach (var component in pdCodeLink.componentList)
            // {
            //     var beadList = component.ToList();
            //     foreach (var bead in beadList)
            //     {
            //         Debug.Log($"{bead.bead.position}");
            //     }
            // }
            
            pdCodeLink.GetCrossingPairs();

            Assert.That(true);
        }
    }
}