using System;
using UnityEngine;

public class KnotGameObject : MonoBehaviour
{
    [SerializeField] float radius = 0.5f;
    [SerializeField] int sides = 6;
    
    public int NumComponents {get; set;} = 1;
    public int CrossingNumber {get; set;} = 3;
    public int Ordering {get; set;} = 1;

    private int _previousCrossingNumber = 3;
    private int _previousNumComponents = 1;
    private int _previousOrdering = 1;


    void Start()
    {
        MeshManipulation.DisplayLink(transform, new LinkStickModel(new DefaultFileBeadsProvider(CrossingNumber, Ordering, NumComponents)), sides, radius);
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
            MeshManipulation.DisplayLink(transform, new LinkStickModel(new DefaultFileBeadsProvider(CrossingNumber, Ordering, NumComponents)), sides, radius);

            _previousRadius = radius;
            _previousSides = sides;
            _previousCrossingNumber = CrossingNumber;
            _previousOrdering = Ordering;
            _previousNumComponents = NumComponents;
        }
    }
}