using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupVision : MonoBehaviour
{
    private List<EnemyBaseAI> _group;
    private bool _open;

    public List<EnemyBaseAI> GetGroup => _group;

    private void Awake()
    {
        _group = new List<EnemyBaseAI>();
        _group.AddRange(GetComponentsInChildren<EnemyBaseAI>());

        _open = true;
    }

    public void Notify(GameObject target)
    {
        if (_open)
        {
            _open = false;
            
            foreach (EnemyBaseAI member in _group)
            {
                member.TargetDetected(target);
            }

            _open = true;
        }
    }
}
