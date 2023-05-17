using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _stepsSound;
    private bool _wasMoving = false;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _stepsSound = GetComponent<AudioSource>();

        Resources.Load<PlayerObject>("Player").OnStealthChanged += SetStealth;
    }

    public void ChangeRunState(Vector2 move)
    {
        bool isMoving = move.sqrMagnitude > 0;
        bool change = isMoving != _wasMoving;
        _wasMoving = isMoving;

        _animator.SetBool("Run", isMoving);

        if (change && isMoving)
        {
            _stepsSound.Play();
        }
        else if (change)
        {
            _stepsSound.Pause();
        }
    }

    public void PrepareAttackAnimation()
    {
        _animator.SetTrigger("Prepare");
    }

    public void PlayAttackAnimation()
    {
        _animator.SetTrigger("Attack");
    }

    public void SetStealth(bool stealth)
    {
        _animator.SetBool("Stealth", stealth);
    }
}
