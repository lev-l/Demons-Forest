using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathSound : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(nameof(SelfDestroy));
    }

    private IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
