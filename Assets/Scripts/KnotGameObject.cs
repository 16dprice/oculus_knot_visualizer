using System;
using UnityEngine;

public class KnotGameObject : MonoBehaviour
{
    [SerializeField] float radius {get; set;} = 0.5f;
    [SerializeField] int sides {get; set;} = 6;
    public int crossingNumber {get; set;} = 3;
    public int ordering {get; set;} = 1;
    [SerializeField] int numComponents {get; set;} = 1;

    private float _previousRadius = 0.5f;
    private int _previousSides = 6;
    
    private int _previousCrossingNumber = 3;
    private int _previousOrdering = 1;
    private int _previousNumComponents = 1;



    void Start()
    {
        DisplayLink();
    }

    void Update()
    {
        if (
            Math.Abs(_previousRadius - radius) > 0.1 ||
            _previousSides != sides ||
            _previousCrossingNumber != crossingNumber ||
            _previousOrdering != ordering ||
            _previousNumComponents != numComponents
        )
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            DisplayLink();

            _previousRadius = radius;
            _previousSides = sides;
            _previousCrossingNumber = crossingNumber;
            _previousOrdering = ordering;
            _previousNumComponents = numComponents;
        }
    }

    void DisplayLink()
    {
        var beadsProvider = new DefaultFileBeadsProvider(crossingNumber, ordering, numComponents);
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
}