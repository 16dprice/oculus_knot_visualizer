using UnityEngine;

namespace LinkRelaxing
{
    public struct Segment
    {
        public Segment(Vector3 P0, Vector3 P1)
        {
            this.P0 = P0;
            this.P1 = P1;
        }
        
        public Vector3 P0 { get; }
        public Vector3 P1 { get; }
    }
}