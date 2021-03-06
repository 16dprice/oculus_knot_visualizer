using System;
using BeadsProviders;
using UI;
using UnityEngine;

public class KnotGameObject : MonoBehaviour
{
    [SerializeField] float radius = 0.5f;
    [SerializeField] int sides = 6;
    
    public int NumComponents {get; set;} = 1;
    public int CrossingNumber {get; set;} = 3;
    public int Ordering {get; set;} = 1;

    private float _previousRadius = 0.5f;
    private int _previousSides = 6;
    
    private int _previousCrossingNumber = 3;
    private int _previousNumComponents = 1;
    private int _previousOrdering = 1;


    void Start()
    {
        var beadsProvider = new DefaultFileBeadsProvider(CrossingNumber, Ordering, NumComponents);
        var linkStickModel = new LinkStickModel(beadsProvider);
        
        MeshManipulation.DisplayLink(transform, linkStickModel, sides, radius);
    }

    private void Update()
    {
        if (
            Math.Abs(_previousRadius - radius) > 0.1 ||
            _previousSides != sides ||
            _previousCrossingNumber != CrossingNumber ||
            _previousOrdering != Ordering ||
            _previousNumComponents != NumComponents
        )
        {
            var beadsProvider = new DefaultFileBeadsProvider(CrossingNumber, Ordering, NumComponents);
            var linkStickModel = new LinkStickModel(beadsProvider);
            
            MeshManipulation.DisplayLink(transform, linkStickModel, sides, radius);

            _previousRadius = radius;
            _previousSides = sides;
            _previousCrossingNumber = CrossingNumber;
            _previousOrdering = Ordering;
            _previousNumComponents = NumComponents;
        }
    }
}