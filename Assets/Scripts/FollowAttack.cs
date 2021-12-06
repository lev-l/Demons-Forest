using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FollowAttack : EnemyTools
{
    public float Speed;
    public float AttackDistance;
    public event Action OnBlocked;
    [SerializeField] private float _frequencyOfPathFinding;
    [SerializeField] private float _peekNextWaypointDistance = 2f;
    private Transform _transform;
    private Transform _target;
    private int _currentWaypoint = 0;
    private Path _path;
    private bool _notBlocked = true;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        OnBlocked += GetComponent<EnemyAttack>().Stop;
    }
        
    private void Update()
    {
        if (_notBlocked
            && _path != null)
        {
            if (_currentWaypoint >= _path.vectorPath.Count)
            {
                _path = null;
                return;
            }

            StartGoAnimation();
            Vector2 direction = (_path.vectorPath[_currentWaypoint] - _transform.position).normalized;

            _transform.rotation = GetNewRotation(selfPosition: _transform.position,
                                            targetPosition: _path.vectorPath[_currentWaypoint]);
            _transform.Translate(Vector2.right * Speed * Time.deltaTime);

            float distanceToNextWaypoint
                = Vector2.Distance(_transform.position, _path.vectorPath[_currentWaypoint]);
            if (distanceToNextWaypoint <= _peekNextWaypointDistance)
            {
                _currentWaypoint++;
            }
        }
        else
        {
            StopGoAnimation();
        }
    }

    public void TargetDetected(GameObject target)
    {
        _target = target.GetComponent<Transform>();
        _currentWaypoint = 0;
        StopAllCoroutines();
        StartCoroutine(BuildingPathWhileSee());
    }

    private void PathCompleted(Path path)
    {
        if (!path.error)
        {
            _path = path;
            _currentWaypoint = 0;
        }
    }

    private IEnumerator BuildingPathWhileSee()
    {
        float distanceToTarget = Vector2.Distance(_transform.position, _target.position);

        while (distanceToTarget < 11)
        {
            BuildPath(selfPosition: _transform.position,
                        targetPosition: _target.position,
                        callbackFunction: PathCompleted);
            if (distanceToTarget < AttackDistance)
            {
                _transform.rotation = GetNewRotation(selfPosition: _transform.position,
                                                    targetPosition: _target.position);
                Block();
                Attack();
            }
            yield return new WaitForSeconds(_frequencyOfPathFinding);
            distanceToTarget = Vector2.Distance(_transform.position, _target.position);
        }
    }

    public void Block()
    {
        OnBlocked?.Invoke();
        _notBlocked = false;
        StartCoroutine(Unblock());
    }

    private IEnumerator Unblock()
    {
        yield return new WaitForSeconds(_frequencyOfPathFinding);
        _notBlocked = true;
    }
}
