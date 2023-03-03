using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GoblinVillageBossAttack), typeof(GoblinVillageBossPhases))]
public class GoblinVillageBossAI : EnemyBaseAI
{
    protected override IEnumerator BuildingPathWhileSee()
    {
        float distanceToTarget = Vector2.Distance(_transform.position, _target.position);
        _seePlayer = true;

        while (distanceToTarget < 15)
        {
            Vector2 destination = _target.position;

            BuildPath(selfPosition: _transform.position,
                        targetPosition: destination,
                        callbackFunction: PathCompleted);

            if (NotBlocked
                && distanceToTarget < AttackDistance)
            {
                CheckForAttack();
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
