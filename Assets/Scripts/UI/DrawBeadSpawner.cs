using System.Collections.Generic;
using BeadsProviders;
using Domain;
using UnityEngine;

namespace UI
{
    public class DrawBeadSpawner : MonoBehaviour
    {
        private readonly float _radius = 0.02f;
        private readonly int _sides = 6;
    
        [SerializeField] private GameObject beadPrefab;
        private readonly List<GameObject> _beadPrefabObjects = new List<GameObject>();
    
        private readonly List<Bead> _component = new List<Bead>();
        private readonly List<LinkComponent> _link = new List<LinkComponent>();

        private bool _previousTriggerState = false;
        private bool _currentTriggerState = false;
        private bool _drawingState = false;
    
        private Vector3 _previousTouchPos = Vector3.zero;
        private Vector3 _currentTouchPos = Vector3.zero;

        private void Update()
        {
            _currentTriggerState = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
            _currentTouchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

            if (_currentTriggerState)
            {
                if (!_previousTriggerState)
                {
                    _drawingState = true;
                }
            }
            else
            {
                _drawingState = false;
                _previousTouchPos = Vector3.zero;
            
                if (!_previousTriggerState)
                {
                    _link.Add(new LinkComponent(_component));
                    _component.Clear();
                
                    DestroyBeads();
                    MeshManipulation.DisplayLink(transform, new LinkStickModel(new DrawBeadsProvider(_link)), _sides, _radius);
                }
            }

            if (_drawingState)
            {
                var d = _currentTouchPos - _previousTouchPos;
                //Avoids square root calculation
                if (d.x*d.x + d.y*d.y + d.z*d.z > 0.001f)
                {
                    var bead = Instantiate(beadPrefab, _currentTouchPos, Quaternion.identity);
                    _beadPrefabObjects.Add(bead);
                    _component.Add(new Bead(_currentTouchPos));
                
                    _previousTouchPos = _currentTouchPos;
                }
            }

            if (OVRInput.GetDown(OVRInput.Button.Two))
                DestroyLink();

            _previousTriggerState = _currentTriggerState;
        }

        private void DestroyBeads()
        {
            foreach (var beadPrefabObject in _beadPrefabObjects)
            {
                Destroy(beadPrefabObject);
            }

            _beadPrefabObjects.Clear();
        }

        private void DestroyLink()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        
            _link.Clear();
        }
    }
}
