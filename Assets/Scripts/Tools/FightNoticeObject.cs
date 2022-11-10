using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FightEvent", menuName = "BattleNotice")]
public class FightNoticeObject : ScriptableObject
{
    public event Action OnFightBegan;

    public void Notice()
    {
        OnFightBegan?.Invoke();
    }
}
