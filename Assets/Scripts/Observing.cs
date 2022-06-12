using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observing : MonoBehaviour
{
    public float ViewDistance;
    public uint ViewAngle;
    public event Action<Transform> SeePlayer;
    private Transform _transform;
    private Trigonometric _trigonometric;
    private bool _notSeeingPlayer;
    private int _layer;

    private void Start()
    {
        _trigonometric = new Trigonometric();
        _transform = GetComponent<Transform>();
        _notSeeingPlayer = true;
        _layer = Physics2D.GetLayerCollisionMask(gameObject.layer);
    }

    private void FixedUpdate()
    {
        bool playerNotDetectedOnThisUpdate = true;
        for (float angle = -ViewAngle; angle <= ViewAngle; angle += ViewAngle / 10)
        {
            Collider2D[] target = CheckRay(Mathf.Round(_transform.eulerAngles.z) + angle);
            foreach(Collider2D collider in target)
            {
                // maybe, OPTIMIZE
                if (collider.GetComponent<PlayerMovement>())
                {
                    playerNotDetectedOnThisUpdate = false;
                    if (_notSeeingPlayer)
                    {
                        SeePlayer?.Invoke(collider.transform);
                        _notSeeingPlayer = false;
                    }
                    break;
                }
            }
        }

        if (playerNotDetectedOnThisUpdate)
        {
            _notSeeingPlayer = true;
        }
    }

    private Collider2D[] CheckRay(float angle)
    {
        Vector3 rayEnd = _trigonometric.CreateRayEnd(distance: ViewDistance,
                                                    angleDegrees: angle);

        RaycastHit2D centralRayHit = Physics2D.Raycast(origin: _transform.position,
                                                        direction: rayEnd,
                                                        distance: ViewDistance,
                                                        layerMask: _layer);
        if (centralRayHit)
        {
            return new Collider2D[] { centralRayHit.collider };
        }

        _trigonometric.RayPaint(_transform.position, rayEnd);
        return new Collider2D[] { };
    }
}
