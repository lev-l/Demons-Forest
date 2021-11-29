using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker), typeof(EnemyAnimations), typeof(RotateToTarget)),
    RequireComponent(typeof(EnemyAttack))]
public class EnemyTools : MonoBehaviour
{
    private Seeker _seeker;
    private EnemyAnimations _animations;
    private RotateToTarget _rotating;
    private EnemyAttack _attacking;

    private void Awake()
    {
        _seeker = GetComponent<Seeker>();
        _animations = GetComponent<EnemyAnimations>();
        _rotating = GetComponent<RotateToTarget>();
        _attacking = GetComponent<EnemyAttack>();
    }

    protected void BuildPath(Vector2 selfPosition, Vector2 targetPosition,
                                OnPathDelegate callbackFunction)
    {
        _seeker.StartPath(selfPosition, targetPosition, callbackFunction);
    }

    protected Quaternion GetNewRotation(Vector2 selfPosition, Vector2 targetPosition,
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

    protected void Attack()
    {
        _attacking.Attack();
    }
}
