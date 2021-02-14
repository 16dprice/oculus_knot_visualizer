//PR: unnecessary using System.Collections
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBeadSpawner : MonoBehaviour
{
    float radius = 0.02f;
    int sides = 6;
    
    [SerializeField] private GameObject _beadPrefab;
    private List<GameObject> _beadPrefabObjects = new List<GameObject>();
    
    private List<Vector3> component = new List<Vector3>();
    private List<Vector3[]> link = new List<Vector3[]>();

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
            
            if (!_previousTriggerState)
            {
                link.Add(component.ToArray());
                component.Clear();
                
                DestroyBeads();
                DisplayKnot();
            }
        }

        if (_drawingState)
        {
            var d = _currentTouchPos - _previousTouchPos;
            //Avoids square root calculation
            if (d.x*d.x + d.y*d.y + d.z*d.z > 0.001f)
            {
                var bead = Instantiate(_beadPrefab, _currentTouchPos, Quaternion.identity);
                _beadPrefabObjects.Add(bead);
                component.Add(_currentTouchPos);
                
                _previousTouchPos = _currentTouchPos;
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.Two))
            DestroyKnot();

        _previousTriggerState = _currentTriggerState;
    }
    
    void DisplayKnot()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
        
        var stickModel = new LinkStickModel(new DrawBeadsProvider(link));

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
        //PR: gameObject is also a global var that you could access in this script
        // maybe change this to var beadPrefabObject to make it clearer?
        foreach (var gameObject in _beadPrefabObjects)
        {
            Destroy(gameObject);
        }

        _beadPrefabObjects.Clear();
    }

    void DestroyKnot()
    {
        //PR: unnecessary "this"
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
        
        link.Clear();
    }
}
