using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSound : MonoBehaviour
{
    [SerializeField] private AudioClip _sound;
    [SerializeField] private AudioSource _audio;
    private EnemyAttack _attack;

    private void Start()
    {
        _attack = GetComponent<EnemyAttack>();

        _attack.OnAttack += Scream;
    }

    private void Scream()
    {
        _audio.PlayOneShot(_sound);
    }
}
