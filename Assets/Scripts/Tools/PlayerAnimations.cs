using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _stepsSound;
    private bool _wasMoving = false;
    private bool _wasStealth = false;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _stepsSound = GetComponent<AudioSource>();

        Resources.Load<PlayerObject>("Player").OnStealthChanged += SetStealth;
    }

    public void ChangeRunState(Vector2 move)
    {
        bool isMoving = move.sqrMagnitude > 0;
        bool change = isMoving != _wasMoving || _isStealth != _wasStealth;
        _wasMoving = isMoving;
        _wasStealth = _isStealth;

        _animator.SetBool("Run", isMoving);

        if (change && isMoving
            && !_isStealth)
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
        _animator.SetTrigger("PrepareAttack");
    }

    public void PrepareStabAnimation()
    {
        _animator.SetTrigger("PrepareStab");
    }

    public void PlayAttackAnimation()
    {
        _animator.SetTrigger("Attack");
    }

    public void PlayStabAnimation()
    {
        _animator.SetTrigger("Stab");
    }

    public void SetStealth(bool stealth)
    {
        _animator.SetBool("Stealth", stealth);
    }

    private bool _isStealth => _animator.GetBool("Stealth");
}
