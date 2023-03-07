using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public KeyCode KeyToOpen;
    public int MaxCost;
    public string Hesh;
    public bool Saveable;
    protected List<CollectableObject> _collectables;
    protected Inventory _playerInventory;
    protected Collider2D _trigger;
    protected ContactFilter2D _filter;
    protected ChestAnimations _animations;
    protected ChestContentsPresenter _contentsPresenter;
    protected ChestsStateSaver _saver;

    protected Collider2D[] _playerCollider = new Collider2D[2];

    protected virtual void Awake()
    {
        _collectables = new List<CollectableObject>();
        _filter = new ContactFilter2D();
        _animations = GetComponent<ChestAnimations>();
        _contentsPresenter = FindObjectOfType<ChestContentsPresenter>();
        _saver = FindObjectOfType<ChestsStateSaver>();
        _playerInventory = FindObjectOfType<Inventory>();
        _trigger = GetComponent<Collider2D>();

        _filter.useTriggers = false;
        _filter.useLayerMask = true;
        _filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));

        if (!_trigger.isTrigger)
        {
            throw new System.Exception("Got the wrong collider");
        }

        RandomlyFillChest();
    }

    protected void RandomlyFillChest()
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
            if (costs.ItemCost[Collectables.StaticTorch] <= availableCost
                && _playerInventory.GetContent()[Collectables.StaticTorch] < 5)
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

    protected void AddNewCollectable(Collectables type)
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

    protected void Update()
    {
        if (Input.GetKeyDown(KeyToOpen)
            && _trigger.OverlapCollider(_filter, _playerCollider) > 0
            && _collectables.Count > 0)
        {
            Give();
        }
    }

    protected void Give()
    {
        _playerInventory.AddObjects(_collectables);
        _contentsPresenter.ShowItems(_collectables);
        if (Saveable)
        {
            _saver.ChestsStates.Add(Hesh, true);
        }
        _collectables.Clear();

        _animations.AnimationOpen();
    }

    public void Empty()
    {
        _collectables.Clear();
        _animations.AnimationOpen();
    }
}
