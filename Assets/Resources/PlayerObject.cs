using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player")]
public class PlayerObject : ScriptableObject
{
    public float Health;
    public bool StealthMode;
    public bool InFight;
}
