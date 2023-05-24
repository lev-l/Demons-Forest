using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Discarding))]
public class Health : MonoBehaviour
{
    public event Action OnHurt;
    public event Action<GameObject> OnDeath;
    public event Action<GameObject> WhenDestroy;
    [SerializeField] protected int _maxHealth;
    protected int _currentHealth;
    private PunchSound _hurtedSound;

    public virtual (int current, int max) GetHealthParams()
    {
        return (_currentHealth, _maxHealth);
    }

    protected virtual void Awake()
    {
        _currentHealth = _maxHealth;
        _hurtedSound = FindObjectOfType<PunchSound>();
    }

    public virtual void Hurt(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            OnDeath?.Invoke(gameObject);
            Destroy(gameObject);
            return;
        }

        OnHurt?.Invoke();
        _hurtedSound.Noise(transform.position);
    }

    public virtual void Heal(int restorePoints)
    {
        _currentHealth += restorePoints;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }

    private void OnDestroy()
    {
        WhenDestroy?.Invoke(gameObject);
    }
}
