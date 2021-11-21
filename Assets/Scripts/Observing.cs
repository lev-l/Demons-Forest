using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observing : MonoBehaviour
{
    public float ViewDistance;
    public uint ViewAngle;
    public event Action<GameObject> SeePlayer;
    private Transform _transform;
    private Trigonometric _trigonometric;
    private bool NotSeeingPlayer;

    private void Start()
    {
        _trigonometric = new Trigonometric();
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        for (float angle = -ViewAngle; angle <= ViewAngle; angle += ViewAngle / 10)
        {
            Collider2D[] target = CheckRay(angle);
            bool playerNotDetectedOnThisUpdate = true;
            foreach(Collider2D collider in target)
            {
                // maybe, OPTIMIZE
                if (NotSeeingPlayer
                    && collider.GetComponent<PlayerMovement>())
                {
                    SeePlayer?.Invoke(collider.gameObject);
                    playerNotDetectedOnThisUpdate = false;
                    NotSeeingPlayer = false;
                    print("only once");
                    break;
                }
            }
            if (playerNotDetectedOnThisUpdate)
            {
                NotSeeingPlayer = true;
            }
        }
    }

    private Collider2D[] CheckRay(float angle)
    {
        Vector3 rayEnd = CreateRayEnd(distance: ViewDistance,
                                        angleDegrees: angle);

        RaycastHit2D centralRayHit = Physics2D.Raycast(origin: _transform.position,
                                                        direction: rayEnd,
                                                        distance: ViewDistance);
        if (centralRayHit)
        {
            return new Collider2D[] { centralRayHit.collider };
        }

        //Debug.DrawRay(_transform.position, rayEnd, Color.red);
        return new Collider2D[] { };
    }

    private Vector2 CreateRayEnd(float distance, float angleDegrees)
    {
        float xEnd = _trigonometric.GetXByRotation(
                            AddAngle(_transform.eulerAngles.z, angleDegrees)) * distance;
        float yEnd = _trigonometric.GetYByRotation(
                            AddAngle(_transform.eulerAngles.z, angleDegrees)) * distance;

        return new Vector2(xEnd, yEnd);
    }

    private float AddAngle(float angle, float addition)
    {
        angle += addition;
        if(angle > 360)
        {
            angle -= 360;
        }
        else if(angle < 0)
        {
            angle += 360;
        }

        return angle;
    }

    private void RayPaint(Vector2 direction)
    {
        Debug.DrawRay(_transform.position, direction, Color.red);
    }

        private class Trigonometric
        {
            public float GetXByRotation(float rotation)
            {
                return Mathf.Cos(GetRoundedRotationInRadians(rotation));
            }
            
            public float GetYByRotation(float rotation)
            {
                return Mathf.Sin(GetRoundedRotationInRadians(rotation));
            }
            
            private float GetRoundedRotationInRadians(float rotation)
            {
                return Mathf.Round(rotation) * Mathf.Deg2Rad;
            }
        }
}
