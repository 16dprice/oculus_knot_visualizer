using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonKnotIncrementor : MonoBehaviour
{
    KnotGameObject knotGameObject;
    int crossingNumber;
    int ordering;

    int[] numberOfKnots = {0, 0, 0, 1, 1, 2, 3, 7};

    void Start()
    {
        knotGameObject = GameObject.Find("KnotGameObject").GetComponent<KnotGameObject>();
    }

    void NextKnot()
    {
        crossingNumber = knotGameObject.crossingNumber;
        ordering = knotGameObject.ordering;

        if (ordering < numberOfKnots[crossingNumber]) ordering++;
        else {
            crossingNumber++;
            ordering = 1;
        }

        if (crossingNumber >= numberOfKnots.Length)  crossingNumber = 3;

        knotGameObject.crossingNumber = crossingNumber;
        knotGameObject.ordering = ordering;
    }

    
    // void Update()
    // {
    //     if (Input.GetButtonDown("Jump"))
    //     {
    //         NextKnot();
    //     }
    // }
}
