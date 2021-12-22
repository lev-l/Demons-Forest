using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimations), typeof(StepsSound))]
public class PlayerMovement : MonoBehaviour
{
    public float ForwardSpeed;
    public float SidesSpeed;
    public Space MoveSpace;
    private Transform _transform;
    private StepsSound _stepsSound;
    private PlayerAnimations _animations;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _stepsSound = GetComponent<StepsSound>();
        _animations = GetComponent<PlayerAnimations>();
    }

    private void Update()
    {
        Vector3 move = new Vector3();
        move.y = Input.GetAxis("Vertical") * ForwardSpeed * Time.deltaTime;
        move.x = Input.GetAxis("Horizontal") * SidesSpeed * Time.deltaTime;
        _animations.ChangeRunState(move);
        if(move.sqrMagnitude > 0)
        {
            _stepsSound.Noise();
        }

        _transform.Translate(move, MoveSpace);
    }
}
