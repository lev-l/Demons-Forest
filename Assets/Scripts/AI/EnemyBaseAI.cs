using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBaseAI : EnemyTools
{
    public float Speed;
    public float AttackDistance;
    [SerializeField] private float _frequencyOfPathFinding;
    [SerializeField] private float _waitTime;
    [SerializeField] private float _peekNextWaypointDistance = 2f;
    private Transform _transform;
    private Transform _target;
    private PlayerObject _player;
    private Vector2 _startPosition;
    private int _currentWaypoint = 0;
    private Path _path;
    private bool _seePlayer = false;

    private void Start()
    {
        _player = Resources.Load<PlayerObject>("Player");
        
        _transform = GetComponent<Transform>();
        _startPosition = _transform.position;
    }

    private void Update()
    {
        if (_notBlocked
            && _path != null)
        {
            if (_currentWaypoint >= _path.vectorPath.Count)
            {
                _path = null;

                // it IS needed BECAUSE it prevents enemy stopping
                if (_seePlayer)
                {
                    CheckForAttack();
                }
                return;
            }

            StartGoAnimation();

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

    private void CheckForAttack()
    {
        _transform.rotation = GetNewRotation(selfPosition: _transform.position,
                                            targetPosition: _target.position);
        GameObject[] forwardObject = GetForwardObject(_transform.position, _transform.rotation);
        foreach (GameObject @object in forwardObject)
        {
            if (@object.CompareTag("Player"))
            {
                Block();
                Attack(_target);
            }
        }
    }

    public void TargetDetected(GameObject target)
    {
        StopCoroutine(nameof(WaitForTarget));
        _player.AddEnemy(gameObject);
        _target = target.GetComponent<Transform>();
        _currentWaypoint = 0;
        if (_notBlocked)
        {
            StopCoroutine(nameof(BuildingPathWhileSee));
            StartCoroutine(nameof(BuildingPathWhileSee));
        }
    }

    public void TargetDetected(Vector2 target)
    {
        if (_notBlocked && !_seePlayer)
        {
            PathToTarget(target);
            StopCoroutine(nameof(WaitForTarget));
            StartCoroutine(WaitForTarget(target));
        }
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
        _seePlayer = true;

        while (distanceToTarget < 7)
        {
            BuildPath(selfPosition: _transform.position,
                        targetPosition: _target.position,
                        callbackFunction: PathCompleted);

            if (_notBlocked
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

    private IEnumerator WaitForTarget(Vector2 trackEndPoint)
    {
        float distanceToTarget = Vector2.Distance(_transform.position, trackEndPoint);

        while (distanceToTarget > _peekNextWaypointDistance)
        {
            yield return new WaitForSeconds(0.5f);
            distanceToTarget = Vector2.Distance(_transform.position, trackEndPoint);
        }

        StopCoroutine(nameof(GoBack));
        StartCoroutine(nameof(GoBack));
    }

    private IEnumerator GoBack()
    {
        yield return new WaitForSeconds(_waitTime);
        BuildPath(selfPosition: _transform.position,
                    targetPosition: _startPosition,
                    callbackFunction: PathCompleted);
    }

    public override void Block()
    {
        base.Block();
        Unblock();
    }

    public override void Discard(Vector2 direction)
    {
        base.Discard(direction);
        Block();
    }

    private void PathToTarget(Vector2 target)
    {
        BuildPath(selfPosition: _transform.position,
                    targetPosition: target,
                    callbackFunction: PathCompleted);
    }

    public override void Unblock()
    {
        StopCoroutine(nameof(WaitForUnblock));
        StartCoroutine(nameof(WaitForUnblock));
    }

    private IEnumerator WaitForUnblock()
    {
        yield return new WaitForSeconds(_frequencyOfPathFinding);
        _notBlocked = true;
    }

    // only for tests
    public void SetWaitTime(float newWaitTime)
    {
        _waitTime = newWaitTime;
    }
}
