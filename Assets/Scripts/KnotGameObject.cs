using System;
using System.Collections.Generic;
using BeadsProviders;
using Domain;
using LinkRelaxing;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class KnotGameObject : MonoBehaviour
{
    [SerializeField] float radius = 0.2f;
    [SerializeField] int sides = 6;
    [SerializeField] bool minimize = false;
    [SerializeField] float H = 0.62f;
    [SerializeField] float K = 12.98f;
    [SerializeField] float alpha = 4;
    [SerializeField] float beta = 1;

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
        // var beadsProvider = new DefaultFileBeadsProvider(6, 3, 3);
        _linkComponents = beadsProvider.GetLinkComponents();
        _linkStickModel = new LinkStickModel(_linkComponents);
        _linkRelaxer = new LinkRelaxer(_linkComponents);

        _linkComponents = _linkRelaxer.SimplifyLink(H, K, alpha, beta);

        // DisplayAsSpheres(_linkComponents[0].BeadList);
        // foreach (var component in _linkComponents)
        // {
        //     DisplayAsBeadsAndCylinders(component.BeadList);
        // }
        MeshManipulation.DisplayLink(transform, _linkStickModel, sides, radius);
    }
    
    private void Update()
    {
        if (true)
        {
            _linkComponents = _linkRelaxer.SimplifyLink(H, K, alpha, beta);
            
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            
            // DisplayAsSpheres(_linkComponents[0].BeadList);
            // foreach (var component in _linkComponents)
            // {
            //     DisplayAsBeadsAndCylinders(component.BeadList);
            // }
            _linkStickModel = new LinkStickModel(_linkComponents);
            MeshManipulation.DisplayLink(transform, _linkStickModel, sides, radius);
        }
    }

    private void DisplayAsBeadsAndCylinders(List<Bead> beads)
    {
        for(int beadIndex = 0; beadIndex < beads.Count - 1; beadIndex++)
        {
            var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            cylinder.transform.position = beads[beadIndex].position;
            cylinder.transform.LookAt(beads[beadIndex + 1].position);
            cylinder.transform.parent = transform;
        }
    }

    private void DisplayAsSpheres(List<Bead> beads)
    {
        var length = beads.Count;
        for (int i = 0; i < length; i++)
        {
            var bead = beads[i];
            
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = bead.position;
            sphere.transform.parent = transform;
            
            sphere.GetComponent<Renderer>().material.SetColor("_Color", new Color((float) i / length, 0, 1, 1));
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