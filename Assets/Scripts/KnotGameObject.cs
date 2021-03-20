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
    
    public int NumComponents {get; set;} = 1;
    public int CrossingNumber {get; set;} = 8;
    public int Ordering {get; set;} = 22;

    private float _previousRadius = 0.5f;
    private int _previousSides = 6;

    private int _previousCrossingNumber = 3;
    private int _previousNumComponents = 1;
    private int _previousOrdering = 1;

    private LinkStickModel linkStickModel;
    private List<LinkComponent> linkComponents;
    
    void Start()
    {
        // var beadsProvider = new DefaultFileBeadsProvider(CrossingNumber, Ordering, NumComponents);
        // var linkStickModel = new LinkStickModel(beadsProvider);
        //
        // MeshManipulation.DisplayLink(transform, linkStickModel, sides, radius);
        var beadsProvider = new DefaultFileBeadsProvider(8, 22, 1);
        linkComponents = beadsProvider.GetLinkComponents();
        linkStickModel = new LinkStickModel(linkComponents);
        
        MeshManipulation.DisplayLink(transform, linkStickModel, sides, radius);
    }

    private void Update()
    {
        linkComponents = LinkRelaxer.SimplifyLink(linkComponents, 1, 0.01f, 1.5f, 3);
        linkStickModel = new LinkStickModel(linkComponents);
    
        MeshManipulation.DisplayLink(transform, linkStickModel, sides, radius);
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