using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Discarding))]
public class Health : MonoBehaviour
{
    [SerializeField] protected int _maxHealth;
    protected int _currentHealth;

    public virtual (int current, int max) GetHealthParams()
    {
        return (_currentHealth, _maxHealth);
    }

    protected virtual void Start()
    {
        _currentHealth = _maxHealth;
    }

    public virtual void Hurt(int damage)
    {
        _currentHealth -= damage;
        if(_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public virtual void Heal(int restorePoints)
    {
        _currentHealth += restorePoints;
        if(_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }
}
