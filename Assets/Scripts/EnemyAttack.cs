using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public AnimationCurve AttackCurve;
    public AnimationCurve BackCurve;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    private Transform _transform;
    private EnemyTools _tools;
    private Collider2D _collider;
    private ContactFilter2D _filter;
    private Transform _target;
    private bool _coroutineOngoing = false;
    private bool _collided = false;

    private void Start()
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
            StartCoroutine(nameof(Attacking));
        }
    }

    private IEnumerator Attacking()
    {
        _coroutineOngoing = true;

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

    private IEnumerator AttacksCharging(int chargeNumber)
    {
        _transform.rotation = _tools.GetNewRotation(selfPosition: _transform.position,
                                                    targetPosition: _target.position);
        yield return Charge();

        chargeNumber++;
        if (!_collided
            && chargeNumber < 3)
        {
            yield return AttacksCharging(chargeNumber);
        }
        _collided = false;
        _target = null;
    }

    private IEnumerator Charge()
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

    private (bool colided, Collider2D[] colliders) DetectCollision()
    {
        List<Collider2D> collisions = new List<Collider2D>();
        bool detectedAnyCollision = Physics2D.GetContacts(_collider, _filter, collisions) > 0;

        return (detectedAnyCollision, collisions.ToArray());
    }

    private void Damage(Health damageTarget)
    {
        damageTarget.Hurt(_damage);

        Vector2 direction = (damageTarget.GetComponent<Transform>().position - _transform.position).normalized;
        damageTarget.GetComponent<Discarding>().Discard(direction);
    }

    public void Stop()
    {
        StopAllCoroutines();
        _coroutineOngoing = false;
    }
}
