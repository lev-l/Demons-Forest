using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBaseAI : EnemyTools
{
    public float Speed;
    public float AttackDistance;
    [SerializeField] private float _frequencyOfAttackChecking;
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
                _player.DeleteEnemy(gameObject);
                StartCoroutine(nameof(GoBack));
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

    public void TargetDetected(Transform target)
    {
        _player.DeleteEnemy(gameObject);
        _player.AddEnemy(gameObject);
        _target = target;
        _currentWaypoint = 0;
        StopCoroutine(nameof(BuildingPathWhileSee));
        StartCoroutine(nameof(BuildingPathWhileSee));
    }

    private void PathCompleted(Path path)
    {
        if (!path.error)
        {
            _path = path;
            _currentWaypoint = 0;
        }
    }

    private IEnumerator CheckForAttackDistance()
    {
        float distanceToTarget = Vector2.Distance(_transform.position, _target.position);

        while (distanceToTarget < 10)
        {
            if (_notBlocked
                    && distanceToTarget < AttackDistance)
            {
                _transform.rotation = GetNewRotation(selfPosition: _transform.position,
                                                    targetPosition: _target.position);
                Block();
                Attack();
            }
            yield return new WaitForSeconds(_frequencyOfAttackChecking);
            distanceToTarget = Vector2.Distance(_transform.position, _target.position);
        }

    }

    public void Block()
    {
        _notBlocked = false;
        StopCoroutine(nameof(Unblock));
        StartCoroutine(nameof(Unblock));
    }

    private IEnumerator Unblock()
    {
        yield return new WaitForSeconds(_frequencyOfAttackChecking);
        _notBlocked = true;
    }

    private IEnumerator GoBack()
    {
        yield return new WaitForSeconds(_waitTime);
        BuildPath(selfPosition: _transform.position,
                    targetPosition: _startPosition,
                    callbackFunction: PathCompleted);
    }

    public override void Discard(Vector2 direction)
    {
        base.Discard(direction);
        Block();
    }
}
