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

    private void Start()
    {
        _movementAI = GetComponent<EnemyBaseAI>();
        _standartSpeed = _movementAI.Speed;
        StartCoroutine(nameof(SprintControling));
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(nameof(SprintControling));
    }

    private IEnumerator SprintControling()
    {
        while (true)
        {
            if(_currentEnergy >= _maxEnergy)
            {
                _currentEnergy = _maxEnergy;
                _movementAI.Speed *= _speedIncrease;
                yield return Sprinting();
            }

            _currentEnergy++;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator Sprinting()
    {
        while(_currentEnergy >= 0)
        {
            _currentEnergy--;
            yield return new WaitForSeconds(0.1f);
        }
        _movementAI.Speed = _standartSpeed;
    }
}