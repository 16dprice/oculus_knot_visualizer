using System.IO;
using UnityEngine;

public class DefaultFileAccessor
{
    private static readonly string DefaultLocation = Path.Combine("PointsFiles", "Default");

    public static TextAsset GetTextAsset(int crossingNumber, int ordering, int numComponents)
    {
        string fileName;

        if (numComponents == 1)
            fileName =
                Path.Combine(
                    $"{DefaultLocation}", "Knots", $"{crossingNumber}_crossings",
                    $"knot_{crossingNumber}_{ordering}"
                );
        else
            fileName =
                Path.Combine(
                    $"{DefaultLocation}", "Links", $"{crossingNumber}_crossings",
                    $"link_{crossingNumber}_{numComponents}_{ordering}"
                );

        return Resources.Load(fileName) as TextAsset;
    }
}