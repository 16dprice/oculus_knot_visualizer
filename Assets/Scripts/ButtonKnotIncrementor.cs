using UnityEngine;

public class ButtonKnotIncrementor : MonoBehaviour
{
    KnotGameObject _knotGameObject;
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
        _knotGameObject = GameObject.Find("KnotGameObject").GetComponent<KnotGameObject>();
    }

    void NextKnot()
    {
        _numComponents = _knotGameObject.NumComponents;
        _crossingNumber = _knotGameObject.CrossingNumber;
        _ordering = _knotGameObject.Ordering;
        
        for ( ; _numComponents < _numberOfLinks.Length; _numComponents++)
        {
            for ( ; _crossingNumber < _numberOfLinks[_numComponents].Length; _crossingNumber++)
            {
                if (_ordering < _numberOfLinks[_numComponents][_crossingNumber]) {
                    _ordering++;

                    _knotGameObject.NumComponents = _numComponents;
                    _knotGameObject.CrossingNumber = _crossingNumber;
                    _knotGameObject.Ordering = _ordering;

                    return;
                }
                
                _ordering = 0;
                
            }
            _crossingNumber = 0;
        }
        
        _knotGameObject.NumComponents = 1;
        _knotGameObject.CrossingNumber = 3;
        _knotGameObject.Ordering = 1;
    }
    
}