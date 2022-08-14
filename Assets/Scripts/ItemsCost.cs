using System;
using System.Collections.Generic;

public class ItemsCost
{
    public Dictionary<Collectables, int> ItemCost;

    public ItemsCost()
    {
        ItemCost = new Dictionary<Collectables, int>();

        ItemCost.Add(Collectables.HealBottle, 5);
        ItemCost.Add(Collectables.ThrowingKnife, 1);
        ItemCost.Add(Collectables.StaticTorch, 3);
    }
}
