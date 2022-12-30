using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EnemyUnload : MonoBehaviour
{
    [SerializeField] private string _originScene;
    private Transform _selfTransform;
    private Transform _playerTransform;

    private void Start()
    {
        _selfTransform = GetComponent<Transform>();
        _playerTransform = FindObjectOfType<PlayerMovement>().transform;

        StartCoroutine(nameof(CheckForUnload));
    }

    private IEnumerator CheckForUnload()
    {
        while (true)
        {
            if(Vector2.Distance(_selfTransform.position, _playerTransform.position) > 40)
            {
                bool notLoaded = true;
                for(int i = 0; i < SceneManager.sceneCount; i++)
                {
                    notLoaded = _originScene != SceneManager.GetSceneAt(i).name;
                    if (!notLoaded)
                    {
                        break;
                    }
                }

                if (notLoaded)
                {
                    Destroy(gameObject);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
