using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBeadSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _beadPrefab;
    private List<GameObject> _beadPrefabObjects = new List<GameObject>();

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            var bead = Instantiate(_beadPrefab, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), Quaternion.identity);
            _beadPrefabObjects.Add(bead);
        }

        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            foreach (var gameObject in _beadPrefabObjects)
            {
                Destroy(gameObject);
            }

            _beadPrefabObjects.Clear();
        }
    }
}
