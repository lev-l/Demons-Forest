using System.Collections.Generic;
using UnityEngine;

public class PunchSound : MonoBehaviour
{
    [SerializeField] private float _soundRadius;
    private int _enemyLayer;

    private void Start()
    {
        _enemyLayer = Physics2D.GetLayerCollisionMask(8);
    }

    public void Noise(Vector2 position)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(position, _soundRadius, _enemyLayer);

        foreach(Collider2D enemy in enemies)
        {
            enemy.GetComponent<EnemyBaseAI>().TargetDetected(position);
        }
    }
}
