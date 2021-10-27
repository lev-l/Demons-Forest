using System.Collections;
using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    public float Rotate(Vector3 position, Vector3 target,
                            float angle = 0)
    {
        Vector3 direction = target - position;
        float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angle;
        return rotation;
    }
}
