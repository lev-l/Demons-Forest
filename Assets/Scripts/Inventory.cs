using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class Inventory : MonoBehaviour
{
    public KeyCode KeyForHealBottles,
                KeyForThrowingKnifes,
                KeyForStaticTorches;
    private InventoryContents _usableObjects;
    private PlayerHealth _playerHP;

    private void Start()
    {
        _usableObjects = Resources.Load<InventoryContents>("PlayerInventory");
        _playerHP = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyForHealBottles))
        {
            _usableObjects.UseHealBottle(_playerHP);
        }
    }
}
