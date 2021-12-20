using System.Collections;
using UnityEngine;

public class PlayerHealth : Health
{
    private PlayerHealthPresenter _presenter;

    protected override void Start()
    {
        base.Start();
        _presenter = FindObjectOfType<PlayerHealthPresenter>();
    }

    public override void Hurt(int damage)
    {
        base.Hurt(damage);
        _presenter.UpdateView(this);
    }

    public override void Heal(int restorePoints)
    {
        base.Heal(restorePoints);
        _presenter.UpdateView(this);
    }
}
