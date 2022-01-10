using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Observing), typeof(EnemyBaseAI))]
public class ObservingListeners : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Observing>().SeePlayer += GetComponent<EnemyBaseAI>().TargetDetected;
    }
}
