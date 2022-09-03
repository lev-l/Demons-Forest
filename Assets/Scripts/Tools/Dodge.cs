using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{
    [SerializeField] private float _strength;
    [SerializeField, Range(0.1f, 1)] private float _speed;
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

    public void DoDodge(Vector3 direction)
    {
        _collider.Cast(direction, _filter, _results, _strength);

        foreach (RaycastHit2D hit in _results)
        {
            StopCoroutine(nameof(Dodging));
            StartCoroutine(Dodging(direction.normalized * hit.distance));
        }

        if(_results.Count == 0)
        {
            StopCoroutine(nameof(Dodging));
            StartCoroutine(Dodging(direction.normalized * _strength));
        }
    }

    private IEnumerator Dodging(Vector3 dodgeDestination)
    {
        float distance = Vector3.Distance(_transform.position, dodgeDestination);

        while(distance > 0.2f)
        {
            print(distance);
            print(Vector3.Lerp(_transform.position, dodgeDestination, _speed));
            print(Time.deltaTime);
            _transform.position
                += Vector3.Lerp(_transform.position, dodgeDestination, _speed) * Time.deltaTime;

            distance = Vector3.Distance(_transform.position, dodgeDestination);
            yield return new WaitForFixedUpdate();
        }
    }
}
