using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyBaseAI))]
public class EnemyAttack : MonoBehaviour
{
    public AnimationCurve AttackCurve;
    public AnimationCurve BackCurve;
    public event Action OnAttack;
    [SerializeField] protected float _speed;
    [SerializeField] protected int _damage;
    [SerializeField] protected int _chargesMax;
    protected Transform _transform;
    protected EnemyTools _tools;
    protected Collider2D _collider;
    protected ContactFilter2D _filter;
    protected Transform _target;
    protected bool _coroutineOngoing = false;
    protected bool _collided = false;

    protected virtual void Start()
    {
        _transform = GetComponent<Transform>();
        _tools = GetComponent<EnemyTools>();
        _collider = GetComponent<Collider2D>();

        _filter = new ContactFilter2D();
        _filter.useTriggers = false;
        _filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        _filter.useLayerMask = true;
    }

    public void Attack(Transform target)
    {
        _target = target;
        if (!_coroutineOngoing)
        {
            OnAttack?.Invoke();
            StartCoroutine(nameof(Attacking));
        }
    }

    protected virtual IEnumerator Attacking()
    {
        _coroutineOngoing = true;
        yield return new WaitForSeconds(0.1f);

        float duration = AttackCurve.keys[AttackCurve.keys.Length - 1].time;
        float currentTime = 0f;
        Vector2 newPosition = Vector2.zero;

        yield return AttacksCharging(0);

        duration = BackCurve.keys[BackCurve.keys.Length - 1].time;
        currentTime = 0;
        while (currentTime <= duration)
        {
            yield return new WaitForEndOfFrame();
            newPosition.x = -BackCurve.Evaluate(currentTime) * _speed;
            _transform.Translate(newPosition * Time.deltaTime, Space.Self);
            currentTime += Time.deltaTime;

            if (DetectCollision().colided)
            {
                break;
            }
        }

        _coroutineOngoing = false;
    }

    protected IEnumerator AttacksCharging(int chargeNumber)
    {
        _transform.rotation = _tools.GetNewRotation(selfPosition: _transform.position,
                                                    targetPosition: _target.position);
        yield return Charge();

        // For a logner charge if enemy does not collide with anything;
        chargeNumber++;
        if (!_collided
            && chargeNumber < _chargesMax)
        {
            yield return AttacksCharging(chargeNumber);
        }
        _collided = false;
        _target = null;
    }

    protected virtual IEnumerator Charge()
    {
        float duration = AttackCurve.keys[AttackCurve.keys.Length - 1].time;
        float currentTime = 0f;
        Vector2 newPosition = Vector2.zero;

        while (currentTime <= duration)
        {
            yield return new WaitForEndOfFrame();
            newPosition.x = AttackCurve.Evaluate(currentTime) * _speed;
            _transform.Translate(newPosition * Time.deltaTime, Space.Self);
            currentTime += Time.deltaTime;

            (bool collided, Collider2D[] colliders) collision = DetectCollision();
            if (collision.collided)
            {
                foreach (Collider2D collider in collision.colliders)
                {
                    Health aliveObject = collider.GetComponent<Health>();
                    if (aliveObject)
                    {
                        Damage(aliveObject);
                        break;
                    }
                }
                _collided = true;
                yield return new WaitForSeconds(0.1f);
                break;
            }
        }
    }

    protected (bool colided, Collider2D[] colliders) DetectCollision()
    {
        List<Collider2D> collisions = new List<Collider2D>();
        bool detectedAnyCollision = _collider.GetContacts(collisions) > 0;
        return (detectedAnyCollision, collisions.ToArray());
    }

    protected virtual void Damage(Health damageTarget)
    {
        damageTarget.Hurt(_damage);

        Vector2 direction = (damageTarget.GetComponent<Transform>().position - _transform.position).normalized;
        damageTarget.GetComponent<Discarding>().Discard(direction, 1f, 10);
    }

    public void Stop()
    {
        StopAllCoroutines();
        _coroutineOngoing = false;
    }
}
