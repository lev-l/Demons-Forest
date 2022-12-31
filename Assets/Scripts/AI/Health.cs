using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Discarding))]
public class Health : MonoBehaviour
{
    public event Action<GameObject> OnDeath;
    [SerializeField] protected int _maxHealth;
    protected int _currentHealth;
    private PunchSound _hurtedSound;

    public virtual (int current, int max) GetHealthParams()
    {
        return (_currentHealth, _maxHealth);
    }

    protected virtual void Start()
    {
        _currentHealth = _maxHealth;
        _hurtedSound = FindObjectOfType<PunchSound>();
    }

    public virtual void Hurt(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }

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
        OnDeath?.Invoke(gameObject);
    }
}
