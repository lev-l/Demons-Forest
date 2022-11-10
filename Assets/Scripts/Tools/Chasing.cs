using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasing : MonoBehaviour
{
    public Transform SubjectTrasform;
    public float Speed;
    private Transform _transform;

    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        float distanceToSubject = Vector2.Distance(_transform.position, SubjectTrasform.position);
        print(distanceToSubject);

        Vector3 move = Vector2.MoveTowards(_transform.position,
                                            SubjectTrasform.position,
                                            Speed * distanceToSubject* Time.deltaTime);
        move.z = _transform.position.z;
        _transform.position = move;
    }
}
