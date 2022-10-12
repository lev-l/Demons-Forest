using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupVision : MonoBehaviour
{
    private List<EnemyBaseAI> _group;

    public List<EnemyBaseAI> GetGroup => _group;

    private void Start()
    {
        _group = new List<EnemyBaseAI>();
        _group.AddRange(GetComponentsInChildren<EnemyBaseAI>());
    }

    public void Notify(GameObject target)
    {
        foreach(EnemyBaseAI member in _group)
        {
            member.TargetDetected(target);
        }
    }
}
