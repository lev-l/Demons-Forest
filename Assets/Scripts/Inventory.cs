using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class Inventory : MonoBehaviour
{
    public KeyCode KeyForHealBottles,
                KeyForThrowingKnifes,
                KeyForStaticTorches;
    private InventoryPresenter _presenter;
    private InventoryContents _usableObjects;
    private PlayerHealth _playerHP;

    private void Start()
    {
        _presenter = FindObjectOfType<InventoryPresenter>();
        _usableObjects = Resources.Load<InventoryContents>("PlayerInventory");
        _playerHP = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyForHealBottles))
        {
            if(_usableObjects.UseHealBottle(_playerHP))
                _presenter.RemoveHealBottle();
        }

        if (Input.GetKeyDown(KeyForThrowingKnifes))
        {
            if(_usableObjects.UseThrowingKnife())
                _presenter.RemoveThrowingKnife();
        }
    }

    public void AddObjects(List<CollectableObject> collectables)
    {
        foreach (CollectableObject collectable in collectables)
        {
            if (collectable is HealBottleObject)
            {
                _usableObjects.AddHealthBottle();
                _presenter.AddHealBottle(1);
            }
            else if (collectable is ThrowingKnifeObject)
            {
                _usableObjects.AddThrowingKnife();
                _presenter.AddThrowingKnife(1);
            }
            else if (collectable is StaticTorchObject)
            {
            }
        }
    }
}
