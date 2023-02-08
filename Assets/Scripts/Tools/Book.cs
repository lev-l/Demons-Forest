using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    private bool _playerInRange;

    private void Start()
    {
        _playerInRange = false;
    }

    private void Update()
    {
        if(_playerInRange
            && Input.GetKeyDown(KeyCode.E))
        {
            print("Open");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _playerInRange = false;
    }
}
