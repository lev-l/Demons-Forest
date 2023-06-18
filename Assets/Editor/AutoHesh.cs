using UnityEditor.SceneManagement;
using UnityEditor;

public class AutoHesh : ScriptableWizard
{
    public string Scene;

    [MenuItem ("Tools/Auto Heshing")]
    private static void ShowAutoHeshWindow()
    {
        ScriptableWizard.DisplayWizard<AutoHesh>("Auto Heshing", "HeshEnemies", "HeshChests");
    }

    private void OnWizardCreate()
    {
        GroupForm[] enemyGroups = FindObjectsOfType<GroupForm>();

        for (int i = 0; i < enemyGroups.Length; i++)
        {
            enemyGroups[i].Hesh = EditorSceneManager.GetActiveScene().name;
            if (i > 0)
            {
                enemyGroups[i].Hesh += $"({i})";
            }
        }

        EnemyUnload[] enemies = FindObjectsOfType<EnemyUnload>();

        foreach(EnemyUnload enemy in enemies)
        {
            enemy.Origin = EditorSceneManager.GetActiveScene().name;
        }
    }

    private void OnWizardOtherButton()
    {
        Chest[] chests = FindObjectsOfType<Chest>();

        for (int i = 0; i < chests.Length; i++)
        {
            chests[i].Hesh = EditorSceneManager.GetActiveScene().name;
            if (i > 0)
            {
                chests[i].Hesh += $"({i})";
            }
        }
    }
}
