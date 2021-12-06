using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Discarding))]
public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    private int _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void Hurt(int damage)
    {
        _currentHealth -= damage;
        if(_currentHealth <= 0)
        {
            print("DIE!!!");
        }
    }

    public void Heal(int restorePoints)
    {
        _currentHealth += restorePoints;
        if(_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }
}
