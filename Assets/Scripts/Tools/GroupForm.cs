using System.Collections.Generic;
using UnityEngine;

public class GroupForm : MonoBehaviour
{
    public string Hesh;
    public bool IsGreatEnemy;
    private GroupVision _vision;
    private List<EnemyBaseAI> _group;

    private void Start()
    {
        _vision = GetComponent<GroupVision>();
        _group = _vision.GetGroup;

        foreach(EnemyBaseAI memeber in _group)
        {
            memeber.OnTargetDetected += _vision.Notify;
            memeber.GetComponent<Health>().WhenDestroy += _vision.DeleteMember;
            memeber.GetComponent<Health>().OnDeath += Killed;
        }
    }

    public void AddMembers()
    {
        EnemyBaseAI[] newMembers = _vision.AddNewMember();
        foreach (EnemyBaseAI newMemeber in newMembers)
        {
            newMemeber.OnTargetDetected += _vision.Notify;
            newMemeber.GetComponent<Health>().WhenDestroy += _vision.DeleteMember;
            newMemeber.GetComponent<Health>().OnDeath += Killed;
        }
    }

    public void Killed(GameObject killedMember)
    {
        if(_vision.GetGroup.Count - 1 == 0)
        {
            FindObjectOfType<EnemiesSaver>().Data.AddKilled(Hesh, IsGreatEnemy);
        }
    }
}
