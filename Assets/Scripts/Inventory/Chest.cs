using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public KeyCode KeyToOpen;
    public int MaxCost;
    private List<CollectableObject> _collectables;
    private Inventory _playerInventory;
    private Collider2D _trigger;
    private ContactFilter2D _filter;
    private ChestAnimations _animations;
    private ChestContentsPresenter _contentsPresenter;

    private Collider2D[] _playerCollider = new Collider2D[2];

    void Start()
    {
        _collectables = new List<CollectableObject>();
        RandomlyFillChest();

        _filter = new ContactFilter2D();
        _animations = GetComponent<ChestAnimations>();
        _contentsPresenter = FindObjectOfType<ChestContentsPresenter>();
        _playerInventory = FindObjectOfType<Inventory>();
        _trigger = GetComponent<Collider2D>();

        _filter.useTriggers = false;
        _filter.useLayerMask = true;
        _filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));

        if (!_trigger.isTrigger)
        {
            throw new System.Exception("Got the wrong collider");
        }
    }

    private void RandomlyFillChest()
    {
        ItemsCost costs = new ItemsCost();
        int currentCost = 0;

        while (currentCost < MaxCost)
        {
            int availableCost = MaxCost - currentCost;
            List<Collectables> availableCollectables = new List<Collectables>();

            if (costs.ItemCost[Collectables.HealBottle] <= availableCost)
            {
                availableCollectables.Add(Collectables.HealBottle);
            }
            if (costs.ItemCost[Collectables.ThrowingKnife] <= availableCost)
            {
                availableCollectables.Add(Collectables.ThrowingKnife);
            }
            if (costs.ItemCost[Collectables.StaticTorch] <= availableCost)
            {
                availableCollectables.Add(Collectables.StaticTorch);
            }

            Collectables randomlyPickedCollectable =
                availableCollectables[Random.Range(0, availableCollectables.Count)];
            currentCost += costs.ItemCost[randomlyPickedCollectable];
            AddNewCollectable(randomlyPickedCollectable);

            if (currentCost > MaxCost)
            {
                throw new System.Exception("Cost is overrising");
            }
        }
    }

    private void AddNewCollectable(Collectables type)
    {
        switch (type)
        {
            case Collectables.HealBottle:
                _collectables.Add(new HealBottleObject());
                break;
            case Collectables.ThrowingKnife:
                _collectables.Add(new ThrowingKnifeObject());
                break;
            case Collectables.StaticTorch:
                _collectables.Add(new StaticTorchObject());
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyToOpen)
            && _trigger.OverlapCollider(_filter, _playerCollider) > 0)
        {
            Give();
        }
    }

    private void Give()
    {
        _playerInventory.AddObjects(_collectables);
        _contentsPresenter.ShowItems(_collectables);
        _collectables.Clear();

        _animations.AnimationOpen();
    }
}
