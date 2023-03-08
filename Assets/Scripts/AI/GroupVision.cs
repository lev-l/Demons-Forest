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

    public void Notify(Vector2 target)
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

    public EnemyBaseAI[] AddNewMember()
    {
        EnemyBaseAI[] allMembers = GetComponentsInChildren<EnemyBaseAI>();
        List<EnemyBaseAI> newMembers = new List<EnemyBaseAI>();
        foreach(EnemyBaseAI member in allMembers)
        {
            if (!_group.Contains(member))
            {
                _group.Add(member);
                newMembers.Add(member);
            }
        }

        return newMembers.ToArray();
    }

    public void DeleteMember(GameObject member)
    {
        _group.Remove(member.GetComponent<EnemyBaseAI>());

        if(_group.Count == 0)
        {
            Destroy(gameObject);
        }
    }
}
