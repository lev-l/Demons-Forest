using System.Collections;
using UnityEngine;

public class PlayerHealth : Health
{
    private PlayerHealthPresenter _presenter;
    private PlayerObject _player;

    protected override void Awake()
    {
        base.Awake();

        _player = Resources.Load<PlayerObject>("Player");
        _player.Health = _maxHealth;
        _presenter = FindObjectOfType<PlayerHealthPresenter>();
        
        OnDeath += _player.Death;
    }

    public override void Hurt(int damage)
    {
        base.Hurt(damage);
        _player.Health = _currentHealth;
        _presenter.UpdateView(this);
    }

    public override void Heal(int restorePoints)
    {
        base.Heal(restorePoints);
        _player.Health = _currentHealth;
        _presenter.UpdateView(this);
    }

    public void SetHealth(int health)
    {
        _currentHealth = health;
        _player.Health = health;
        _presenter.UpdateView(this);
    }

    public IEnumerator SetWithDelay(int health)
    {
        yield return null;
        _currentHealth = health;
        _player.Health = health;
        _presenter.UpdateView(this);
    }
}
