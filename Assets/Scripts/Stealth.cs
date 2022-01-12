using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Stealth : MonoBehaviour
{
    [SerializeField] private KeyCode _stealthKey;
    private PlayerMovement _movement;

    private void Start()
    {
        _movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_stealthKey))
        {
            _movement.ChangeStealthMod();
        }
    }
}
