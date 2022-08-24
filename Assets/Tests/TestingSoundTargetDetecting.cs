using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestingSoundTargetDetecting
{

    private EnemyBaseAI EnemySetup()
    {
        GameObject enemy = Resources.Load<GameObject>("Enemy");
        enemy = MonoBehaviour.Instantiate(enemy);
        EnemyBaseAI _movementAI = enemy.GetComponent<EnemyBaseAI>();
        _movementAI.SetWaitTime(0.2f);

        return _movementAI;
    }

    private PunchSound SoundSetup()
    {
        GameObject newGameObject = MonoBehaviour.Instantiate(new GameObject());
        newGameObject.AddComponent(typeof(PunchSound));
        newGameObject.GetComponent<PunchSound>().SetSoundRadious(2);

        return newGameObject.GetComponent<PunchSound>();
    }

    private GameObject GetPathfinding()
    {
        return MonoBehaviour.Instantiate(Resources.Load<GameObject>("Pathfinding"));
    }

    [UnityTest]
    public IEnumerator TestingSoundDetecting()
    {
        EnemyBaseAI enemy = EnemySetup();
        PunchSound sound = SoundSetup();
        GameObject pathfinding = GetPathfinding();
        yield return null;

        enemy.transform.position = Vector3.zero;
        sound.Noise(Vector3.right);
        yield return new WaitForSeconds(0.22f);

        Vector3 enemyPosition = enemy.transform.position;
        MonoBehaviour.Destroy(enemy.gameObject);
        MonoBehaviour.Destroy(sound.gameObject);
        MonoBehaviour.Destroy(pathfinding);

        Assert.IsTrue(enemyPosition != Vector3.zero);
    }

    [UnityTest]
    public IEnumerator TestingGoBackAfterSound()
    {
        MonoBehaviour.Instantiate(new GameObject()).AddComponent<Camera>().transform.position = new Vector3(0, 0, -10);

        EnemyBaseAI enemy = EnemySetup();
        PunchSound sound = SoundSetup();
        GameObject pathfinding = GetPathfinding();
        yield return null;

        enemy.transform.position = Vector3.zero;
        sound.Noise(Vector3.right);
        yield return new WaitForSeconds(0.22f);

        Vector3 reachedPosition = enemy.transform.position;
        yield return new WaitForSeconds(1f);

        Vector3 enemyPosition = enemy.transform.position;
        MonoBehaviour.Destroy(enemy.gameObject);
        MonoBehaviour.Destroy(sound.gameObject);
        MonoBehaviour.Destroy(pathfinding);

        Assert.AreNotEqual(enemyPosition, reachedPosition);
    }
}
