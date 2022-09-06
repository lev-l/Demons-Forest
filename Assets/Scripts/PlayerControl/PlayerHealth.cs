using System.Collections;
using UnityEngine;

public class PlayerHealth : Health
{
    private PlayerHealthPresenter _presenter;
    private PlayerObject _player;

    protected override void Start()
    {
        base.Start();
        _player = Resources.Load<PlayerObject>("Player");
        _player.Health = _maxHealth;
        _presenter = FindObjectOfType<PlayerHealthPresenter>();
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
}
