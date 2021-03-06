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
    private bool _notBlocked = true;

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
        _player.AddEnemy(gameObject);
        _target = target.GetComponent<Transform>();
        _currentWaypoint = 0;
        if (_notBlocked)
        {
            StopCoroutine(nameof(BuildingPathWhileSee));
            StartCoroutine(nameof(BuildingPathWhileSee));
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

        while (distanceToTarget < 11)
        {
            print("s");
            BuildPath(selfPosition: _transform.position,
                        targetPosition: _target.position,
                        callbackFunction: PathCompleted);
            if (_notBlocked
                && distanceToTarget < AttackDistance)
            {
                _transform.rotation = GetNewRotation(selfPosition: _transform.position,
                                                    targetPosition: _target.position);
                Block();
                Attack();
            }
            yield return new WaitForSeconds(_frequencyOfPathFinding);
            distanceToTarget = Vector2.Distance(_transform.position, _target.position);
        }
        _player.DeleteEnemy(gameObject);
        yield return new WaitForSeconds(_waitTime);
        BuildPath(selfPosition: _transform.position,
                    targetPosition: _startPosition,
                    callbackFunction: PathCompleted);
    }

    public void Block()
    {
        _notBlocked = false;
        StopCoroutine(nameof(Unblock));
        StartCoroutine(nameof(Unblock));
    }

    private IEnumerator Unblock()
    {
        yield return new WaitForSeconds(_frequencyOfPathFinding);
        _notBlocked = true;
    }

    public override void Discard(Vector2 direction)
    {
        base.Discard(direction);
        Block();
    }
}
