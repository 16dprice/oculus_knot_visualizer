using UnityEngine;

namespace BeadsCircularLinkedList
{
    class Bead
    {
        public Vector3 Position { get; }

        public Bead Next;
        public Bead Previous;
        
        public Bead(Vector3 position)
        {
            Position = position;
        }
        
        
    }
}