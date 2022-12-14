using System.Collections.Generic;
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

        List<string> levelsToLoad = GetNearLevels(0, 0);

        foreach (string level in levelsToLoad)
        {
            SceneManager.LoadScene(level, LoadSceneMode.Additive);
        }
    }

    private void FixedUpdate()
    {
        int positionX = Mathf.FloorToInt(_player.position.x / 40f);
        int positionY = Mathf.FloorToInt(_player.position.y / 40f);

        if(_lastPositionX == positionX
            && _lastPositionY == positionY)
        {
            return;
        }

        _lastPositionX = positionX;
        _lastPositionY = positionY;

        GridGraph graph = _pathfinder.data.gridGraph;
        graph.RelocateNodes(new Vector3(20 + 40 * positionX, 20 + 40 * positionY), Quaternion.identity, 1);
        graph.is2D = true;

        List<string> levelsToLoad = GetNearLevels(positionX, positionY);

        foreach(string level in levelsToLoad)
        {
            if (SceneManager.GetSceneByName(level) == null)
            {
                SceneManager.LoadScene(level, LoadSceneMode.Additive);
            }
        }

        graph.Scan();
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
}
