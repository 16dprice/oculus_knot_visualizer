using System.Collections.Generic;

public class Component
{
    public readonly List<Bead> BeadList;

    public Component(List<Bead> beadList)
    {
        BeadList = beadList;
    }
}