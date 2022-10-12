using System.Collections.Generic;
using UnityEngine;

public class GroupForm : MonoBehaviour
{
    private GroupVision _vision;
    private List<EnemyBaseAI> _group;

    private void Start()
    {
        _vision = GetComponent<GroupVision>();
        _group = _vision.GetGroup;

        foreach(EnemyBaseAI memeber in _group)
        {
            memeber.OnTargetDetected += _vision.Notify;
            memeber.GetComponent<Health>().OnDeath += _vision.DeleteMember;
        }
    }
}
