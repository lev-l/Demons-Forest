using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ExitToMainMenu : MonoBehaviour
{
    private PlayerDataCollector _playerData;
    private ChestsStateSaver _chestsData;
    private EnemiesSaver _enemiesData;
    private TorchesSaver _torchesData;

    private void Start()
    {
        _playerData = FindObjectOfType<PlayerDataCollector>();
        _chestsData = FindObjectOfType<ChestsStateSaver>();
        _enemiesData = FindObjectOfType<EnemiesSaver>();
        _torchesData = FindObjectOfType<TorchesSaver>();
    }

    public void ExitToMenu()
    {
        if (_playerData.SaveData("MainSave.add"))
        {
            _chestsData.Save("ChestsSave.add");
            _enemiesData.Save("EnemiesSave.add");
            _torchesData.Save("TorchesSave.add");

            Time.timeScale = 1;
            SceneManager.LoadScene((int)Scenes.MainMenu);
        }
        else
        {
            GetComponentInChildren<TextMeshProUGUI>().text = "Can't exit during battle";
            Invoke(nameof(BackToNormal), 0.1f);
        }
    }

    public void BackToNormal()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = "Your game will be saved";
    }
}