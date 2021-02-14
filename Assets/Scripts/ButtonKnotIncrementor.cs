using UnityEngine;

public class ButtonKnotIncrementor : MonoBehaviour
{
    KnotGameObject knotGameObject;
    int _numComponents;
    int _crossingNumber;
    int _ordering;

    //PR: no need for int[] inside of this (i.e. it can just be new[] {0} instead of new int[] {0}
    int[][] _numberOfLinks =
    {
        new int[] {0},
        new int[] {0, 0, 0, 1, 1, 2, 3, 7},
        new int[] {0, 0, 1, 0, 1, 1, 3},
        new int[] {0, 0, 0, 0, 0, 0, 3},
    };

    void Start()
    {
        knotGameObject = GameObject.Find("KnotGameObject").GetComponent<KnotGameObject>();
    }

    void NextKnot()
    {
        _numComponents = knotGameObject.numComponents;
        _crossingNumber = knotGameObject.crossingNumber;
        _ordering = knotGameObject.ordering;
        
        for ( ; _numComponents < _numberOfLinks.Length; _numComponents++)
        {
            for ( ; _crossingNumber < _numberOfLinks[_numComponents].Length; _crossingNumber++)
            {
                if (_ordering < _numberOfLinks[_numComponents][_crossingNumber]) {
                    _ordering++;

                    knotGameObject.numComponents = _numComponents;
                    knotGameObject.crossingNumber = _crossingNumber;
                    knotGameObject.ordering = _ordering;

                    return;
                }
                //PR: unnecessary else clause. You can get rid of it entirely and just leave the _ordering = 0;
                else
                {
                    _ordering = 0;
                }
            }
            _crossingNumber = 0;
        }
        
        knotGameObject.numComponents = 1;
        knotGameObject.crossingNumber = 3;
        knotGameObject.ordering = 1;
    }
    
}