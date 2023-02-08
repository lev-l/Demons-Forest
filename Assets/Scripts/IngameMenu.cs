using System;
using System.Collections.Generic;
using UnityEngine;

public class IngameMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _menuButton;
    [SerializeField] private GameObject _texts;
    private PlayerMovement _player;

    private void Start()
    {
        _player = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _menu.SetActive(!_menu.activeSelf);
            _menuButton.SetActive(!_menuButton.activeSelf);
            _texts.SetActive(false);
            TimeChange();
        }
    }

    public void TimeChange()
    {
        if (_menu.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        if (_player.NotBlocked)
        {
            _player.Block();
        }
        else
        {
            _player.Unblock();
        }
    }
}
