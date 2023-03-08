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
    private EnemiesSaver _enemiesData;
    private Scene _mainScene;
    private string[,] _levelsMap;
    private string[] _lastLoadedLevels;
    private string[] _levelsLoaded;
    private int _lastPositionX, _lastPositionY;

    private void Start()
    {
        // setting params
        _mainScene = SceneManager.GetSceneByBuildIndex((int)Scenes.Game);
        _levelsMap = new Forest_levelsMap().GetMap();
        _player = FindObjectOfType<PlayerMovement>().transform;
        _pathfinder = FindObjectOfType<AstarPath>();
        _enemiesData = FindObjectOfType<EnemiesSaver>();

        // loading saved data
        #region Loading
        string filename = PlayerPrefs.GetString("FileToLoad", "");
        ChestsStateSaver chestData = FindObjectOfType<ChestsStateSaver>();
        PlayerDataCollector playerData = FindObjectOfType<PlayerDataCollector>();
        TorchesSaver torchData = FindObjectOfType<TorchesSaver>();

        if (filename.Length > 0)
        {
            string[] filenameParts = filename.Split('.');

            if(filenameParts.Length > 1)
            {
                if (playerData.LoadData(filename))
                {
                    chestData.ChestsStates = chestData.Load("ChestsSave.add");
                    _enemiesData.Load("EnemiesSave.add");
                    torchData.Load("TorchesSave.add");
                }
                else
                {
                    if (playerData.LoadData("MainSave"))
                    {
                        chestData.ChestsStates = chestData.Load("ChestsSave");
                        _enemiesData.Load("EnemiesSave");
                        torchData.Load("TorchesSave");
                    }
                }
            }
            else
            {
                if (playerData.LoadData(filename))
                {
                    chestData.ChestsStates = chestData.Load("ChestsSave");
                    _enemiesData.Load("EnemiesSave");
                    torchData.Load("TorchesSave");
                }
            }
        }
        #endregion Loading

        // loading nearby locations
        _lastPositionX = PlayerPositionIndexX;
        _lastPositionY = PlayerPositionIndexY;
        StartCoroutine(LoadNearLevels(PlayerPositionIndexX, PlayerPositionIndexY));
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
                _enemiesData.Data.UpdateLocation();

                StartCoroutine(LoadNearLevels(positionX, positionY));

                yield return new WaitForFixedUpdate();

                GridGraph graph = _pathfinder.data.gridGraph;
                Vector2 currentLevelCenter = new Vector3(20 + 40 * positionX, 20 + 40 * positionY);
                graph.RelocateNodes(currentLevelCenter, Quaternion.identity, 1);
                graph.is2D = true;

                yield return new WaitForFixedUpdate();

                graph.Scan();

                yield return new WaitForSeconds(0.5f);

                UnloadLevels();

                yield return new WaitForSeconds(3.5f);
            }

            yield return new WaitForSeconds(4);
        }
    }

    private IEnumerator LoadNearLevels(int x, int y)
    {
        List<string> levelsToLoad = GetNearLevels(x, y);

        foreach(string level in levelsToLoad)
        {
            if (SceneManager.GetSceneByName(level).name == null)
            {
                SceneManager.LoadScene(level, LoadSceneMode.Additive);
                Scene loadedScene = SceneManager.GetSceneByName(level);

                while (!loadedScene.isLoaded)
                {
                    yield return null;
                }

                var loadedGroups = from root in loadedScene.GetRootGameObjects()
                                   where root.GetComponent<GroupForm>()
                                   select root.GetComponent<GroupForm>();
                var existingGroups = from root in _mainScene.GetRootGameObjects()
                                     where root.GetComponent<GroupForm>()
                                     select root.GetComponent<GroupForm>().Hesh;

                foreach (GroupForm loadedGroup in loadedGroups)
                {
                    bool destroy = false;

                    foreach (string existingGroup in existingGroups)
                    {
                        if (existingGroup == loadedGroup.Hesh)
                        {
                            Destroy(loadedGroup.gameObject);
                            destroy = true;
                            break;
                        }
                    }

                    if(!destroy)
                    {
                        bool killed = false;

                        IEnumerator<string> killedGroups = _enemiesData.Data.GetKilled();
                        while (killedGroups.MoveNext())
                        {
                            if(killedGroups.Current == loadedGroup.Hesh)
                            {
                                Destroy(loadedGroup.gameObject);
                                killed = true;
                                break;
                            }
                        }

                        if (!killed)
                        {
                            SceneManager.MoveGameObjectToScene(loadedGroup.gameObject, _mainScene);
                        }
                    }
                }
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
