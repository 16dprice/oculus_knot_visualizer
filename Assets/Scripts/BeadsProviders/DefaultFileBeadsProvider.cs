using System;
using System.Collections.Generic;
using Domain;
using FileAccess;
using UnityEngine;

namespace BeadsProviders
{
    public class DefaultFileBeadsProvider : ILinkBeadsProvider
    {
        private readonly TextAsset _linkPointsTextFile;

        public DefaultFileBeadsProvider(int crossingNumber, int ordering, int numComponents = 1)
        {
            _linkPointsTextFile = DefaultFileAccessor.GetTextAsset(crossingNumber, ordering, numComponents);
        }

        public List<LinkComponent> GetLinkComponents()
        {
            var componentBeads = new List<Bead>();
            var linkList = new List<LinkComponent>();

            var linkTextFileText = _linkPointsTextFile.text;
            var points = linkTextFileText.Split(Environment.NewLine.ToCharArray());

            foreach (var line in points)
            {
                var point = ParseTextFileLineIntoVector(line);

                if (point != null) componentBeads.Add(new Bead(point.Value));

                if (string.IsNullOrWhiteSpace(line))
                {
                    linkList.Add(new LinkComponent(componentBeads));
                    componentBeads = new List<Bead>();
                }
            }

            return linkList;
        }

        private static Vector3? ParseTextFileLineIntoVector(string line)
        {
            var coordinates = line.Split(' ');

            if (coordinates.Length != 3) return null;

            var xCoord = float.Parse(coordinates[0]);
            var yCoord = float.Parse(coordinates[1]);
            var zCoord = float.Parse(coordinates[2]);

            return new Vector3(xCoord, yCoord, zCoord);
        }
    }
}