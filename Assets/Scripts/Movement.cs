using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float ForwardSpeed;
    public float SidesSpeed;
    public Space MoveSpace;
    private Transform _transform;
    private PlayerAnimations _animations;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _animations = GetComponentInChildren<PlayerAnimations>();
    }

    private void Update()
    {
        Vector3 move = new Vector3();
        move.y = Input.GetAxis("Vertical") * ForwardSpeed * Time.deltaTime;
        move.x = Input.GetAxis("Horizontal") * SidesSpeed * Time.deltaTime;
        _animations.Change(move);

        _transform.Translate(move, MoveSpace);
    }
}
