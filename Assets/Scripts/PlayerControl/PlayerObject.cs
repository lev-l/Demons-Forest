using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Player")]
public class PlayerObject : ScriptableObject
{
    public int NumberEnemiesSeeYou { get; private set; }
    public int Health;
    public event Action<bool> OnStealthChanged;
    public event Action OnZeroEnemies;
    public List<GameObject> _enemiesSeeYou { get; private set; }
    [SerializeField] private int _energyMax;
    [SerializeField] private FightNoticeObject _fightEvent;
    private int _energy;
    private bool _stealthMode;
    private bool _freeRotation;

    public int MaxEnergy => _energyMax;
    public int Energy => _energy;
    public bool StealthMode => _stealthMode;
    public bool FreeRotation => _freeRotation;

    private void OnEnable()
    {
        _energy = _energyMax;
        _enemiesSeeYou = new List<GameObject>();
        _fightEvent = Resources.Load<FightNoticeObject>("FightEvent");
        _freeRotation = true;

        _fightEvent.OnFightBegan += ChangeStealthMode;
    }

    public void AddEnemy(GameObject enemy)
    {
        if (!_enemiesSeeYou.Contains(enemy))
        {
            _enemiesSeeYou.Add(enemy);
            NumberEnemiesSeeYou++;
            enemy.GetComponent<Health>().WhenDestroy += DeleteEnemy;

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
            enemy.GetComponent<Health>().WhenDestroy -= DeleteEnemy;

            if(NumberEnemiesSeeYou == 0)
            {
                OnZeroEnemies?.Invoke();
            }
            if(NumberEnemiesSeeYou < 0)
            {
                Debug.LogAssertion("Somehow when deleting enemies we got a minus.");
            }
        }
    }

    public void ChangeStealthMode()
    {
        if (NumberEnemiesSeeYou == 0)
        {
            _stealthMode = !StealthMode;
        }
        else
        {
            _stealthMode = false;
        }
        OnStealthChanged?.Invoke(_stealthMode);
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

    public void BlockRotation()
    {
        _freeRotation = false;
    }

    public void ReleaseRotation()
    {
        _freeRotation = true;
    }

    public void Death(GameObject player)
    {
        NumberEnemiesSeeYou = 0;
        _stealthMode = false;
        Health = 100;
        _energy = _energyMax;
        _enemiesSeeYou.Clear();

        PlayerPrefs.SetString("FileToLoad", "MainSave");
        File.Delete(Application.dataPath + "/MainSave.add");
        File.Delete(Application.dataPath + "/ChestsSave.add");
        File.Delete(Application.dataPath + "/VillageBoss.add");
        SceneManager.LoadScene((int)Scenes.Game);
    }
}
