using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GoblinVillageBossAttack))]
public class GoblinVillageBossAI : EnemyBaseAI
{
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

    protected override IEnumerator WaitForUnblock()
    {
        yield return new WaitForSeconds(1.7f);
        NotBlocked = true;
    }
}
