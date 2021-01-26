using System.Collections.Generic;
using UnityEngine;

namespace BeadsCircularLinkedList
{
    public class BeadsCircularLinkedList
    {
        private Vector3[] _beadsVectorList;
        
        public readonly int Count;
        
        private BeadsCircularLinkedList(Vector3[] beadsVectorList)
        {
            _beadsVectorList = beadsVectorList;
            init();
        }

        private void init()
        {
            foreach (var beadVector in _beadsVectorList)
            {
                
            }
        }
    }
}