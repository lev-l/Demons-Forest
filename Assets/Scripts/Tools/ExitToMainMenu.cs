using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ExitToMainMenu : MonoBehaviour
{
    private PlayerDataCollector _playerData;
    private ChestsStateSaver _chestsData;
    //private Smth _locationData;

    private void Start()
    {
        _playerData = FindObjectOfType<PlayerDataCollector>();
        _chestsData = FindObjectOfType<ChestsStateSaver>();
    }

    public void ExitToMenu()
    {
        if (_playerData.SaveData("MainSave.add"))
        {
            _chestsData.Save("ChestsSave.add");
            Time.timeScale = 1;
            SceneManager.LoadScene((int)Scenes.MainMenu);
        }
        else
        {
            GetComponentInChildren<TextMeshProUGUI>().text = "Can't exit durring battle";
            Invoke(nameof(BackToNormal), 0.1f);
        }
    }

    public void BackToNormal()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = "Your game will be saved";
    }
}
