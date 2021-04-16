using System.Collections.Generic;
using BeadsProviders;
using Domain;
using LinkRelaxing;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class DrawBeadSpawner : MonoBehaviour
    {
        private const float BEAD_CREATION_DISTANCE = 0.005f;

        private readonly float _radius = 0.2f;
        private readonly int _sides = 6;

        [SerializeField] private GameObject _beadPrefab;
        [SerializeField] private GameObject _linkGameObject;
        private readonly List<GameObject> _beadPrefabObjects = new List<GameObject>();

        private List<Bead> _component = new List<Bead>();
        private List<LinkComponent> _link = new List<LinkComponent>();

        private Vector3 _previousTouchPos = Vector3.zero;
        private Vector3 _currentTouchPos = Vector3.zero;
        
        private float H = 0.62f;
        private float K = 12.98f;

        private void Update()
        {
            var leftTrigger = OVRInput.Button.PrimaryIndexTrigger;
            var rightTrigger = OVRInput.Button.SecondaryIndexTrigger;
            var rightGrip = OVRInput.Button.SecondaryHandTrigger;
            var aButton = OVRInput.Button.One;
            var bButton = OVRInput.Button.Two;
            var leftStick = OVRInput.Axis2D.PrimaryThumbstick;
            var rightStick = OVRInput.Axis2D.SecondaryThumbstick;

            if (OVRInput.Get(leftTrigger))
            {
                _link = new List<LinkComponent>();
                MeshManipulation.DisplayLink(_linkGameObject.transform, new LinkStickModel(_link));
            }
            
            if (OVRInput.Get(rightTrigger)) AddBeads();

            if (!OVRInput.Get(rightTrigger) && OVRInput.GetDown(aButton))
            {
                CompleteComponent();
            }

            if (OVRInput.GetDown(bButton))
            {
                DestroyBeads();
                DestroyLink();
            }

            if (OVRInput.Get(rightGrip))
            {
                var alpha = 4f;
                var beta = 1f;

                K += 10 * OVRInput.Get(rightStick).x;
                if (K > 1000) K = 1000;
                if (K < 1) K = 1;

                H += 10 * OVRInput.Get(leftStick).x;
                if (H > 1000) H = 1000;
                if (H < 1) H = 1;
                
                var linkRelaxer = new LinkRelaxer(_link);
                _link = linkRelaxer.SimplifyLink(H, K, alpha, beta);
                
                MeshManipulation.DisplayLink(_linkGameObject.transform, new LinkStickModel(_link));
            }
        }

        private void AddBeads()
        {
            _currentTouchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            var d = _currentTouchPos - _previousTouchPos;
            
            if (d.x * d.x + d.y * d.y + d.z * d.z > BEAD_CREATION_DISTANCE)
            {
                var bead = Instantiate(_beadPrefab, _currentTouchPos, Quaternion.identity);
                
                _beadPrefabObjects.Add(bead);
                _component.Add(new Bead(_currentTouchPos));
                _previousTouchPos = _currentTouchPos;
            }
        }

        private void CompleteComponent()
        {
            _link.Add(new LinkComponent(_component));
            _component = new List<Bead>();

            DestroyBeads();

            MeshManipulation.DisplayLink(_linkGameObject.transform, new LinkStickModel(_link));

            _previousTouchPos = Vector3.zero;
            _currentTouchPos = Vector3.zero;
        }

        private void DestroyBeads()
        {
            foreach (var beadPrefabObject in _beadPrefabObjects) Destroy(beadPrefabObject);
            _beadPrefabObjects.Clear();
        }

        private void DestroyLink()
        {
            foreach (Transform child in transform) Destroy(child.gameObject);
            _link.Clear();
        }

        // private void Update()
        // {
        //     _currentTriggerState = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
        //     _currentTouchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        //
        //     if (_currentTriggerState)
        //     {
        //         if (!_previousTriggerState)
        //         {
        //             _drawingState = true;
        //         }
        //     }
        //     else
        //     {
        //         _drawingState = false;
        //         _previousTouchPos = Vector3.zero;
        //     
        //         if (!_previousTriggerState)
        //         {
        //             _link.Add(new LinkComponent(_component));
        //             _component.Clear();
        //         
        //             DestroyBeads();
        //             MeshManipulation.DisplayLink(transform, new LinkStickModel(new DrawBeadsProvider(_link)), _sides, _radius);
        //         }
        //     }
        //
        //     if (_drawingState)
        //     {
        //         var d = _currentTouchPos - _previousTouchPos;
        //         //Avoids square root calculation
        //         if (d.x*d.x + d.y*d.y + d.z*d.z > 0.1f)
        //         {
        //             var bead = Instantiate(beadPrefab, _currentTouchPos, Quaternion.identity);
        //             _beadPrefabObjects.Add(bead);
        //             _component.Add(new Bead(_currentTouchPos));
        //         
        //             _previousTouchPos = _currentTouchPos;
        //         }
        //     }
        //
        //     if (OVRInput.GetDown(OVRInput.Button.Two))
        //         DestroyLink();
        //
        //     _previousTriggerState = _currentTriggerState;
        // }
    }
}