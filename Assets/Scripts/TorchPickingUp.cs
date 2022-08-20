using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TorchPickingUp : MonoBehaviour
{
    public KeyCode KeyToPickup;
    private Collider2D _collider;
    private Inventory _playerInventory;
    private List<Collider2D> _overlapResults;
    private ContactFilter2D _filter;

    private void Start()
    {
        _collider = transform.GetChild(2).GetComponent<Collider2D>();
        _playerInventory = FindObjectOfType<Inventory>();
        _overlapResults = new List<Collider2D>();
        _filter = new ContactFilter2D();

        _filter.useTriggers = false;
        _filter.useLayerMask = true;
        _filter.SetLayerMask(Physics2D.GetLayerCollisionMask(_collider.gameObject.layer));

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyToPickup)
            && _collider.OverlapCollider(_filter, _overlapResults) > 0)
        {
            _playerInventory.AddObjects(new List<CollectableObject>() { new StaticTorchObject() });
            Destroy(gameObject);
        }
    }
}
