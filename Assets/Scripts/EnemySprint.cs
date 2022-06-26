using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyBaseAI))]
public class EnemySprint : MonoBehaviour
{
    [SerializeField] private float _maxEnergy;
    [SerializeField] private float _speedIncrease;
    private float _currentEnergy;
    private EnemyBaseAI _movementAI;
    private float _standartSpeed;

    private void Awake()
    {
        _currentEnergy = _maxEnergy;
        _movementAI = GetComponent<EnemyBaseAI>();
        _standartSpeed = _movementAI.Speed;
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(SprintControling));
    }

    private IEnumerator SprintControling()
    {
        print("Max energy is: " + _maxEnergy);
        print("OK, I started!");
        while (true)
        {
            print("So current energy is: " + _currentEnergy);
            if (_currentEnergy >= _maxEnergy)
            {
                print("Energy filled up!");
                _currentEnergy = _maxEnergy;
                yield return Sprinting();
            }

            _currentEnergy++;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator Sprinting()
    {
        _movementAI.Speed *= _speedIncrease;
        print("Speed increased to: " + _movementAI.Speed);
        while (_currentEnergy >= 0)
        {
            print("Energy in sprinting is now: " + _currentEnergy);
            _currentEnergy--;
            yield return new WaitForSeconds(0.1f);
        }

        _movementAI.Speed = _standartSpeed;
        print("Speed returned to: " + _movementAI.Speed);
    }

    //for tests
    public void Exhaust()
    {
        _currentEnergy = _maxEnergy = 1;
    }
}
