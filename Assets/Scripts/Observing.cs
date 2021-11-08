using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observing : MonoBehaviour
{
    public float ViewDistance;
    public int ViewAngle;
    public Transform _transform;
    //private RaycastHit2D 

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        //Physics2D.Raycast(_transform.position, )
    }

    private void OnDrawGizmos()
    {
        Vector2 direction = CreateRayEnd(distance: ViewDistance,
                                        angleDegrees: 0);
        Vector2 directionLeftCorner = CreateRayEnd(distance: ViewDistance,
                                                    angleDegrees: ViewAngle);
        Vector2 directionRightCorner = CreateRayEnd(distance: ViewDistance,
                                                    angleDegrees: -ViewAngle);

        RayPaint(direction);
        RayPaint(directionLeftCorner);
        RayPaint(directionRightCorner);
    }

    private Vector2 CreateRayEnd(float distance, float angleDegrees)
    {
        Trigonometric trigonometric = new Trigonometric();

        float xEnd = trigonometric.GetXByRotation(
                            AddAngle(_transform.eulerAngles.z, angleDegrees)) * distance;
        float yEnd = trigonometric.GetYByRotation(
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
