using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinVillageBossPhases : MonoBehaviour
{
    public GameObject EnemyPefab;
    [SerializeField] private float _phase2Time;
    [SerializeField] private int _enemiesOnPhase2;
    private GoblinVillageBossAI _bossAI;
    private Transform _transform;

    private void Start()
    {
        _bossAI = GetComponent<GoblinVillageBossAI>();
        _transform = gameObject.transform;
    }

    public void StartPhase1()
    {

    }

    public void StartPhase2()
    {
        _bossAI.Block(_phase2Time);
        for(int i = 0; i < _enemiesOnPhase2; i++)
        {
            GameObject newEnemy = Instantiate(EnemyPefab, _transform.position, Quaternion.identity);
            newEnemy.transform.SetParent(_transform.parent);
            _transform.parent.GetComponent<GroupForm>().AddMembers();
        }
    }
}
