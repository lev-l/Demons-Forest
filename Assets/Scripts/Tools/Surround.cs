using UnityEngine;

public class Surround
{
    private Trigonometric _angleFunctions;

    public Surround()
    {
        _angleFunctions = new Trigonometric();
    }

    public float[] FindAngles(float basicAngle, int numberOfAngles)
    {
        float angle = 360f / numberOfAngles;

        float[] directionsAngles = new float[numberOfAngles];
        for(int i = 0; i < numberOfAngles; i++)
        {
            directionsAngles[i] = _angleFunctions.AddAngle(basicAngle, angle * i);
        }

        return directionsAngles;
    }

    public Vector2[] FindDestinations(float[] angles,
                                float distanceFromTarget)
    {
        Vector2[] results = new Vector2[angles.Length];

        for(int i = 0; i < angles.Length; i++)
        {
            results[i] = _angleFunctions.CreateRayEnd(distanceFromTarget, angles[i]);


            _angleFunctions.RayPaint(Vector2.zero, results[i]);
        }

        return results;
    }
}
