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
    private int _lastPositionX, _lastPositionY;

    private void Start()
    {
        _levelsMap = new Forest_levelsMap().GetMap();
        _lastPositionX = _lastPositionY = 0;
        _player = FindObjectOfType<PlayerMovement>().transform;
        _pathfinder = FindObjectOfType<AstarPath>();

        LoadNearLevels(PlayerPositionIndexX, PlayerPositionIndexY);

        StartCoroutine(nameof(UpdateLoad));
    }

    private IEnumerator UpdateLoad()
    {
        int positionX = PlayerPositionIndexX;
        int positionY = PlayerPositionIndexY;

        if(_lastPositionX == positionX
            && _lastPositionY == positionY)
        {
            yield return new WaitForSeconds(5);
        }

        _lastPositionX = positionX;
        _lastPositionY = positionY;

        LoadNearLevels(positionX, positionY);

        yield return null;

        GridGraph graph = _pathfinder.data.gridGraph;
        Vector2 currentLevelCenter = new Vector3(20 + 40 * positionX, 20 + 40 * positionY);
        graph.RelocateNodes(currentLevelCenter, Quaternion.identity, 1);
        graph.is2D = true;

        yield return null;

        graph.Scan();

        yield return new WaitForSeconds(5);
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
    }

    private List<string> GetNearLevels(int x, int y)
    {
        List<string> levels = new List<string>();

        for(int pX = x - 1; pX < _levelsMap.GetLength(0); pX++)
        {
            if (pX >= 0)
            {
                for (int pY = y - 1; pY < _levelsMap.GetLength(1); pY++)
                {
                    if(pY >= 0)
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
