using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public AnimationCurve BackCurve;
    [SerializeField] private float _speedOfGoBack;
    [SerializeField] private int _damage;
    [SerializeField] private float _maxEnergy;
    [SerializeField] private float _speedUp;
    private float _currentEnergy;
    private bool _attacking = false;
    private bool _notRecharging = true;
    private float _normalSpeed;
    private EnemyBaseAI _movementAI;
    private Transform _transform;
    private Collider2D _collider;
    private ContactFilter2D _filter;

    private void Start()
    {
        _currentEnergy = _maxEnergy;
        _movementAI = GetComponent<EnemyBaseAI>();
        _transform = GetComponent<Transform>();
        _collider = GetComponent<Collider2D>();
        _normalSpeed = _movementAI.Speed;

        _filter = new ContactFilter2D();
        _filter.useTriggers = false;
        _filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        _filter.useLayerMask = true;
    }

    private void Update()
    {
        if (_attacking)
        {
            _currentEnergy -= 0.5f * Time.deltaTime;
        }
        else if(_currentEnergy < _maxEnergy)
        {
            _currentEnergy += 0.1f * Time.deltaTime;
        }

        if(_currentEnergy <= 0)
        {
            _attacking = false;
            _notRecharging = false;
            _movementAI.Speed = _normalSpeed;
        }
        if(_currentEnergy >= _maxEnergy)
        {
            _currentEnergy = _maxEnergy;
            _notRecharging = true;
        }
    }

    public void Attack()
    {
        if (_notRecharging)
        {
            _movementAI.Speed *= _speedUp;
            _attacking = true;

            StopCoroutine(nameof(CheckingForAttack));
            StartCoroutine(nameof(CheckingForAttack));
        }
    }

    private IEnumerator CheckingForAttack()
    {
        while (_notRecharging)
        {
            (bool colided, Collider2D[] colliders) collision = DetectCollision();
            foreach (Collider2D collider in collision.colliders)
            {
                if (collider.GetComponent<PlayerMovement>())
                {
                    Damage(collider.GetComponent<Health>());
                    _movementAI.Block();
                    _movementAI.Speed = _normalSpeed;
                    _attacking = false;
                    StopCoroutine(nameof(GoBack));
                    StartCoroutine(nameof(GoBack));

                    StopCoroutine(nameof(CheckingForAttack));
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator GoBack()
    {
        float duration = BackCurve.keys[BackCurve.keys.Length - 1].time;
        float currentTime = 0;
        Vector2 newPosition = Vector2.zero;

        while (currentTime <= duration)
        {
            yield return new WaitForEndOfFrame();
            newPosition.x = -BackCurve.Evaluate(currentTime) * _speedOfGoBack;
            _transform.Translate(newPosition * Time.deltaTime, Space.Self);
            currentTime += Time.deltaTime;

            if (DetectCollision().colided)
            {
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
        _currentEnergy = -1;
    }
}
