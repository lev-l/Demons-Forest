using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTo : MonoBehaviour
{
    public Transform SubjectTrasform;
    [Range(0, 1)] public float Speed;
    private Transform _transform;

    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 newPosition = Vector2.Lerp(_transform.position,
                                            SubjectTrasform.position,
                                            Speed);
        newPosition.z = _transform.position.z;

        _transform.position = newPosition;
    }
}
