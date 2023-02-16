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
    [SerializeField] private List<HealBottleObject> HealBottles = new List<HealBottleObject>();
    [SerializeField] private List<ThrowingKnifeObject> ThrowingKnifes = new List<ThrowingKnifeObject>();
    [SerializeField] private List<StaticTorchObject> StaticTorches = new List<StaticTorchObject>();

    public Dictionary<Collectables, int> GetInventoryContent()
    {
        return new Dictionary<Collectables, int>() { { Collectables.HealBottle, HealBottles.Count },
                                                    { Collectables.ThrowingKnife, ThrowingKnifes.Count },
                                                    { Collectables.StaticTorch, StaticTorches.Count } };
    }

    public bool UseHealBottle(Health player)
    {
        //use one of heal bottles
        (int current, int max) health = player.GetHealthParams();

        if (health.current < health.max)
            foreach (HealBottleObject healBottle in HealBottles)
            {
                healBottle.Heal(player);
                HealBottles.RemoveAt(0);
                return true;
            }

        return false;
    }

    public bool UseThrowingKnife(Transform player)
    {
        //use one of throwing knifes
        foreach (ThrowingKnifeObject knife in ThrowingKnifes)
        {
            knife.Throw(player);
            ThrowingKnifes.RemoveAt(0);
            return true;
        }

        return false;
    }

    public bool UseStaticTorch(Transform player)
    {
        //use one of the static torches
        foreach (StaticTorchObject staticTorch in StaticTorches)
        {
            staticTorch.SetUp(player);
            StaticTorches.RemoveAt(0);
            return true;
        }
        return false;
    }

    public void AddHealthBottle()
    {
        HealBottles.Add(new HealBottleObject());
    }

    public void AddThrowingKnife()
    {
        ThrowingKnifes.Add(new ThrowingKnifeObject());
    }

    public void AddStaticTorch()
    {
        StaticTorches.Add(new StaticTorchObject());
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
    private GameObject _knife;

    public ThrowingKnifeObject()
    {
        _knife = Resources.Load<GameObject>("Knife");
    }

    public void Throw(Transform player)
    {
        GameObject newKnife = Object.Instantiate(_knife, player);
        newKnife.transform.SetParent(null);
    }

    public override Collectables GetItemType()
    {
        return Collectables.ThrowingKnife;
    }
}

public class StaticTorchObject : CollectableObject
{
    private GameObject _torch;

    public StaticTorchObject()
    {
        _torch = Resources.Load<GameObject>("Torch");
    }

    public void SetUp(Transform player)
    {
        GameObject newTorch = Object.Instantiate(_torch, player);

        Transform newTorchTransform = newTorch.transform;
        newTorchTransform.localPosition += Vector3.up;
        newTorchTransform.SetParent(null);
    }

    public override Collectables GetItemType()
    {
        return Collectables.StaticTorch;
    }
}
