using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discarding : MonoBehaviour
{
    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    public void Discard(Vector2 direction, float distance, float speed)
    {
        StopAllCoroutines();
        StartCoroutine(DiscardProcess(direction, distance, speed));
    }

    private IEnumerator DiscardProcess(Vector2 direction, float distance, float speed)
    {
        float distanceCompleted = 0;
        while(distanceCompleted < distance)
        {
            Vector2 translation = direction * speed * Time.deltaTime;
            distanceCompleted += translation.magnitude;
            _transform.Translate(translation, Space.World);
            yield return new WaitForEndOfFrame();
        }
    }
}
