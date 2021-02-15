using UnityEngine;

public class ButtonKnotIncrementor : MonoBehaviour
{
    KnotGameObject knotGameObject;
    int _numComponents;
    int _crossingNumber;
    int _ordering;

    int[][] _numberOfLinks =
    {
        new[] {0},
        new[] {0, 0, 0, 1, 1, 2, 3, 7},
        new[] {0, 0, 1, 0, 1, 1, 3},
        new[] {0, 0, 0, 0, 0, 0, 3},
    };

    void Start()
    {
        knotGameObject = GameObject.Find("KnotGameObject").GetComponent<KnotGameObject>();
    }

    void NextKnot()
    {
        _numComponents = knotGameObject.NumComponents;
        _crossingNumber = knotGameObject.CrossingNumber;
        _ordering = knotGameObject.Ordering;
        
        for ( ; _numComponents < _numberOfLinks.Length; _numComponents++)
        {
            for ( ; _crossingNumber < _numberOfLinks[_numComponents].Length; _crossingNumber++)
            {
                if (_ordering < _numberOfLinks[_numComponents][_crossingNumber]) {
                    _ordering++;

                    knotGameObject.NumComponents = _numComponents;
                    knotGameObject.CrossingNumber = _crossingNumber;
                    knotGameObject.Ordering = _ordering;

                    return;
                }
                
                _ordering = 0;
                
            }
            _crossingNumber = 0;
        }
        
        knotGameObject.NumComponents = 1;
        knotGameObject.CrossingNumber = 3;
        knotGameObject.Ordering = 1;
    }
    
}