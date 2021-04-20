using BeadsProviders;
using UI;
using UnityEngine;

namespace StrandPassage
{
    public class StrandPassObject : MonoBehaviour
    {
        private float radius = 0.5f;
        private int sides = 6;
    
        private int NumComponents = 1;
        private int CrossingNumber = 3;
        private int Ordering = 1;

        [SerializeField] int FirstStrandComponent = 0;
        [SerializeField] int SecondStrandComponent = 0;
        [SerializeField] int FirstStrandSegment = 0;
        [SerializeField] int SecondStrandSegment = 0;

        private int _previousFirstStrandSegment = 0;
        private int _previousSecondStrandSegment = 0;
        private int _previousFirstStrandComponent = 0;
        private int _previousSecondStrandComponent = 0;
        void Start()
        {
            var beadsProvider = new DefaultFileBeadsProvider(CrossingNumber, Ordering, NumComponents);
            var linkStickModel = new LinkStickModel(beadsProvider);
            
            MeshManipulation.DisplayLink(transform, linkStickModel, sides, radius);
        }

        private void Update()
        {
            if (
                _previousFirstStrandSegment != FirstStrandSegment ||
                _previousSecondStrandSegment != SecondStrandSegment ||
                _previousFirstStrandComponent != FirstStrandComponent ||
                _previousSecondStrandComponent != SecondStrandComponent
                )
            {
                var beadsProvider = new DefaultFileBeadsProvider(CrossingNumber, Ordering, NumComponents);
                var strandPassProvider = new StrandPassProvider(beadsProvider.GetLinkComponents(),
                    (FirstStrandComponent, FirstStrandSegment), (SecondStrandComponent, SecondStrandSegment));
                var linkStickModel = new LinkStickModel(strandPassProvider);
            
                MeshManipulation.DisplayLink(transform, linkStickModel, sides, radius);
                
                _previousFirstStrandSegment = FirstStrandSegment;
                _previousSecondStrandSegment = SecondStrandSegment;
                _previousFirstStrandComponent = FirstStrandComponent;
                _previousSecondStrandComponent = SecondStrandComponent;
            }
        }
    }   
}
