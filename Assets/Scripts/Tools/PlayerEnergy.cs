﻿using System;
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

        StartRegenertaion();
    }

    public bool UseEnergy(int amount)
    {
        if(_player.Energy == 0)
        {
            return false;
        }

        bool exhaust = _player.ConsumeEnergy(amount);
        _energyPresenter.UpdateView(_player.Energy, _player.MaxEnergy);

        if (exhaust)
        {
            StopCoroutine(nameof(EnergyRegeneration));
            Invoke(nameof(StartRegenertaion), _regenerationCooldown);
            _energyPresenter.Exhaust();
        }

        return true;
    }

    public void ReturnEnergy(int amount)
    {
        _player.AddEnergy(amount);
        _energyPresenter.UpdateView(_player.Energy, _player.MaxEnergy);
    }

    private IEnumerator EnergyRegeneration()
    {
        while (true)
        {
            _player.AddEnergy(_energyRegenerationRate);
            _energyPresenter.UpdateView(_player.Energy, _player.MaxEnergy);
            yield return new WaitForSeconds(_regenerationDelay);
        }
    }

    private void StartRegenertaion()
    {
        _energyPresenter.ResumeRegeneration();
        StartCoroutine(nameof(EnergyRegeneration));
    }
}
