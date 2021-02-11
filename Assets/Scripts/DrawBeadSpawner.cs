using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBeadSpawner : MonoBehaviour
{
    float radius = 0.01f;
    int sides = 6;
    
    [SerializeField] private GameObject _beadPrefab;
    private List<GameObject> _beadPrefabObjects = new List<GameObject>();
    private List<Vector3> knot = new List<Vector3>();

    private bool _previousTriggerState = false;
    private bool _currentTriggerState = false;
    private bool _drawingState = false;
    
    private Vector3 _previousTouchPos = Vector3.zero;
    private Vector3 _currentTouchPos = Vector3.zero;

    void Update()
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
        }

        if (_drawingState)
        {
            var d = _currentTouchPos - _previousTouchPos;
            //Avoids square root calculation
            if (d.x*d.x + d.y*d.y + d.z*d.z > 0.001f)
            {
                var bead = Instantiate(_beadPrefab, _currentTouchPos, Quaternion.identity);
                _beadPrefabObjects.Add(bead);
                knot.Add(_currentTouchPos);
                
                _previousTouchPos = _currentTouchPos;
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.Two))
            DestroyBeads();

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            DestroyKnot();
            DisplayKnot();
            
            knot.Clear();
        }

        _previousTriggerState = _currentTriggerState;
    }
    
    void DisplayKnot()
    {
        //TODO: check if this is necessary
        if (knot.Count < 3) return;
        
        var beadsProvider = new DrawBeadsProvider(knot);
        var stickModel = new LinkStickModel(beadsProvider);

        var knotMeshObjects = stickModel.GetKnotMeshObjects(sides, radius);

        foreach (var meshObject in knotMeshObjects)
        {
            if (meshObject != null)
            {
                meshObject.transform.parent = transform;
                ResetTransform(meshObject);
            }
        }
    }

    void ResetTransform(GameObject obj)
    {
        obj.transform.localPosition = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
    }

    void DestroyBeads()
    {
        foreach (var gameObject in _beadPrefabObjects)
        {
            Destroy(gameObject);
        }

        _beadPrefabObjects.Clear();
    }

    void DestroyKnot()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
