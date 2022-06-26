using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player")]
public class PlayerObject : ScriptableObject
{
    public int NumberEnemiesSeeYou { get; private set; }
    public float Health;
    public bool StealthMode;
    private List<GameObject> _enemiesSeeYou = new List<GameObject>();

    public void AddEnemy(GameObject enemy)
    {
        if (!_enemiesSeeYou.Contains(enemy))
        {
            _enemiesSeeYou.Add(enemy);
            NumberEnemiesSeeYou++;
        }
    }

    public void DeleteEnemy(GameObject enemy)
    {
        if(NumberEnemiesSeeYou > 0
            && _enemiesSeeYou.Count > 0)
        _enemiesSeeYou.Remove(enemy);
        NumberEnemiesSeeYou--;
    }
}
