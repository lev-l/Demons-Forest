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
    [SerializeField] private float _stabDistance;
    [SerializeField] private float _attackSquareAngle;
    [SerializeField] private float _stabAngle;
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
        if (Input.GetMouseButtonDown(AttackButton))
        {
            _animations.PrepareAttackAnimation();
        }
        if (Input.GetMouseButtonDown(StabButton))
        {
            _animations.PrepareStabAnimation();
        }

        if (_attackPrepared
            && !Input.GetMouseButton(AttackButton))
        {
            _animations.PlayAttackAnimation();
            OnSound?.Invoke();

            StopCoroutine(nameof(DamagingAttack));
            StartCoroutine(nameof(DamagingAttack));

            _attackPrepared = false;
        }
        if(_stabPrepared
            && !Input.GetMouseButton(StabButton))
        {
            _animations.PlayStabAnimation();
            OnSound?.Invoke();

            StopCoroutine(nameof(DamagingStab));
            StartCoroutine(nameof(DamagingStab));

            _stabPrepared = false;
        }
    }

    private IEnumerator DamagingAttack()
    {
        List<EnemyBaseAI> damaged = new List<EnemyBaseAI>();

        for (int n = 0; n < 5; n++)
        {
            float central = Mathf.Round(_transform.eulerAngles.z);
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
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator DamagingStab()
    {
        List<EnemyBaseAI> damaged = new List<EnemyBaseAI>();
        _player.BlockRotation();

        for (int n = 0; n < 5; n++)
        {
            float central = Mathf.Round(_transform.eulerAngles.z);
            ///dealing damage
            for (int modifier = -1; modifier <= 1; modifier++)
            {
                RaycastHit2D[] hits = GetRayHitsStab(central + (_stabAngle * modifier));
                foreach (RaycastHit2D hit in hits)
                {
                    EnemyBaseAI enemy = hit.collider.GetComponent<EnemyBaseAI>();
                    if (enemy
                        && !damaged.Contains(enemy))
                    {
                        enemy.TakeDamage(_player.StealthMode && _player.NumberEnemiesSeeYou == 0 ?
                                                        _damage * 5 : _damage);
                        enemy.Discard(Trigonometric.CreateRayEnd(distance: 1.2f, central + 90));
                        damaged.Add(enemy);
                    }
                    else if (!enemy
                        && !hit.collider.isTrigger)
                    {
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.05f);
        }

        _player.ReleaseRotation();
    }

    private RaycastHit2D[] GetRayHits(float angle)
    {
        Vector2 direction = Trigonometric.CreateRayEnd(_attackSquareDistance, angle + 90).normalized;
        RaycastHit2D[] hits = Physics2D.RaycastAll(_transform.position + (Vector3.right * 0.5f), direction,
                                                    _attackSquareDistance, _filterLayerMask);
        Trigonometric.RayPaint(_transform.position + (Vector3.right * 0.3f), direction * _attackSquareDistance);

        return hits;
    }

    private RaycastHit2D[] GetRayHitsStab(float angle)
    {
        Vector2 direction = Trigonometric.CreateRayEnd(_stabDistance, angle + 90).normalized;
        RaycastHit2D[] hits = Physics2D.RaycastAll(_transform.position + (Vector3.right * 0.5f), direction,
                                                    _stabDistance, _filterLayerMask);
        Trigonometric.RayPaint(_transform.position + (Vector3.right * 0.5f), direction * _stabDistance);

        return hits;
    }

    public void AttackPrepared()
    {
        _attackPrepared = true;
    }

    public void StabPrepared()
    {
        _stabPrepared = true;
    }
}
