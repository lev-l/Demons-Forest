using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class PlayerDataSaver
{
    public Vector2 PlayerPosition;
    public float Health;
    public HealBottleObject[] HealBottles;
    public ThrowingKnifeObject[] ThrowingKnives;
    public StaticTorchObject[] staticTorches;

    public PlayerDataSaver(Vector2 playerPosition, float health,
                            HealBottleObject[] healBottles, ThrowingKnifeObject[] throwingKnives,
                            StaticTorchObject[] staticTorches)
    {

    }

    public void Save()
    {

    }

    public void Load()
    {

    }
}
