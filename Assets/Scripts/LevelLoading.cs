using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;

public class LevelLoading : MonoBehaviour
{
    private Transform _player;
    private AstarPath _pathfinder;
    private string[,] _levelsMap;
    private string[] _lastLoadedLevels;
    private string[] _levelsLoaded;
    private int _lastPositionX, _lastPositionY;

    private void Start()
    {
        _levelsMap = new Forest_levelsMap().GetMap();
        _lastPositionX = _lastPositionY = 0;
        _player = FindObjectOfType<PlayerMovement>().transform;
        _pathfinder = FindObjectOfType<AstarPath>();

        LoadNearLevels(PlayerPositionIndexX, PlayerPositionIndexY);
        _lastLoadedLevels = _levelsLoaded;
        _pathfinder.data.gridGraph.Scan();

        StartCoroutine(nameof(UpdateLoad));
    }

    private IEnumerator UpdateLoad()
    {
        while (true)
        {
            int positionX = PlayerPositionIndexX;
            int positionY = PlayerPositionIndexY;

            if (_lastPositionX != positionX
                || _lastPositionY != positionY)
            {
                _lastPositionX = positionX;
                _lastPositionY = positionY;

                LoadNearLevels(positionX, positionY);

                yield return new WaitForFixedUpdate();

                GridGraph graph = _pathfinder.data.gridGraph;
                Vector2 currentLevelCenter = new Vector3(20 + 40 * positionX, 20 + 40 * positionY);
                graph.RelocateNodes(currentLevelCenter, Quaternion.identity, 1);
                graph.is2D = true;

                yield return new WaitForFixedUpdate();

                graph.Scan();

                yield return null;

                UnloadLevels();

                yield return new WaitForSeconds(4);
            }

            yield return new WaitForSeconds(4);
        }
    }

    private void LoadNearLevels(int x, int y)
    {
        List<string> levelsToLoad = GetNearLevels(x, y);

        foreach(string level in levelsToLoad)
        {
            if (SceneManager.GetSceneByName(level).name == null)
            {
                SceneManager.LoadScene(level, LoadSceneMode.Additive);
            }
        }

        _lastLoadedLevels = _levelsLoaded;
        _levelsLoaded = levelsToLoad.ToArray();
    }

    private void UnloadLevels()
    {
        IEnumerable<string> levelsToUnload = _lastLoadedLevels.Except(_levelsLoaded);

        foreach(string level in levelsToUnload)
        {
            SceneManager.UnloadSceneAsync(level);
        }
    }

    private List<string> GetNearLevels(int x, int y)
    {
        List<string> levels = new List<string>();

        for(int pX = x - 1; pX <= x + 1; pX++)
        {
            if (pX >= 0
                && pX < _levelsMap.GetLength(0))
            {
                for (int pY = y - 1; pY <= y + 1; pY++)
                {
                    if(pY >= 0
                        && pY < _levelsMap.GetLength(1))
                    {
                        levels.Add(_levelsMap[pX, pY]);
                    }
                }
            }
        }

        return levels;
    }

    private int PlayerPositionIndexX => Mathf.FloorToInt(_player.position.x / 40f);
    private int PlayerPositionIndexY => Mathf.FloorToInt(_player.position.y / 40f);
}
