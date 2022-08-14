using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Collectables
{
    HealBottle = 0,
    ThrowingKnife = 1,
    StaticTorch = 2
}

[CreateAssetMenu()]
public class InventoryContents : ScriptableObject
{
    private List<HealBottleObject> HealBottles = new List<HealBottleObject>();
    private List<ThrowingKnifeObject> ThrowingKnifes = new List<ThrowingKnifeObject>();
    private List<StaticTorchObject> StaticTorches = new List<StaticTorchObject>();

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

    public void AddThrowingKnife()
    {
        ThrowingKnifes.Add(new ThrowingKnifeObject());
    }
}

public abstract class CollectableObject
{
    public virtual Collectables GetItemType()
    {
        throw new System.Exception("Base function GetType has been called");
    }
}

public class HealBottleObject : CollectableObject
{
    private int _bottleHeal = 15;

    public void Heal(Health subject)
    {
        subject.Heal(_bottleHeal);
    }

    public override Collectables GetItemType()
    {
        return Collectables.HealBottle;
    }
}

public class ThrowingKnifeObject : CollectableObject
{
    public void Throw()
    {
    }

    public override Collectables GetItemType()
    {
        return Collectables.ThrowingKnife;
    }
}

public class StaticTorchObject : CollectableObject
{
    public void SetUp()
    {
    }

    public override Collectables GetItemType()
    {
        return Collectables.StaticTorch;
    }
}
