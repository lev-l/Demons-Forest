using UnityEngine;

[CreateAssetMenu(fileName = "EnemyLoot")]
public class LootGiver : ScriptableObject
{
    public Collectables[] LootTypes;
}
