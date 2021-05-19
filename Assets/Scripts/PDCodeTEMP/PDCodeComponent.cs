using System.Collections.Generic;
using Domain;
using UnityEngine;

namespace PDCodeTEMP
{
    public class PDCodeComponent
    {
        public PDCodeBead firstBead;
        public int length;
        
        public PDCodeComponent(List<Bead> beads)
        {
            length = beads.Count;
            firstBead = new PDCodeBead(beads[0], null, null);
            
            var currentBead = firstBead;
            for(var i = 1; i < beads.Count; i++)
            {
                var nextBead = new PDCodeBead(beads[i], currentBead, null);
                
                currentBead.SetNext(nextBead);
                currentBead = nextBead;
            }
            
            currentBead.SetNext(firstBead);
            firstBead.SetPrev(currentBead);
        }

        public List<PDCodeBead> ToList()
        {
            RecalculateLength();
            var allBeads = new List<PDCodeBead>();
        
            var count = 0;
            var currentBead = firstBead;
        
            while (count < length)
            {
                allBeads.Add(currentBead);
                currentBead = currentBead.GetNext();
                
                count++;
            }

            return allBeads;
        }

        public List<PDCodeBeadPair> GetAllPDCodeBeadPairs()
        {
            var allPairs = new List<PDCodeBeadPair>();
        
            var count = 0;
            var currentBead = firstBead;
        
            while (count < length)
            {
                allPairs.Add(new PDCodeBeadPair(currentBead, currentBead.GetNext()));
                currentBead = currentBead.GetNext();
                
                count++;
            }
        
            return allPairs;
        }

        private void RecalculateLength()
        {
            var newLength = 1;
            var currentNode = firstBead.GetNext();
            
            while (currentNode != firstBead)
            {
                currentNode = currentNode.GetNext();
                newLength++;
            }

            length = newLength;
        }
    }
}