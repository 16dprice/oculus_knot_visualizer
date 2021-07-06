using System;
using System.Collections.Generic;
using BeadsProviders;
using StrandPassage;
using Domain;
using UnityEngine;

namespace UI
{
    public class DrawBeadSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject beadPrefab;
        private readonly List<GameObject> _beadPrefabObjects = new List<GameObject>();
    
        private List<Bead> _component = new List<Bead>();

        private bool _previousTriggerState = false;
        private bool _currentTriggerState;
        private bool _drawingState = false;
    
        private Vector3 _previousTouchPos = Vector3.zero;
        private Vector3 _currentTouchPos = Vector3.zero;

        [SerializeField] private GameObject strandPassGameObject;
        private StrandPassObject _strandPassObject;

        private void Start()
        {
            _strandPassObject = strandPassGameObject.GetComponent<StrandPassObject>();
        }

        private void Update()
        {
            _currentTriggerState = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
            _currentTouchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

            if (_currentTriggerState)
            {
                _drawingState = true;
                
                if (!_previousTriggerState) _component.Clear();
            }
            else
            {
                _drawingState = false;
                _previousTouchPos = Vector3.zero;
            
                if (_previousTriggerState)
                {
                    //passing reference I think. coding around this right now
                    _strandPassObject.AddStrandPassObject(new LinkComponent(_component));
                
                    DestroyBeads();
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
                _strandPassObject.DeleteStrandPassObject();

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
    }
}
