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
    private Collider2D _collider;
    private ContactFilter2D _filter;
    private bool _coroutineOngoing = false;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _collider = GetComponent<Collider2D>();

        _filter = new ContactFilter2D();
        _filter.useTriggers = false;
        _filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        _filter.useLayerMask = true;
    }

    public void Attack()
    {
        if (!_coroutineOngoing)
        {
            StartCoroutine(Attacking());
        }
    }

    private IEnumerator Attacking()
    {
        _coroutineOngoing = true;

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
                foreach(Collider2D collider in collision.colliders)
                {
                    Health aliveObject = collider.GetComponent<Health>();
                    if (aliveObject)
                    {
                        Damage(aliveObject);
                        break;
                    }
                }
                yield return new WaitForSeconds(0.1f);
                break;
            }
        }

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

    private (bool colided, Collider2D[] colliders) DetectCollision()
    {
        List<Collider2D> collisions = new List<Collider2D>();
        bool detectedAnyCollision = Physics2D.GetContacts(_collider, _filter, collisions) > 0;

        return (detectedAnyCollision, collisions.ToArray());
    }

    private void Damage(Health damageTarget)
    {
        damageTarget.Hurt(_damage);
        //damageTarget.GetComponent<Discarding>().Discard();
    }

    public void Stop()
    {
        StopAllCoroutines();
        _coroutineOngoing = false;
    }
}
