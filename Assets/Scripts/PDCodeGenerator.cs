using System;
using UnityEngine;

public class PDCodeGenerator
{
    public static void Main()
    {
        var generator = new PDCodeGenerator(new DefaultFileBeadsProvider(4, 1));
        generator.PrintInfo();
    }

    private ILinkBeadsProvider _beadsProvider;

    public PDCodeGenerator(ILinkBeadsProvider provider)
    {
        _beadsProvider = provider;
    }

    public void PrintInfo()
    {
        Debug.Log(DoSegmentsIntersect(
            new Vector2(),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0)
        ));
        
        Debug.Log(GetNumCrossings());
    }

    private int GetNumCrossings()
    {
        var numCrossings = 0;

        var beadsList = _beadsProvider.GetBeadsList();

        var firstComponent = beadsList[0];
        
        for (int i = 0; i < firstComponent.Length - 5; i++)
        {
            for (int j = i + 3; j < firstComponent.Length - 2; j++)
            {
                var p1 = ProjectPoint(firstComponent[i]);
                var p3 = ProjectPoint(firstComponent[i + 1]);
                
                var p2 = ProjectPoint(firstComponent[j]);
                var p4 = ProjectPoint(firstComponent[j + 1]);

                if (DoSegmentsIntersect(p1, p2, p3, p4)) numCrossings++;
            }
        }

        return numCrossings;
    }

    private Vector2 ProjectPoint(Vector3 point)
    {
        return new Vector2(point.x, point.y);
    }

    private bool DoSegmentsIntersect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        var x1 = p1.x;
        var y1 = p1.y;
        var x2 = p2.x;
        var y2 = p2.y;
        var x3 = p3.x;
        var y3 = p3.y;
        var x4 = p4.x;
        var y4 = p4.y;

        var slopeOfSegmentA = (y3 - y1) / (x3 - x1);
        var slopeOfSegmentB = (y4 - y2) / (x4 - x2);

        var intersectionXValue = (y2 - slopeOfSegmentB * x2 - y1 + slopeOfSegmentA * x1) /
                                 (slopeOfSegmentA - slopeOfSegmentB);

        var segmentAParameterizationValue = (intersectionXValue - x1) / (x3 - x1);
        var segmentBParameterizationValue = (intersectionXValue - x2) / (x4 - x2);
        
        var isOnSegmentA = false;
        var isOnSegmentB = false;

        if (0 < segmentAParameterizationValue && segmentAParameterizationValue < 1) isOnSegmentA = true;
        if (0 < segmentBParameterizationValue && segmentBParameterizationValue < 1) isOnSegmentB = true;

        return isOnSegmentA && isOnSegmentB;
    }
}