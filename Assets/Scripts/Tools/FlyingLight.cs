using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingLight : MonoBehaviour
{
    public float TimeToNextDirection;
    public float Speed;
    private Transform _transform;
    private Vector2[] _directions;
    private Vector2 _currentDirection;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _directions = new Vector2[8];

        _directions[0] = Vector2.up + (Vector2)_transform.position;
        _directions[1] = Vector2.left + (Vector2)_transform.position;
        _directions[2] = Vector2.down + (Vector2)_transform.position;
        _directions[3] = Vector2.right + (Vector2)_transform.position;
        _directions[4] = (Vector2.up + Vector2.right).normalized + (Vector2)_transform.position;
        _directions[5] = (Vector2.up + Vector2.left).normalized + (Vector2)_transform.position;
        _directions[6] = (Vector2.down + Vector2.right).normalized + (Vector2)_transform.position;
        _directions[7] = (Vector2.down + Vector2.left).normalized + (Vector2)_transform.position;

        StartCoroutine(nameof(ChooseNextDirection));
    }

    private void Update()
    {
        Vector2 move = (_currentDirection - (Vector2)_transform.position).normalized * Time.deltaTime * Speed;
        _transform.Translate(move);
    }

    private IEnumerator ChooseNextDirection()
    {
        while (true)
        {
            _currentDirection = _directions[Random.Range(0, 8)];
            yield return new WaitForSeconds(TimeToNextDirection);
        }
    }
}
