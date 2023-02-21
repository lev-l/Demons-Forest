using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        string path = Application.dataPath + "/";
        File.Delete(path + "MainSave");
        File.Delete(path + "MainSave.add");
        File.Delete(path + "ChestsSave");
        File.Delete(path + "ChestsSave.add");
        File.Delete(path + "CompasSave");
        File.Delete(path + "JournalSave");

        SceneManager.LoadScene((int)Scenes.Game);
    }

    public void Continue()
    {
        SceneManager.LoadScene((int)Scenes.Game);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
