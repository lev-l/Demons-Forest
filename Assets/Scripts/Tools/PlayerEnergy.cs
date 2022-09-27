using System;
using System.Collections;
using UnityEngine;

class PlayerEnergy : MonoBehaviour
{
    [SerializeField] private int _energyRegenerationRate;
    [SerializeField] private float _regenerationCooldown;
    [SerializeField] private float _regenerationDelay;
    private PlayerObject _player;
    private PlayerEnergyPresenter _energyPresenter;

    private void Start()
    {
        _player = Resources.Load<PlayerObject>("Player");
        _energyPresenter = FindObjectOfType<PlayerEnergyPresenter>();
    }

    public bool UseEnergy(int amount)
    {
        if(_player.Energy == 0)
        {
            return false;
        }

        bool exhaust = _player.ConsumeEnergy(amount);
        _energyPresenter.UpdateView();

        if (exhaust)
        {
            StopCoroutine(nameof(EnergyRegeneration));
            Invoke(nameof(StartRegenertaion), _regenerationCooldown);
            _energyPresenter.Exhaust();
        }

        return true;
    }

    private IEnumerator EnergyRegeneration()
    {
        while (true)
        {
            if (_player.AddEnergy(_energyRegenerationRate))
            {
                _energyPresenter.HideBar();
            }
            _energyPresenter.UpdateView();
            yield return new WaitForSeconds(_regenerationDelay);
        }
    }

    private void StartRegenertaion()
    {
        StartCoroutine(EnergyRegeneration());
    }
}
