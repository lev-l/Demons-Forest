using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EnemyUnload : MonoBehaviour
{
    public string Origin;
    private Scene _originScene;
    private Transform _selfTransform;
    private Transform _playerTransform;

    private void Start()
    {
        _originScene = SceneManager.GetSceneByName(Origin);
        _selfTransform = GetComponent<Transform>();
        _playerTransform = FindObjectOfType<PlayerMovement>().transform;

        StartCoroutine(nameof(CheckForUnload));
    }

    private IEnumerator CheckForUnload()
    {
        while (true)
        {
            if (Vector2.Distance(_selfTransform.position, _playerTransform.position) > 40
                && !_originScene.isLoaded)
            {
                Destroy(gameObject);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
