using System.Collections.Generic;
using BeadsProviders;
using Domain;
using UI;
using UnityEngine;

namespace StrandPassage
{
    public class StrandPassObject : MonoBehaviour
    {
        private float radius = 0.5f;
        private int sides = 6;

        private int NumComponents = 1;
        private int CrossingNumber = 3;
        private int Ordering = 1;

        [SerializeField] int FirstStrandComponent = 0;
        [SerializeField] int SecondStrandComponent = 0;
        [SerializeField] int FirstStrandSegment = 0;
        [SerializeField] int SecondStrandSegment = 0;

        private int _previousFirstStrandComponent = 0;
        private int _previousSecondStrandComponent = 0;
        private int _previousFirstStrandSegment = 0;
        private int _previousSecondStrandSegment = 0;

        private List<LinkComponent> _linkComponents;

        void Start()
        {
            var beadsProvider = new DefaultFileBeadsProvider(CrossingNumber, Ordering, NumComponents);
            _linkComponents = beadsProvider.GetLinkComponents();

            var linkStickModel = new LinkStickModel(_linkComponents);

            MeshManipulation.DisplayLink(transform, linkStickModel, sides, radius);
        }

        private void Update()
        {
            if (
                _previousFirstStrandComponent != FirstStrandComponent ||
                _previousSecondStrandComponent != SecondStrandComponent ||
                _previousFirstStrandSegment != FirstStrandSegment ||
                _previousSecondStrandSegment != SecondStrandSegment
            )
            {
                var strandPassProvider = new StrandPassProvider(
                    _linkComponents,
                    (FirstStrandComponent, FirstStrandSegment),
                    (SecondStrandComponent, SecondStrandSegment)
                );
                var linkStickModel = new LinkStickModel(strandPassProvider);

                MeshManipulation.DisplayLink(transform, linkStickModel, sides, radius);

                _previousFirstStrandComponent = FirstStrandComponent;
                _previousSecondStrandComponent = SecondStrandComponent;
                _previousFirstStrandSegment = FirstStrandSegment;
                _previousSecondStrandSegment = SecondStrandSegment;
            }
        }
    }
}