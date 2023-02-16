using System.Collections.Generic;
using UnityEngine;

public class PlayerDataSaver
{
    public Vector2 PlayerPosition;
    public int Health;
    public Dictionary<Collectables, int> Inventory;

    public PlayerDataSaver(Vector2 playerPosition, int health,
                            Dictionary<Collectables, int> inventory)
    {
        PlayerPosition = playerPosition;
        Health = health;
        Inventory = inventory;
    }
}
