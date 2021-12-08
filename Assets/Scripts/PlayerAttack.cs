using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int AttackButton;
    public AnimationClip AttackAnimation;
    [SerializeField] private int _damage;
    [SerializeField] private float _attackSquareDistance;
    [SerializeField] private float _attackSquareAngle;
    private int _filterLayerMask;
    private PlayerAnimations _animations;
    private Trigonometric _trigonometric;
    private Transform _transform;

    private void Start()
    {
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
        if (Input.GetMouseButtonUp(AttackButton))
        {
            _animations.PlayAttackAnimation();
            Damage();
        }
    }

    private void Damage()
    {
        float central = Mathf.Round(_transform.eulerAngles.z);
        List<Health> damaged = new List<Health>();

        for(int modifier = -1; modifier <= 1; modifier++)
        {
            RaycastHit2D[] hits = GetRayHits(central + (_attackSquareAngle * modifier));
            foreach(RaycastHit2D hit in hits)
            {
                Health aliveTarget = hit.collider.GetComponent<Health>();
                if (aliveTarget
                    && !damaged.Contains(aliveTarget))
                {
                    aliveTarget.Hurt(_damage);
                    aliveTarget.GetComponent<Discarding>().Discard(_trigonometric.CreateRayEnd
                                                                                            (distance: 1, 
                                                                                            central + 90)
                                                                                        );
                    damaged.Add(aliveTarget);
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
}
