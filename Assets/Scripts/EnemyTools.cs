using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker), typeof(EnemyAnimations), typeof(RotateToTarget)),
    RequireComponent(typeof(EnemyAttack), typeof(Discarding), typeof(Health))]
public class EnemyTools : MonoBehaviour
{
    private Seeker _seeker;
    private EnemyAnimations _animations;
    private RotateToTarget _rotating;
    private EnemyAttack _attacking;
    private Discarding _discarding;
    private Health _health;

    private void Awake()
    {
        _seeker = GetComponent<Seeker>();
        _animations = GetComponent<EnemyAnimations>();
        _rotating = GetComponent<RotateToTarget>();
        _attacking = GetComponent<EnemyAttack>();
        _discarding = GetComponent<Discarding>();
        _health = GetComponent<Health>();
    }

    protected void BuildPath(Vector2 selfPosition, Vector2 targetPosition,
                                OnPathDelegate callbackFunction)
    {
        _seeker.StartPath(selfPosition, targetPosition, callbackFunction);
    }

    protected Quaternion GetNewRotation(Vector2 selfPosition, Vector2 targetPosition,
                                    float angle = 0)
    {
        float zRotation = _rotating.Rotate(selfPosition, targetPosition, angle);
        Vector3 newRotation = Vector3.forward * zRotation;
        return Quaternion.Euler(newRotation);
    }

    protected void StartGoAnimation()
    {
        _animations.StartGoAnimation();
    }

    protected void StopGoAnimation()
    {
        _animations.StopGoAnimation();
    }

    protected void Attack()
    {
        _attacking.Attack();
        _animations.StartAttackAnimation();
    }

    public virtual void Discard(Vector2 direction)
    {
        _discarding.Discard(direction);
        _attacking.Stop();
    }

    public virtual void Damage(int damageNumber)
    {
        _health.Hurt(damageNumber);
    }

    protected virtual void Heal(int healNumber)
    {
        _health.Heal(healNumber);
    }
}
