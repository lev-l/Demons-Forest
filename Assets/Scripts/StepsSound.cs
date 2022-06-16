using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsSound : MonoBehaviour
{
    [SerializeField] private float _soundRadius;
    private Transform _noisyPosition;
    private int _enemyLayer;

    private void Start()
    {
        _noisyPosition = GetComponent<Transform>();
        _enemyLayer = Physics2D.GetLayerCollisionMask(8);
    }

    public void Noise()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_noisyPosition.position, _soundRadius, _enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            enemy.GetComponent<EnemyBaseAI>().TargetDetected(_noisyPosition.position);
        }
    }
}
