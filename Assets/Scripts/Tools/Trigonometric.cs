using System;
using UnityEngine;

public static class Trigonometric
{
    public static Vector2 CreateRayEnd(float distance, float angleDegrees)
    {
        float xEnd = GetXByRotation(angleDegrees) * distance;
        float yEnd = GetYByRotation(angleDegrees) * distance;

        return new Vector2(xEnd, yEnd);
    }

    public static float AddAngle(float angle, float addition)
    {
        angle += addition;
        if (angle > 360)
        {
            angle -= 360;
        }
        else if (angle < 0)
        {
            angle += 360;
        }

        return angle;
    }

    public static float GetXByRotation(float rotation)
    {
        return Mathf.Cos(GetRoundedRotationInRadians(rotation));
    }

    public static float GetYByRotation(float rotation)
    {
        return Mathf.Sin(GetRoundedRotationInRadians(rotation));
    }

    private static float GetRoundedRotationInRadians(float rotation)
    {
        return Mathf.Round(rotation) * Mathf.Deg2Rad;
    }

    public static void RayPaint(Vector2 start, Vector2 direction)
    {
        Debug.DrawRay(start, direction, Color.red, 0.2f);
    }
}
