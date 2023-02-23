using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private SaveNotice _confirmationMenu;

    private void Start()
    {
        _confirmationMenu = FindObjectOfType<SaveNotice>(true);

        if(!File.Exists(Application.dataPath + "/MainSave.add")
            && !File.Exists(Application.dataPath + "/MainSave"))
        {
            GameObject.Find("Continue").GetComponent<Button>().interactable = false;
        }
    }

    public void NewGame()
    {
        if(File.Exists(Application.dataPath + "/MainSave")
            || File.Exists(Application.dataPath + "/MainSave.add"))
        {
            _confirmationMenu.Show();
        }
        else
        {
            StartNewGame();
        }
    }

    public void StartNewGame()
    {
        string path = Application.dataPath + "/";
        File.Delete(path + "MainSave");
        File.Delete(path + "MainSave.add");
        File.Delete(path + "ChestsSave");
        File.Delete(path + "ChestsSave.add");
        File.Delete(path + "CompasSave");
        File.Delete(path + "JournalSave");
        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene((int)Scenes.Game);
    }

    public void Continue()
    {
        SceneManager.LoadScene((int)Scenes.Game);
        PlayerPrefs.SetString("FileToLoad", "MainSave.add");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
