using System;
using UnityEngine;

public class PDCodeGenerator
{
    public static void Main()
    {
        var generator = new PDCodeGenerator(new DefaultFileBeadsProvider(6, 1, 2));
        generator.PrintInfo();
    }

    private ILinkBeadsProvider _beadsProvider;

    public PDCodeGenerator(ILinkBeadsProvider provider)
    {
        _beadsProvider = provider;
    }

    public void PrintInfo()
    {
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
                var beadPair1 = new BeadPair(new Bead(firstComponent[i]), new Bead(firstComponent[i + 1]));
                var beadPair2 = new BeadPair(new Bead(firstComponent[j]), new Bead(firstComponent[j + 1]));
                
                if (beadPair1.DoesIntersectOtherBeadPair(beadPair2)) numCrossings++;
            }
        }

        return numCrossings;
    }
}