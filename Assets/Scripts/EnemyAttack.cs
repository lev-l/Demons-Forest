using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public AnimationCurve AttackCurve;
    [SerializeField] private float _speed;
    private Transform _transform;
    private Collider2D _collider;
    private ContactFilter2D _filter;
    private bool _coroutineOngoing = false;

    private void Start()
    {
        _transform = GetComponent<Transform>();

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
            newPosition = EvaluatePosition(currentTime);
            _transform.Translate(newPosition * Time.deltaTime, Space.Self);
            currentTime += Time.deltaTime;

            DetectCollision();
        }

        currentTime = 0;
        while (currentTime <= duration)
        {
            yield return new WaitForEndOfFrame();
            newPosition = -EvaluatePosition(currentTime);
            _transform.Translate(newPosition * Time.deltaTime, Space.Self);
            currentTime += Time.deltaTime;

            DetectCollision();
        }

        _coroutineOngoing = false;
    }

    private Vector2 EvaluatePosition(float time)
    {
        Vector2 newPosition = Vector2.zero;
        newPosition.x = AttackCurve.Evaluate(time) * _speed;
        return newPosition;
    }

    private void DetectCollision()
    {
        List<Collider2D> collisions = new List<Collider2D>();
        Physics2D.GetContacts(_collider, _filter, collisions);

        foreach(Collider2D collision in collisions)
        {

        }
    }
}
