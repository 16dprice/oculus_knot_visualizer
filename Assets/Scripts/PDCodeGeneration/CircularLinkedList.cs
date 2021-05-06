using System.Collections.Generic;

namespace PDCodeGeneration
{
    public class CircularLinkedList
    {
        public ListNode firstNode;
        public int length;
        
        public CircularLinkedList(List<PDCodeBead> beads)
        {
            length = beads.Count;
            firstNode = new ListNode(beads[0], null, null);
            
            var currentNode = firstNode;
            for (int i = 1; i < beads.Count; i++)
            {
                var nextNode = new ListNode(beads[i], currentNode, null);
                
                currentNode.SetNext(nextNode);
                currentNode = nextNode;
            }
            
            currentNode.SetNext(firstNode);
            firstNode.SetPrev(currentNode);
        }

        public List<PDCodeBeadPair> GetAllPDCodeBeadPairs()
        {
            var allPairs = new List<PDCodeBeadPair>();

            var count = 0;
            var currentNode = firstNode;

            while (count < length)
            {
                allPairs.Add(new PDCodeBeadPair(currentNode.bead, currentNode.next.bead));
                currentNode = currentNode.next;
                
                count++;
            }

            return allPairs;
        }
    }
}