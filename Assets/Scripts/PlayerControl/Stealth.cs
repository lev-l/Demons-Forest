using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Stealth : MonoBehaviour
{
    [SerializeField] private KeyCode _stealthKey;
    private PlayerObject _player;

    private void Start()
    {
        _player = Resources.Load<PlayerObject>("Player");
    }

    private void Update()
    {
        if (Input.GetKeyDown(_stealthKey))
        {
            _player.ChangeStealthMode();
        }
    }
}
