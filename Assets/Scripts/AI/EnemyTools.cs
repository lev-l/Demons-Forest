using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker), typeof(EnemyAnimations), typeof(RotateToTarget)),
    RequireComponent(typeof(EnemyAttack), typeof(Discarding), typeof(Health))]
public class EnemyTools : Movement
{
    private Seeker _seeker;
    private EnemyAnimations _animations;
    private RotateToTarget _rotating;
    private EnemyAttack _attacking;
    private GameObject _deathSound;
    private Discarding _discarding;
    private Health _health;
    private ContactFilter2D _filter;

    private void Awake()
    {
        _seeker = GetComponent<Seeker>();
        _animations = GetComponent<EnemyAnimations>();
        _rotating = GetComponent<RotateToTarget>();
        _attacking = GetComponent<EnemyAttack>();
        _deathSound = Resources.Load<GameObject>("DeathSound");
        _discarding = GetComponent<Discarding>();
        _health = GetComponent<Health>();

        _filter = new ContactFilter2D();
        _filter.useTriggers = false;
        _filter.useLayerMask = true;
        _filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));

        _health.OnDeath += PlayDeathSound;
    }

    protected void BuildPath(Vector2 selfPosition, Vector2 targetPosition,
                                OnPathDelegate callbackFunction)
    {
        _seeker.StartPath(selfPosition, targetPosition, callbackFunction);
    }

    public Quaternion GetNewRotation(Vector2 selfPosition, Vector2 targetPosition,
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

    protected void Attack(Transform target)
    {
        _attacking.Attack(target);
        _animations.StartAttackAnimation();
    }

    public virtual void Discard(Vector2 direction)
    {
        _discarding.Discard(direction, 1.2f, 9);
        _attacking.Stop();
    }

    protected GameObject[] GetForwardObject(Vector2 selfPosition, Quaternion selfRotation)
    {
        Vector2 rayEnd = Trigonometric.CreateRayEnd(5, selfRotation.eulerAngles.z);
        RaycastHit2D hit = Physics2D.Raycast(selfPosition, rayEnd, 5, _filter.layerMask);
        if (hit)
        {
            return new GameObject[1] { hit.collider.gameObject };
        }
        else
        {
            return new GameObject[0];
        }
    }

    public virtual void TakeDamage(int damageNumber)
    {
        _health.Hurt(damageNumber);
    }

    protected virtual void TakeHeal(int healNumber)
    {
        _health.Heal(healNumber);
    }

    public void PlayDeathSound(GameObject dead)
    {
        Instantiate(_deathSound, dead.transform.position, Quaternion.identity);
    }
}
