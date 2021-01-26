using UnityEngine;

public class DefaultFileAccessor
{
    private const string DefaultLocation = "PointsFiles/Default";

    public static TextAsset GetTextAsset(int crossingNumber, int ordering, int numComponents = 1)
    {
        string fileName;
        
        if (numComponents == 1)
        {
            fileName = $"{DefaultLocation}/Knots/{crossingNumber}_crossings/knot_{crossingNumber}_{ordering}";            
        }
        else
        {
            fileName =
                $"{DefaultLocation}/Links/{crossingNumber}_crossings/link_{crossingNumber}_{numComponents}_{ordering}";
        }

        return Resources.Load(fileName) as TextAsset;
    }
}