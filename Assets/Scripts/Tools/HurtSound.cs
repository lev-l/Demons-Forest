using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class HurtSound : MonoBehaviour
{
    [SerializeField] private AudioSource _sound;
    private Health _health;

    private void Start()
    {
        _health = GetComponent<Health>();

        _health.OnHurt += Hit;
    }

    public void Hit()
    {
        _sound.Play();
    }
}
