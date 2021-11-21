using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker), typeof(RotateToTarget))]
public class FollowAttack : MonoBehaviour
{
    public float Speed;
    public float FrequencyOfPathFinding;
    public float PeekNextWaypointDistance = 2f;
    private Transform _transform;
    private Seeker _seeker;
    private RotateToTarget _rotating;
    private Transform _target;
    private int _currentWaypoint = 0;
    private bool _endReached = true;
    private Path _path;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _seeker = GetComponent<Seeker>();
        _rotating = GetComponent<RotateToTarget>();
    }

    private void Update()
    {
        if (_path != null)
        {
            if (_currentWaypoint >= _path.vectorPath.Count)
            {
                _endReached = true;
                return;
            }
            else
            {
                _endReached = false;
            }

            Vector2 direction = (_path.vectorPath[_currentWaypoint] - _transform.position).normalized;

            _transform.rotation = Quaternion.Euler(Vector3.forward
                                                    * _rotating.Rotate(
                                                                _transform.position,
                                                                _path.vectorPath[_currentWaypoint]
                                                                )
                                                    );
            _transform.Translate(Vector2.right * Speed * Time.deltaTime);

            float distanceToNextWaypoint
                = Vector2.Distance(_transform.position, _path.vectorPath[_currentWaypoint]);
            if (distanceToNextWaypoint <= PeekNextWaypointDistance)
            {
                _currentWaypoint++;
            }
        }
    }

    public void TargetDetected(GameObject target)
    {
        _target = target.GetComponent<Transform>();
        _currentWaypoint = 0;
        _endReached = false;
        StopAllCoroutines();
        StartCoroutine(BuildingPathWhileSee());
    }

    private void BuildPath()
    {
        _seeker.StartPath(_transform.position, _target.position, PathCompleted);
    }

    private void PathCompleted(Path path)
    {
        if (!path.error)
        {
            _path = path;
        }
    }

    private IEnumerator BuildingPathWhileSee()
    {
        float distanceToTarget = Vector2.Distance(_transform.position, _target.position);

        while (distanceToTarget < 11)
        {
            print("i buildiing");
            BuildPath();
            yield return new WaitForSeconds(FrequencyOfPathFinding);
            distanceToTarget = Vector2.Distance(_transform.position, _target.position);
        }
    }
}
