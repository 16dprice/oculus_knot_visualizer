using System.Collections.Generic;
using System.Linq;
using BeadsProviders;
using Domain;
using UI;
using UnityEngine;
using PDCodeGeneration;


namespace StrandPassage
{
    public class StrandPassObject : MonoBehaviour
    {
        private bool displayCrossings = true;
        private bool _previousDisplayCrossings = true;
        
        private float radius = 0.5f;
        private int sides = 6;

        private int NumComponents = 2;
        private int CrossingNumber = 6;
        private int Ordering = 1;

        [SerializeField] int FirstStrandComponent = 0;
        [SerializeField] int SecondStrandComponent = 0;
        [SerializeField] int FirstStrandSegment = 0;
        [SerializeField] int SecondStrandSegment = 0;

        private int _previousFirstStrandComponent = 0;
        private int _previousSecondStrandComponent = 0;
        private int _previousFirstStrandSegment = 0;
        private int _previousSecondStrandSegment = 0;

        private List<LinkComponent> _linkComponents;
        private ILinkBeadsProvider _beadsProvider;

        [SerializeField] private GameObject centerEyeAnchor;
        private Transform _centerEyeAnchorTransform;
        
        void Start()
        {
            _centerEyeAnchorTransform = centerEyeAnchor.transform;
            
            _beadsProvider = new DefaultFileBeadsProvider(CrossingNumber, Ordering, NumComponents);
            _linkComponents = _beadsProvider.GetLinkComponents();

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
                _beadsProvider = new StrandPassProvider(
                    _linkComponents,
                    (FirstStrandComponent, FirstStrandSegment),
                    (SecondStrandComponent, SecondStrandSegment)
                );
                _linkComponents = _beadsProvider.GetLinkComponents();
                
                var linkStickModel = new LinkStickModel(_beadsProvider);
                
                MeshManipulation.DisplayLink(transform, linkStickModel, sides, radius);

                _previousFirstStrandComponent = FirstStrandComponent;
                _previousSecondStrandComponent = SecondStrandComponent;
                _previousFirstStrandSegment = FirstStrandSegment;
                _previousSecondStrandSegment = SecondStrandSegment;
            }

            if (displayCrossings)
            {
                HighlightBeads(_beadsProvider);
                _previousDisplayCrossings = true;
            } 
            else if (_previousDisplayCrossings != displayCrossings)
            {
                UnHighlightBeads(_linkComponents);
                _previousDisplayCrossings = false;
            }
        }

        private void HighlightBeads(ILinkBeadsProvider beadsProvider)
        {
            var whichBeads = PerspectiveCrossingDetection.SortCrossingBeads(beadsProvider, _linkComponents.Count, _centerEyeAnchorTransform, transform);

            for (int i = 0; i < _linkComponents.Count; i++)
            {
                var numberOfBeads = _linkComponents[i].BeadList.Count;
                var numberOfVerts = numberOfBeads * sides;
                    
                var uvs = new Vector2[numberOfVerts];
                    
                //initialize all as (1,0)
                for (int vert = 0; vert < numberOfVerts; vert++)
                {
                    uvs[vert] = new Vector2(1.0f, 0.0f);
                }
                    
                //Every highlighted vertex gets (0.48,0)
                foreach (var t in whichBeads[i])
                {
                    for (int side = 0; side < sides; side++)
                    {
                        uvs[t * sides + side] = new Vector2(0.48f, 0.0f);
                    }
                }
                    
                //set uvs to the link component game object
                _linkComponents[i].ComponentGameObject.GetComponent<MeshFilter>().mesh.uv = uvs;
            }
        }

        private void UnHighlightBeads(List<LinkComponent> linkComponents)
        {
            for (int i = 0; i < linkComponents.Count; i++)
            {
                var numberOfBeads = linkComponents[i].BeadList.Count;
                var numberOfVerts = numberOfBeads * sides;
                    
                var uvs = new Vector2[numberOfVerts];
                    
                //initialize all as (1,0)
                for (int vert = 0; vert < numberOfVerts; vert++)
                {
                    uvs[vert] = new Vector2(1.0f, 0.0f);
                }
                    
                //set uvs to the link component game object
                linkComponents[i].ComponentGameObject.GetComponent<MeshFilter>().mesh.uv = uvs;
            }
        }
        
    }
}