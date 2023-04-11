using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();

        Resources.Load<PlayerObject>("Player").OnStealthChanged += SetStealth;
    }

    public void ChangeRunState(Vector2 move)
    {
        _animator.SetBool("Run", move.sqrMagnitude > 0);
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
