using System;
using UnityEngine;

public class FightNoticeObject : ScriptableObject
{
    public event Action OnFightBegan;

    public void Notice()
    {
        OnFightBegan?.Invoke();
    }
}
