using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement), typeof(PlayerEnergy), typeof(Collider2D))]
public class Dodge : MonoBehaviour
{
    [SerializeField] private float _strength;
    [SerializeField, Range(0.1f, 1)] private float _speed;
    [SerializeField] private int _energyCost;
    private PlayerObject _player;
    private PlayerEnergy _energyBuffer;
    private Transform _transform;
    private Movement _playerMovement;
    private Collider2D _collider;
    private List<RaycastHit2D> _results;
    private ContactFilter2D _filter;

    private void Start()
    {
        _player = Resources.Load<PlayerObject>("Player");
        _energyBuffer = GetComponent<PlayerEnergy>();
        _transform = GetComponent<Transform>();
        _playerMovement = GetComponent<Movement>();
        _collider = GetComponent<Collider2D>();
        _results = new List<RaycastHit2D>();
        _filter = new ContactFilter2D();

        _filter.useTriggers = false;
        _filter.useLayerMask = true;
        _filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    }

    public void DoDodge(Vector3 direction)
    {
        if (_energyBuffer.UseEnergy(_energyCost))
        {
            _collider.Cast(direction, _filter, _results, _strength);

            float minDistance = _strength;
            foreach (RaycastHit2D hit in _results)
            {
                if (hit.distance < minDistance)
                {
                    minDistance = hit.distance;
                }
            }
            if(minDistance <= 0.3f)
            {
                _energyBuffer.ReturnEnergy(_energyCost);
            }

            StopCoroutine(nameof(Dodging));
            StartCoroutine(Dodging(direction.normalized * minDistance));
        }
    }

    private IEnumerator Dodging(Vector3 dodgeDestination)
    {
        _playerMovement.Block();

        dodgeDestination = dodgeDestination += _transform.position;
        Vector3 startPoint = _transform.position;
        float distance = Vector3.Distance(_transform.position, dodgeDestination);
        float index = 0;

        while(distance > 0.1f)
        {
            index += _speed;
            if(index > 1)
            {
                index = 1;
            }

            _transform.position
                = Vector3.Lerp(startPoint, dodgeDestination, index);

            distance = Vector3.Distance(_transform.position, dodgeDestination);
            yield return new WaitForFixedUpdate();
        }

        _playerMovement.Unblock();
    }
}
