using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{
    [SerializeField] private float _strength;
    [SerializeField, Range(0.1f, 1)] private float _speed;
    private Transform _transform;
    private Movement _movement;
    private Collider2D _collider;
    private List<RaycastHit2D> _results;
    private ContactFilter2D _filter;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _movement = GetComponent<Movement>();
        _collider = GetComponent<Collider2D>();
        _results = new List<RaycastHit2D>();
        _filter = new ContactFilter2D();

        _filter.useTriggers = false;
        _filter.useLayerMask = true;
        _filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    }

    public void DoDodge(Vector3 direction)
    {
        _collider.Cast(direction, _filter, _results, _strength);

        float minDistance = _strength;
        foreach (RaycastHit2D hit in _results)
        {
            if(hit.distance < minDistance)
            {
                minDistance = hit.distance;
            }
        }

        if(_results.Count > 0)
        {
            StopCoroutine(nameof(Dodging));
            StartCoroutine(Dodging(direction.normalized * minDistance));
        }
        else
        {
            StopCoroutine(nameof(Dodging));
            StartCoroutine(Dodging(direction.normalized * _strength));
        }
    }

    private IEnumerator Dodging(Vector3 dodgeDestination)
    {
        _movement.Block();

        dodgeDestination = dodgeDestination += _transform.position;
        float distance = Vector3.Distance(_transform.position, dodgeDestination);

        while(distance > 0.2f)
        {
            _transform.position
                = Vector3.Lerp(_transform.position, dodgeDestination, _speed);

            distance = Vector3.Distance(_transform.position, dodgeDestination);
            yield return new WaitForFixedUpdate();
        }

        _movement.Unblock();
    }
}
