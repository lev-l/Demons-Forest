using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discarding : MonoBehaviour
{
    [SerializeField] private float _discardSpeed;
    [SerializeField] private float _distance;
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
        float distanceCompleted = 0;
        while(distanceCompleted < _distance)
        {
            Vector2 translation = direction * _discardSpeed * Time.deltaTime;
            distanceCompleted += translation.magnitude;
            _transform.Translate(translation, Space.World);
            yield return new WaitForEndOfFrame();
        }
    }
}
