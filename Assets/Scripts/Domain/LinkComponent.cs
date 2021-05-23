using System.Collections.Generic;
using UnityEngine;

namespace Domain
{
    public class LinkComponent
    {
        public readonly List<Bead> BeadList;

        public GameObject ComponentGameObject { get; set; }

        public LinkComponent(List<Bead> beadList)
        {
            BeadList = beadList;
        }
    }
}