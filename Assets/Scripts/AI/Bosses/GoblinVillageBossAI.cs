using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GoblinVillageBossAttack))]
public class GoblinVillageBossAI : EnemyBaseAI
{
    public GameObject StandingBoss;
    public GameObject FallenBoss;
    protected override IEnumerator BuildingPathWhileSee()
    {
        float distanceToTarget = Vector2.Distance(_transform.position, _target.position);
        _seePlayer = true;

        while (distanceToTarget < 22)
        {
            if (NotBlocked)
            {
                Vector2 destination = _target.position;

                BuildPath(selfPosition: _transform.position,
                            targetPosition: destination,
                            callbackFunction: PathCompleted);

                if (distanceToTarget < AttackDistance)
                {
                    CheckForAttack();
                }
            }
            yield return new WaitForSeconds(_frequencyOfPathFinding);
            distanceToTarget = Vector2.Distance(_transform.position, _target.position);
        }

        _seePlayer = false;
        _player.DeleteEnemy(gameObject);
        StopCoroutine(nameof(GoBack));
        StartCoroutine(nameof(GoBack));
    }

    public override void Discard(Vector2 direction)
    {
    }

    public override void Unblock()
    {
        if (_currentBlocking != null)
        {
            StopCoroutine(_currentBlocking);
        }
        _currentBlocking = StartCoroutine(WaitForUnblock(2.1f));
    }

    protected override IEnumerator WaitForUnblock(float timeToWait)
    {
        yield return base.WaitForUnblock(timeToWait);
        FallenBoss.SetActive(false);
        StandingBoss.SetActive(true);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
}
