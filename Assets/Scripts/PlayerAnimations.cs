using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Change(Vector2 move)
    {
        _animator.SetBool("Run", move.magnitude > 0);
    }
}
