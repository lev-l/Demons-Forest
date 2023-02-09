using System;
using System.Collections.Generic;
using UnityEngine;

public class SpecificChest : Chest
{
    [SerializeField] private List<Collectables> _specificItems;

    protected override void Start()
    {
        base.Start();

        foreach(Collectables item in _specificItems)
        {
            AddNewCollectable(item);
        }
    }
}