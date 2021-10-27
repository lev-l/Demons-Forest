using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float ForwardSpeed;
    public float SidesSpeed;
    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        Vector3 move = new Vector3();
        move.y = Input.GetAxis("Vertical") * ForwardSpeed * Time.deltaTime;
        move.x = Input.GetAxis("Horizontal") * SidesSpeed * Time.deltaTime;

        _transform.Translate(move, Space.Self);
    }
}
