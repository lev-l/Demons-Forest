using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class InventoryContents : ScriptableObject
{
    public List<HealBottleObject> HealBottles = new List<HealBottleObject>();
    public List<ThrowingKnifeObject> ThrowingKnifes = new List<ThrowingKnifeObject>();
    public List<StaticTorchObject> StaticTorches = new List<StaticTorchObject>();

    public void UseHealBottle(Health player)
    {
        //use one of heal bottles
        foreach(HealBottleObject healBottle in HealBottles)
        {
            healBottle.Heal(player);
            HealBottles.RemoveAt(0);
            break;
        }
    }

    public void AddHealthBottle()
    {
        HealBottles.Add(new HealBottleObject());
    }
}

public abstract class CollectableObject
{
}

public class HealBottleObject : CollectableObject
{
    private int _bottleHeal = 15;

    public void Heal(Health subject)
    {
        subject.Heal(_bottleHeal);
    }
}

public class ThrowingKnifeObject : CollectableObject
{
    public void Throw()
    {
    }
}

public class StaticTorchObject : CollectableObject
{
    public void SetUp()
    {
    }
}
