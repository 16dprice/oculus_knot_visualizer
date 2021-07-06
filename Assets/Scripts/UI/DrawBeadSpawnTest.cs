using System.Collections.Generic;
using StrandPassage;
using BeadsProviders;
using Domain;
using UnityEngine;

namespace UI
{
    public class DrawBeadSpawnTest : MonoBehaviour
    {
        [SerializeField] private GameObject beadPrefab;
        
        private List<LinkComponent> _link;
        
        [SerializeField] private GameObject strandPassGameObject;
        private StrandPassObject _strandPassObject;

        private void Start()
        {
            _strandPassObject = strandPassGameObject.GetComponent<StrandPassObject>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                var beadsProvider = new DefaultFileBeadsProvider(3, 1, 1);
                _link = beadsProvider.GetLinkComponents();
                
                _strandPassObject.AddStrandPassObject(new LinkComponent(_link[0].BeadList));
            }
            
            if (Input.GetButtonDown("Fire1")) { _strandPassObject.DeleteStrandPassObject(); }
        }

    }
}
