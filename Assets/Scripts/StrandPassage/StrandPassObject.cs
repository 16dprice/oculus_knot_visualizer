using System.Collections.Generic;
using BeadsProviders;
using Domain;
using UI;
using UnityEngine;


namespace StrandPassage
{
    public class StrandPassObject : MonoBehaviour
    {
        private bool displayCrossings = true;
        private bool _previousDisplayCrossings = true;
        
        private float radius = 0.01f;
        private int sides = 6;

        private List<LinkComponent> _linkComponents = new List<LinkComponent>();
        private ILinkBeadsProvider _beadsProvider;

        private int _previousLinkComponentCount = 0;

        [SerializeField] private GameObject centerEyeAnchor;
        private Transform _centerEyeAnchorTransform;
        
        void Start()
        {
            _centerEyeAnchorTransform = centerEyeAnchor.transform;
        }

        public void AddStrandPassObject(LinkComponent linkComponent)
        {
            _previousLinkComponentCount = _linkComponents.Count;
            _linkComponents.Add(linkComponent);
        }

        public void DeleteStrandPassObject()
        {
            _previousLinkComponentCount = _linkComponents.Count;
            _linkComponents.Clear();
        }

        private void Update()
        {
            if (_linkComponents.Count != _previousLinkComponentCount)
            {
                if (_linkComponents.Count > 0)
                {
                    _beadsProvider = new DrawBeadsProvider(_linkComponents);
                    var linkStickModel = new LinkStickModel(_beadsProvider);
                    
                    MeshManipulation.DisplayLink(transform, linkStickModel, sides, radius);

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
                else
                {
                    MeshManipulation.DeleteChildren(transform);
                }
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