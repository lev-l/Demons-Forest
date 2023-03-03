using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBaseAI : EnemyTools
{
    #region Properties
    public float Speed;
    public float AttackDistance;
    [SerializeField] protected float _frequencyOfPathFinding;
    [SerializeField] protected float _waitTime;
    [SerializeField] protected float _peekNextWaypointDistance = 2f;
    protected Transform _transform;
    protected Transform _target;
    protected GroupAttacking _group;
    protected PlayerObject _player;
    protected Vector2 _startPosition;
    protected int _currentWaypoint = 0;
    protected Path _path;
    protected bool _seePlayer = false;
    #endregion
    #region Events
    public event Action<Vector2> OnTargetDetected;
    #endregion

    protected virtual void Start()
    {
        _player = Resources.Load<PlayerObject>("Player");
        _group = FindObjectOfType<GroupAttacking>();
        
        _transform = GetComponent<Transform>();
        _startPosition = _transform.position;
    }

    protected void Update()
    {
        if (NotBlocked
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

    protected void CheckForAttack()
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

        if (NotBlocked)
        {
            StopCoroutine(nameof(BuildingPathWhileSee));
            StartCoroutine(nameof(BuildingPathWhileSee));
        }
    }

    public void TargetDetected(Vector2 target)
    {
        OnTargetDetected?.Invoke(target);

        if (NotBlocked && !_seePlayer)
        {
            PathToTarget(target);
            StopCoroutine(nameof(WaitForTarget));
            StartCoroutine(WaitForTarget(target));
        }
    }

    protected void PathCompleted(Path path)
    {
        if (!path.error)
        {
            _path = path;
            _currentWaypoint = 0;
        }
    }

    protected virtual IEnumerator BuildingPathWhileSee()
    {
        float distanceToTarget = Vector2.Distance(_transform.position, _target.position);
        _seePlayer = true;

        while (distanceToTarget < 8)
        {
            Vector2 destination = _group.GetDestination(_player._enemiesSeeYou.IndexOf(gameObject),
                                                        _target,
                                                        AttackDistance);

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

    protected IEnumerator WaitForTarget(Vector2 trackEndPoint)
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

    protected IEnumerator GoBack()
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

    protected void PathToTarget(Vector2 target)
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

    protected virtual IEnumerator WaitForUnblock()
    {
        yield return new WaitForSeconds(_frequencyOfPathFinding);
        NotBlocked = true;
    }

    // only for tests
    public void SetWaitTime(float newWaitTime)
    {
        _waitTime = newWaitTime;
    }
}
