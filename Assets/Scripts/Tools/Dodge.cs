using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{
    [SerializeField] private float _dodgeStrength;
    private Transform _transform;
    private Collider2D _collider;
    private List<RaycastHit2D> _results;
    private ContactFilter2D _filter;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _collider = GetComponent<Collider2D>();
        _results = new List<RaycastHit2D>();
        _filter = new ContactFilter2D();

        _filter.useTriggers = false;
        _filter.useLayerMask = true;
        _filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    }

    public void DoDodge(Vector2 direction)
    {
        _collider.Cast(direction, _filter, _results, _dodgeStrength);

        foreach (RaycastHit2D hit in _results)
        {
            print(hit.distance);
        }
    }
}
