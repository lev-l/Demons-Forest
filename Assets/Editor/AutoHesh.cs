using UnityEditor.SceneManagement;
using UnityEditor;

public class AutoHesh : ScriptableWizard
{
    [MenuItem ("Tools/Auto Heshing")]
    private static void ShowAutoHeshWindow()
    {
        string activeScene = EditorSceneManager.GetActiveScene().name;

        GroupForm[] enemyGroups = FindObjectsOfType<GroupForm>();

        for (int i = 0; i < enemyGroups.Length; i++)
        {
            enemyGroups[i].Hesh = activeScene;
            if (i > 0)
            {
                enemyGroups[i].Hesh += $"({i})";
            }
        }

        Chest[] chests = FindObjectsOfType<Chest>();

        for (int i = 0; i < chests.Length; i++)
        {
            chests[i].Hesh = activeScene;
            if (i > 0)
            {
                chests[i].Hesh += $"({i})";
            }
        }

        EnemyUnload[] enemies = FindObjectsOfType<EnemyUnload>();

        foreach (EnemyUnload enemy in enemies)
        {
            enemy.Origin = activeScene;
        }
    }
}
