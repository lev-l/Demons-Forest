using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Observing), typeof(FollowAttack))]
public class ObservingListeners : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Observing>().SeePlayer += GetComponent<FollowAttack>().TargetDetected;
    }
}
