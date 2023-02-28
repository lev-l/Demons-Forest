using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPresenter : MonoBehaviour
{
    private GameObject _roof;

    private void Start()
    {
        _roof = GetComponentInChildren<SpriteRenderer>().gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _roof.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _roof.SetActive(true);
    }
}
