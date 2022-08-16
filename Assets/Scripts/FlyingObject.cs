using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingObject : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _flyDistance;
    private Transform _transform;
    private RaycastHit2D[] _hitsBuffer;
    private ContactFilter2D _filter;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _hitsBuffer = new RaycastHit2D[3];
        _filter = new ContactFilter2D();

        _filter.useTriggers = false;
        _filter.useLayerMask = true;
        _filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    }

    private void Update()
    {
        _transform.Translate(Vector3.up * _speed * Time.deltaTime, Space.Self);
    }

    private void FixedUpdate()
    {
        int resultsNum = Physics2D.Raycast(origin: _transform.position,
                                            direction: _transform.TransformDirection(Vector2.up),
                                            contactFilter: _filter,
                                            results: _hitsBuffer,
                                            distance: 0.25f);
        if(resultsNum > 0)
        {
            List<RaycastHit2D> hits = new List<RaycastHit2D>(resultsNum);
        }
    }
}
