using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[RequireComponent(typeof(FlyingObject))]
public class FlyingObject : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _flyDistance;
    private Transform _transform;
    private float _completedDistance;
    private RaycastHit2D[] _hitsBuffer;
    private ContactFilter2D _filter;
    private FlyingObjectDamage _damager;
    private PunchSound _sound;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _hitsBuffer = new RaycastHit2D[3];
        _filter = new ContactFilter2D();
        _damager = GetComponent<FlyingObjectDamage>();
        _sound = FindObjectOfType<PunchSound>();

        _filter.useTriggers = false;
        _filter.useLayerMask = true;
        _filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    }

    private void Update()
    {
        Vector3 move = Vector3.left * _speed * Time.deltaTime;
        _transform.Translate(move, Space.Self);
        _completedDistance += move.magnitude;

        if(_completedDistance >= _flyDistance)
        {
            Destroy(gameObject);
        }
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
            for(int i = 0; i < resultsNum; i++)
            {
                hits.Add(_hitsBuffer[i]);
            }

            foreach(RaycastHit2D hit in hits)
            {
                Health target = hit.collider.GetComponent<Health>();
                AudioSource hitSound = hit.collider.GetComponent<AudioSource>();

                if (target)
                {
                    _damager.DamageOn(target);
                }
                else if (hitSound)
                {
                    hitSound.Play();
                }

                _sound.Noise(_transform.TransformPoint(Vector3.down));
            }

            Destroy(gameObject);
        }
    }
}
