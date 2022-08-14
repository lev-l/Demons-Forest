using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public KeyCode KeyToOpen;
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
        _collectables.Add(new HealBottleObject());
        _collectables.Add(new HealBottleObject());
        _collectables.Add(new ThrowingKnifeObject());

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
