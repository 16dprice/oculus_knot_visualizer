using System;
using System.Collections.Generic;
using System.Linq;
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
        var beadsProvider = new DefaultFileBeadsProvider(CrossingNumber, Ordering);
        
        _linkComponents = beadsProvider.GetLinkComponents();
        _linkStickModel = new LinkStickModel(_linkComponents);
        _linkRelaxer = new LinkRelaxer(_linkComponents);
        
        // MeshManipulation.DisplayLink(transform, _linkStickModel, sides, radius);
        
        DrawLine(_linkComponents, Color.blue);
    }
    
    private void Update()
    {
        if (minimize)
        {
            _linkComponents = _linkRelaxer.SimplifyLink(H, K, alpha, beta);
            
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            DrawLine(_linkComponents, Color.blue);
            // _linkStickModel = new LinkStickModel(_linkComponents);
            // MeshManipulation.DisplayLink(transform, _linkStickModel, sides, radius);
        }
    }
    
    void DrawLine(List<LinkComponent> linkComponents, Color color)
    {
        var positionCount = linkComponents.Sum(linkComponent => linkComponent.BeadList.Count);

        var myLine = new GameObject();

        myLine.transform.parent = gameObject.transform;
        myLine.transform.position = Vector3.zero;
        myLine.AddComponent<LineRenderer>();
        
        var lineRenderer = myLine.GetComponent<LineRenderer>();
        lineRenderer.positionCount = positionCount;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.1f;
        
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        for (int i = 0; i < linkComponents.Count; i++)
        {
            for (int j = 0; j < linkComponents[i].BeadList.Count; j++)
            {
                lineRenderer.SetPosition(i + j, linkComponents[i].BeadList[j].position);
                
                var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                
                sphere.transform.parent = gameObject.transform;
                sphere.transform.position = linkComponents[i].BeadList[j].position;
                sphere.transform.localScale = 0.5f * Vector3.one;
                sphere.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
            }
        }

        lineRenderer.loop = true;
    }
}