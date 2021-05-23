using System.Collections.Generic;
using System.Linq;
using BeadsProviders;
using Domain;
using UI;
using UnityEngine;

namespace StrandPassage
{
    public class StrandPassObject : MonoBehaviour
    {
        public bool DisplayCrossings = false;
        private bool _previousDisplayCrossings = false;
        
        private float radius = 0.5f;
        private int sides = 6;

        private int NumComponents = 1;
        private int CrossingNumber = 3;
        private int Ordering = 1;

        [SerializeField] int FirstStrandComponent = 0;
        [SerializeField] int SecondStrandComponent = 0;
        [SerializeField] int FirstStrandSegment = 0;
        [SerializeField] int SecondStrandSegment = 0;

        private int _previousFirstStrandComponent = 0;
        private int _previousSecondStrandComponent = 0;
        private int _previousFirstStrandSegment = 0;
        private int _previousSecondStrandSegment = 0;

        [SerializeField] private List<int> beadsToHighlight;

        private List<LinkComponent> _linkComponents;
        
        void Start()
        {
            var beadsProvider = new DefaultFileBeadsProvider(CrossingNumber, Ordering, NumComponents);
            _linkComponents = beadsProvider.GetLinkComponents();

            var linkStickModel = new LinkStickModel(_linkComponents);

            MeshManipulation.DisplayLink(transform, linkStickModel, sides, radius);
        }

        private void Update()
        {
            if (
                _previousFirstStrandComponent != FirstStrandComponent ||
                _previousSecondStrandComponent != SecondStrandComponent ||
                _previousFirstStrandSegment != FirstStrandSegment ||
                _previousSecondStrandSegment != SecondStrandSegment
            )
            {
                var strandPassProvider = new StrandPassProvider(
                    _linkComponents,
                    (FirstStrandComponent, FirstStrandSegment),
                    (SecondStrandComponent, SecondStrandSegment)
                );
                var linkStickModel = new LinkStickModel(strandPassProvider);

                MeshManipulation.DisplayLink(transform, linkStickModel, sides, radius);

                _previousFirstStrandComponent = FirstStrandComponent;
                _previousSecondStrandComponent = SecondStrandComponent;
                _previousFirstStrandSegment = FirstStrandSegment;
                _previousSecondStrandSegment = SecondStrandSegment;
            }

            if (_previousDisplayCrossings != DisplayCrossings)
            {
                if (DisplayCrossings)
                {
                    HighlightBeads(beadsToHighlight);
                }
                else
                {
                    HighlightBeads(new List<int>());
                }

                _previousDisplayCrossings = DisplayCrossings;
            }
        }
        
        //only works with knots
        public void HighlightBeads(List<int> whichBeads)
        {
            //exit if beads are out of range
            if (whichBeads.Any(i => (i >= _linkComponents[0].BeadList.Count)||(i < 0))) return;

            foreach (var component in _linkComponents)
            {
                var numberOfBeads = component.BeadList.Count;
                var numberOfVerts = numberOfBeads * sides;
                    
                var uvs = new Vector2[numberOfVerts];
                    
                //initialize all as (1,0)
                for (int vert = 0; vert < numberOfVerts; vert++)
                {
                    uvs[vert] = new Vector2(1.0f, 0.0f);
                }
                    
                //Every other vertex gets (0,0)
                foreach (var t in whichBeads)
                {
                    for (int side = 0; side < sides; side++)
                    {
                        uvs[t * sides + side] = new Vector2(0.0f, 0.0f);
                    }
                }
                    
                //set uvs to the link component game object
                component.ComponentGameObject.GetComponent<MeshFilter>().mesh.uv = uvs;
            }
        }
    }
}