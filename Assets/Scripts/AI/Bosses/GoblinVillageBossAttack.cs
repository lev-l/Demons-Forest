using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GoblinVillageBossPhases))]
public class GoblinVillageBossAttack : EnemyAttack
{
    private GoblinVillageBossPhases _phases;

    protected override void Start()
    {
        base.Start();
        _phases = GetComponent<GoblinVillageBossPhases>();
    }

    protected override IEnumerator Attacking()
    {
        _coroutineOngoing = true;

        float waitTime = 0.5f;
        float currentTime = 0f;
        while(currentTime < waitTime)
        {
            _transform.rotation = _tools.GetNewRotation(selfPosition: _transform.position,
                                                        targetPosition: _target.position);
            yield return null;
            currentTime += Time.deltaTime;
        }

        Vector2 newPosition = Vector2.zero;

        yield return AttacksCharging(0);

        _coroutineOngoing = false;
    }

    protected override IEnumerator Charge()
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
            print(collision.collided);
            if (collision.collided)
            {
                foreach (Collider2D collider in collision.colliders)
                {
                    Health aliveObject = collider.GetComponent<Health>();
                    print((bool)aliveObject);
                    if (aliveObject)
                    {
                        Damage(aliveObject);
                        break;
                    }
                    else
                    {
                        if (collider.CompareTag("DestroyableBuilding"))
                        {
                            // Add some functionality to destroy houses colliders and change their sprites
                        }
                        _phases.StartPhase2();// move up ^
                        break;
                    }
                }
                _collided = true;
                yield return new WaitForSeconds(0.1f);
                break;
            }
        }
    }

    protected override void Damage(Health damageTarget)
    {
        damageTarget.Hurt(_damage);

        Vector2 direction = (damageTarget.GetComponent<Transform>().position - _transform.position).normalized;
        damageTarget.GetComponent<Discarding>().Discard(direction, 3f, 15);
    }
}
