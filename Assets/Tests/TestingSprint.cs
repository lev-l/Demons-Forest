using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestingSprint
{
    private EnemyBaseAI EnemySetup()
    {
        GameObject enemy = Resources.Load<GameObject>("Enemy");
        enemy.GetComponent<EnemySprint>().Exhaust();
        enemy = MonoBehaviour.Instantiate(enemy);
        EnemyBaseAI _movementAI = enemy.GetComponent<EnemyBaseAI>();

        return _movementAI;
    }

    [UnityTest]
    public IEnumerator TestSprintSpeedIncresing()
    {
        EnemyBaseAI _movementAI = EnemySetup();
        yield return null;

        float speed = _movementAI.Speed;
        MonoBehaviour.Destroy(_movementAI.gameObject);

        Assert.IsTrue(speed == 4.5f * 1.8f);
    }

    [UnityTest]
    public IEnumerator TestSprintSpeedDecresing()
    {
        EnemyBaseAI _movementAI = EnemySetup();
        yield return new WaitForSeconds(0.3f);

        float speed = _movementAI.Speed;
        MonoBehaviour.Destroy(_movementAI.gameObject);

        Assert.IsTrue(speed == 4.5f);
    }
}
