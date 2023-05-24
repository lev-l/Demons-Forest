using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioSource DodgeAudio, AttackAudio;
    private Dodge _dodge;
    private PlayerAttack _attack;

    private void Start()
    {
        _dodge = GetComponent<Dodge>();
        _attack = GetComponent<PlayerAttack>();

        _dodge.OnSound += DodgeSound;
        _attack.OnSound += AttackSound;
    }

    public void DodgeSound()
    {
        DodgeAudio.Play();
    }

    public void AttackSound()
    {
        AttackAudio.Play();
    }
}
