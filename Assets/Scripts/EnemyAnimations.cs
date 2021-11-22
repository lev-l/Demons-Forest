using System.Collections;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void StartGoAnimation()
    {
        _animator.SetBool("Go", true);
    }

    public void StopGoAnimation()
    {
        _animator.SetBool("Go", false);
    }
}