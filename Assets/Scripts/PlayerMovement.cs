using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimations), typeof(StepsSound))]
public class PlayerMovement : MonoBehaviour
{
    public float ForwardSpeed;
    public float SidesSpeed;
    public Space MoveSpace;
    private bool _coroutineOngoing;
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

        if(!_coroutineOngoing
            && move.sqrMagnitude > 0)
        {
            _coroutineOngoing = true;
            StartCoroutine(Noising());
        }
        else if(move.sqrMagnitude == 0)
        {
            _coroutineOngoing = false;
            StopAllCoroutines();
        }

        _transform.Translate(move, MoveSpace);
    }

    private IEnumerator Noising()
    {
        while (true)
        {
            _stepsSound.Noise();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
