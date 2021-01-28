using System.Collections.Generic;
using UnityEngine;

public class DefaultFileBeadsProvider: ILinkBeadsProvider
{
        private readonly TextAsset _linkPointsTextFile;

        public DefaultFileBeadsProvider(int crossingNumber, int ordering, int numComponents = 1)
        {
                _linkPointsTextFile = DefaultFileAccessor.GetTextAsset(crossingNumber, ordering, numComponents);                
        }

        public List<Vector3[]> GetBeadsList()
        {
                var componentPoints = new List<Vector3>();
                var linkList = new List<Vector3[]>();

                var linkTextFileText = _linkPointsTextFile.text;
                var points = linkTextFileText.Split(System.Environment.NewLine.ToCharArray());
                
                foreach(var line in points)
                {
                        var point = ParseTextFileLineIntoVector(line);

                        if (point != null) componentPoints.Add(point.Value);

                        if (string.IsNullOrWhiteSpace(line))
                        {
                                linkList.Add(componentPoints.ToArray());
                                componentPoints = new List<Vector3>();
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