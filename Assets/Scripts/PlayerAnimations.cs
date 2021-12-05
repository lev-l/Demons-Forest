using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void ChangeRunState(Vector2 move)
    {
        _animator.SetBool("Run", move.magnitude > 0);
    }

    public void PlayAttackAnimation()
    {
        _animator.Play("PlayerAtack");
    }
}
