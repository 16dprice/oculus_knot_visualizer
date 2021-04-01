using System;
using System.Collections.Generic;
using BeadsProviders;
using Domain;
using LinkRelaxing;
using UI;
using UnityEngine;

public class KnotGameObject : MonoBehaviour
{
    [SerializeField] float radius = 0.5f;
    [SerializeField] int sides = 6;
    [SerializeField] bool minimize = false;
    [SerializeField] float H = 0.9f;
    [SerializeField] float K = 0.01f;

    public int NumComponents {get; set;} = 1;
    public int CrossingNumber {get; set;} = 8;
    public int Ordering {get; set;} = 22;

    private float _previousRadius = 0.5f;
    private int _previousSides = 6;

    private int _previousCrossingNumber = 3;
    private int _previousNumComponents = 1;
    private int _previousOrdering = 1;

    private LinkStickModel _linkStickModel;
    private List<LinkComponent> _linkComponents;
    private LinkRelaxer _linkRelaxer;

    void Start()
    {
        // var beadsProvider = new DefaultFileBeadsProvider(CrossingNumber, Ordering, NumComponents);
        // var linkStickModel = new LinkStickModel(beadsProvider);
        //
        // MeshManipulation.DisplayLink(transform, linkStickModel, sides, radius);
        var beadsProvider = new DefaultFileBeadsProvider(8, 22, 1);
        _linkComponents = beadsProvider.GetLinkComponents();
        // _linkStickModel = new LinkStickModel(linkComponents);
        _linkRelaxer = new LinkRelaxer(_linkComponents);
        
        MeshManipulation.DisplayLink(transform, _linkStickModel, sides, radius);
    }

    private void Update()
    {
        if (minimize)
        {
            _linkComponents = _linkRelaxer.SimplifyLink(H, K, 4, 1);
            _linkStickModel = new LinkStickModel(_linkComponents);

            MeshManipulation.DisplayLink(transform, _linkStickModel, sides, radius);
        }
    }

    // private void Update()
    // {
    //     if (
    //         Math.Abs(_previousRadius - radius) > 0.1 ||
    //         _previousSides != sides ||
    //         _previousCrossingNumber != CrossingNumber ||
    //         _previousOrdering != Ordering ||
    //         _previousNumComponents != NumComponents
    //     )
    //     {
    //         var beadsProvider = new DefaultFileBeadsProvider(CrossingNumber, Ordering, NumComponents);
    //         var linkStickModel = new LinkStickModel(beadsProvider);
    //         
    //         MeshManipulation.DisplayLink(transform, linkStickModel, sides, radius);
    //
    //         _previousRadius = radius;
    //         _previousSides = sides;
    //         _previousCrossingNumber = CrossingNumber;
    //         _previousOrdering = Ordering;
    //         _previousNumComponents = NumComponents;
    //     }
    // }
}