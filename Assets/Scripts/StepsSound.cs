using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsSound : MonoBehaviour
{
    [SerializeField] private float _soundRadius;
    private Transform _noisyPosition;

    private void Start()
    {
        _noisyPosition = GetComponent<Transform>();
    }

    public void Noise()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_noisyPosition.position, _soundRadius, 2);

        foreach(Collider2D enemy in enemies)
        {

        }
    }
}
