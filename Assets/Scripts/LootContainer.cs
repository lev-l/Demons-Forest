using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyLoot")]
public class LootContainer : ScriptableObject
{
    public CollectableObject[][] LootTypes;
    public int PossibilityOf1, PossibilityOf2, PossibilityOf3; //from most common to most rare
    [SerializeField] private Collectables[] _lootVariant1;
    [SerializeField] private Collectables[] _lootVariant2;
    [SerializeField] private Collectables[] _lootVariant3;

    private void OnEnable()
    {
        CollectableObject[] lootVariant1 = ConvertToCollectableObject(_lootVariant1);
        CollectableObject[] lootVariant2 = ConvertToCollectableObject(_lootVariant2);
        CollectableObject[] lootVariant3 = ConvertToCollectableObject(_lootVariant3);

        LootTypes = new CollectableObject[][]
        {
            lootVariant1,
            lootVariant2,
            lootVariant3
        };
    }

    private CollectableObject[] ConvertToCollectableObject(Collectables[] items)
    {
        CollectableObject[] result = new CollectableObject[items.Length];

        for(int i = 0; i < items.Length; i++)
        {
            switch (items[i])
            {
                case Collectables.HealBottle:
                    result[i] = new HealBottleObject();
                    break;
                case Collectables.ThrowingKnife:
                    result[i] = new ThrowingKnifeObject();
                    break;
                case Collectables.StaticTorch:
                    result[i] = new StaticTorchObject();
                    break;
                default:
                    Debug.LogError("Wrong type");
                    break;
            }
        }

        return result;
    }
}
