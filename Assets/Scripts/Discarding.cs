using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discarding : MonoBehaviour
{
    [SerializeField] private float _discardForce;
    [SerializeField] private uint _duration;
    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    public void Discard(Vector2 direction)
    {
        StopAllCoroutines();
        StartCoroutine(DiscardProcess(direction));
    }

    private IEnumerator DiscardProcess(Vector2 direction)
    {
        for (int time = 0; time < _duration; time++)
        {
            yield return new WaitForEndOfFrame();
            _transform.Translate((direction * _discardForce) * Time.deltaTime, Space.World);
        }
    }
}
