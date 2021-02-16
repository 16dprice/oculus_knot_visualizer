using UnityEngine;

public class DefaultFileAccessor
{
    private static string DefaultLocation = System.IO.Path.Combine("PointsFiles","Default");

    public static TextAsset GetTextAsset(int crossingNumber, int ordering, int numComponents)
    {
        string fileName;
        
        if (numComponents == 1)
        {
            fileName =
            System.IO.Path.Combine(
                $"{DefaultLocation}","Knots",$"{crossingNumber}_crossings",
                $"knot_{crossingNumber}_{ordering}"
            );
        }
        else
        {
            fileName =
            System.IO.Path.Combine(
                $"{DefaultLocation}","Links",$"{crossingNumber}_crossings",
                $"link_{crossingNumber}_{numComponents}_{ordering}"
            );
        }

        return Resources.Load(fileName) as TextAsset;
    }
}