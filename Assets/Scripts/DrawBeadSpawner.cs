using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DrawBeadSpawner : MonoBehaviour
{
    private readonly float _radius = 0.02f;
    private readonly int _sides = 6;
    
    [SerializeField] private GameObject beadPrefab;
    private readonly List<GameObject> _beadPrefabObjects = new List<GameObject>();
    
    private readonly List<Vector3> _component = new List<Vector3>();
    private readonly List<Vector3[]> _link = new List<Vector3[]>();

    private bool _previousTriggerState = false;
    private bool _currentTriggerState = false;
    private bool _drawingState = false;
    
    private Vector3 _previousTouchPos = Vector3.zero;
    private Vector3 _currentTouchPos = Vector3.zero;
    
    [SerializeField] private Transform _indexFingerTransform;
    [SerializeField] private Transform _thumbFingerTransform;
    [SerializeField] private Transform _middleFingerTransform;

    private void Update()
    {
        _currentTouchPos = _indexFingerTransform.position;
        _currentTriggerState = (Vector3.Distance(_currentTouchPos,_thumbFingerTransform.position) < 0.02);
        
        //_currentTriggerState = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
        //_currentTouchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

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
                _link.Add(_component.ToArray());
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
                _component.Add(_currentTouchPos);
                
                _previousTouchPos = _currentTouchPos;
            }
        }

        if (Vector3.Distance(_middleFingerTransform.position, _thumbFingerTransform.position) < 0.02)
            DestroyLink();
        
        //if (OVRInput.GetDown(OVRInput.Button.Two))
        //    DestroyLink();

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
