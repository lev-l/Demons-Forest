using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int AttackButton;
    public int StabButton;
    public event Action OnSound;
    [SerializeField] private int _damage;
    [SerializeField] private float _attackSquareDistance;
    [SerializeField] private float _attackSquareAngle;
    private bool _attackPrepared;
    private bool _stabPrepared;
    private int _filterLayerMask;
    private PlayerAnimations _animations;
    private Transform _transform;
    private PlayerObject _player;

    private void Start()
    {
        _player = Resources.Load<PlayerObject>("Player");

        _animations = GetComponent<PlayerAnimations>();
        _transform = GetComponent<Transform>();

        _filterLayerMask = Physics2D.GetLayerCollisionMask(gameObject.layer);
    }

    private void Update()
    {
        if (_attackPrepared
            && !Input.GetMouseButton(AttackButton))
        {
            _animations.PlayAttackAnimation();
            OnSound?.Invoke();

            StopCoroutine(nameof(Damaging));
            StartCoroutine(nameof(Damaging));

            _attackPrepared = false;
        }
        if(_stabPrepared
            && !Input.GetMouseButton(StabButton))
        {

        }
    }

    private IEnumerator Damaging()
    {
        float central = Mathf.Round(_transform.eulerAngles.z);
        List<EnemyBaseAI> damaged = new List<EnemyBaseAI>();

        for (int n = 0; n < 5; n++)
        {
            ///dealing damage
            for (int modifier = -1; modifier <= 1; modifier++)
            {
                RaycastHit2D[] hits = GetRayHits(central + (_attackSquareAngle * modifier));
                foreach (RaycastHit2D hit in hits)
                {
                    EnemyBaseAI enemy = hit.collider.GetComponent<EnemyBaseAI>();
                    if (enemy
                        && !damaged.Contains(enemy))
                    {
                        enemy.TakeDamage(_player.StealthMode && _player.NumberEnemiesSeeYou == 0 ?
                                                        _damage * 5 : _damage);
                        enemy.Discard(Trigonometric.CreateRayEnd(distance: 0.8f, central + 90));
                        damaged.Add(enemy);
                    }
                    else if (!enemy
                        && !hit.collider.isTrigger)
                    {
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    private RaycastHit2D[] GetRayHits(float angle)
    {
        Vector2 direction = Trigonometric.CreateRayEnd(_attackSquareDistance, angle + 90);
        RaycastHit2D[] hits = Physics2D.RaycastAll(_transform.position, direction,
                                                    _attackSquareDistance, _filterLayerMask);
        Trigonometric.RayPaint(_transform.position, direction * _attackSquareDistance);

        return hits;
    }

    public void AttackPrepared()
    {
        _attackPrepared = true;
    }
}
