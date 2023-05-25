using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPreparedObserver : MonoBehaviour
{
    private PlayerAttack _player;

    private void Start()
    {
        _player = GetComponentInParent<PlayerAttack>();
    }

    public void NoticeAttack()
    {
        _player.AttackPrepared();
    }

    public void NoticeStab()
    {
        _player.StabPrepared();
    }
}
