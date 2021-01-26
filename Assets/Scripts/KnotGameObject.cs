using System;
using UnityEngine;

public class KnotGameObject : MonoBehaviour
{
    [SerializeField] float radius = 0.5f;
    [SerializeField] [Range(3, 20)] int sides = 6;
    [SerializeField] private int crossingNumber = 3;
    [SerializeField] private int ordering = 1;
    [SerializeField] private int numComponents = 1;

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
            meshObject.transform.parent = transform;
            ResetTransform(meshObject);
        }
    }

    void ResetTransform(GameObject obj)
    {
        obj.transform.localPosition = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
    }
}