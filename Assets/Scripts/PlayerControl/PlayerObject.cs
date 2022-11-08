using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player")]
public class PlayerObject : ScriptableObject
{
    public int NumberEnemiesSeeYou { get; private set; }
    public int Health;
    public List<GameObject> _enemiesSeeYou { get; private set; }
    [SerializeField] private int _energyMax;
    [SerializeField] private FightNoticeObject _fightEvent;
    private int _energy;
    private bool _stealthMode;

    public int MaxEnergy => _energyMax;
    public int Energy => _energy;
    public bool StealthMode => _stealthMode;

    private void OnEnable()
    {
        _energy = _energyMax;
        _enemiesSeeYou = new List<GameObject>();
        _fightEvent = Resources.Load<FightNoticeObject>("FightEvent");

        _fightEvent.OnFightBegan += ChangeStealthMode;
    }

    public void AddEnemy(GameObject enemy)
    {
        if (!_enemiesSeeYou.Contains(enemy))
        {
            _enemiesSeeYou.Add(enemy);
            NumberEnemiesSeeYou++;
            enemy.GetComponent<Health>().OnDeath += DeleteEnemy;

            if (NumberEnemiesSeeYou == 1)
            {
                _fightEvent.Notice();
            }
        }
    }

    public void DeleteEnemy(GameObject enemy)
    {
        if (NumberEnemiesSeeYou > 0
            && _enemiesSeeYou.Contains(enemy))
        {
            _enemiesSeeYou.Remove(enemy);
            NumberEnemiesSeeYou--;
            enemy.GetComponent<Health>().OnDeath -= DeleteEnemy;
        }
    }

    public void ChangeStealthMode()
    {
        if(NumberEnemiesSeeYou == 0)
        {
            _stealthMode = !StealthMode;
        }
        else
        {
            _stealthMode = false;
        }
    }

    // returns if the energy level reached zero
    public bool ConsumeEnergy(int amount)
    {
        if((_energy -= amount) <= 0)
        {
            _energy = 0;
            return true;
        }

        return false;
    }

    // returns if the energy level reached maximum
    public bool AddEnergy(int amount)
    {
        if((_energy += amount) >= _energyMax)
        {
            _energy = _energyMax;
            return true;
        }

        return false;
    }

    public void Death(GameObject player)
    {
        NumberEnemiesSeeYou = 0;
        _stealthMode = false;
        Health = 100;
        _energy = _energyMax;
        _enemiesSeeYou.Clear();
    }
}
