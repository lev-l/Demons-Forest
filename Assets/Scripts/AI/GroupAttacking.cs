using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupAttacking : MonoBehaviour
{
    private PlayerObject _player;

    private void Start()
    {
        _player = Resources.Load<PlayerObject>("Player");
        Resources.Load<FightNoticeObject>("FightEvent").OnFightBegan += Begin;
    }

    public void Begin()
    {
        StopCoroutine(nameof(Surrounding));
        StartCoroutine(nameof(Surrounding));
    }

    private IEnumerator Surrounding()
    {
        while (_player.NumberEnemiesSeeYou > 0)
        {
            print(_player._enemiesSeeYou);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
