using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int AttackButton;
    [SerializeField] private int _damage;
    [SerializeField] private float _attackSquareDistance;
    [SerializeField] private float _attackSquareAngle;
    private bool _attackPrepared;
    private int _filterLayerMask;
    private PlayerAnimations _animations;
    private Trigonometric _trigonometric;
    private Transform _transform;
    private PlayerObject _player;

    private void Start()
    {
        _player = Resources.Load<PlayerObject>("Player");

        _animations = GetComponent<PlayerAnimations>();
        _trigonometric = new Trigonometric();
        _transform = GetComponent<Transform>();

        _filterLayerMask = Physics2D.GetLayerCollisionMask(gameObject.layer);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(AttackButton))
        {
            _animations.PrepareAttackAnimation();
        }
        if (_attackPrepared
            && !Input.GetMouseButton(AttackButton))
        {
            _animations.PlayAttackAnimation();
            Damage();

            _attackPrepared = false;
        }
    }

    private void Damage()
    {
        float central = Mathf.Round(_transform.eulerAngles.z);
        List<EnemyBaseAI> damaged = new List<EnemyBaseAI>();

        for(int modifier = -1; modifier <= 1; modifier++)
        {
            RaycastHit2D[] hits = GetRayHits(central + (_attackSquareAngle * modifier));
            foreach(RaycastHit2D hit in hits)
            {
                EnemyBaseAI enemy = hit.collider.GetComponent<EnemyBaseAI>();
                if (enemy
                    && !damaged.Contains(enemy))
                {
                    enemy.TakeDamage(_player.StealthMode && _player.NumberEnemiesSeeYou == 0 ?
                                                    _damage * 5 : _damage);
                    enemy.Discard(_trigonometric.CreateRayEnd(distance: 1, central + 90));
                    damaged.Add(enemy);
                }
            }
        }
    }

    private RaycastHit2D[] GetRayHits(float angle)
    {
        Vector2 direction = _trigonometric.CreateRayEnd(_attackSquareDistance, angle + 90);
        RaycastHit2D[] hits = Physics2D.RaycastAll(_transform.position, direction,
                                                    _attackSquareDistance, _filterLayerMask);
        _trigonometric.RayPaint(_transform.position, direction * _attackSquareDistance);

        return hits;
    }

    public void AttackPrepared()
    {
        _attackPrepared = true;
    }
}
