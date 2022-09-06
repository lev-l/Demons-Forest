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
    private Transform _playerTransform;

    private void Start()
    {
        _presenter = FindObjectOfType<InventoryPresenter>();
        _usableObjects = Resources.Load<InventoryContents>("PlayerInventory");
        _playerHP = GetComponent<PlayerHealth>();
        _playerTransform = _playerHP.GetComponent<Transform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyForHealBottles)
            && _usableObjects.UseHealBottle(_playerHP))
        {
            _presenter.RemoveHealBottle();
        }

        if (Input.GetKeyDown(KeyForThrowingKnifes)
            && _usableObjects.UseThrowingKnife(_playerTransform))
        {
            _presenter.RemoveThrowingKnife();
        }

        if (Input.GetKeyDown(KeyForStaticTorches)
            && _usableObjects.UseStaticTorch(_playerTransform))
        {
            _presenter.RemoveStaticTorch();
        }
    }

    public void AddObjects(List<CollectableObject> collectables)
    {
        foreach (CollectableObject collectable in collectables)
        {
            if (collectable is HealBottleObject)
            {
                _usableObjects.AddHealthBottle();
                _presenter.AddHealBottles(1);
            }
            else if (collectable is ThrowingKnifeObject)
            {
                _usableObjects.AddThrowingKnife();
                _presenter.AddThrowingKnifes(1);
            }
            else if (collectable is StaticTorchObject)
            {
                _usableObjects.AddStaticTorch();
                _presenter.AddStaticTorches(1);
            }
        }
    }
}
