using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class AutoGrassPlace : ScriptableWizard
{
    [MenuItem("Tools/Place Grass")]
    static void ShowAutoGrassPlaceWindow()
    {
        string activeScene = EditorSceneManager.GetActiveScene().name;
        // Names based on how the result should look like
        string x_y = activeScene.Split('_')[1];
        string[] xy = x_y.Split('-');
        int x = Convert.ToInt32(xy[0]);
        int y = Convert.ToInt32(xy[1]);

        GameObject grass = Instantiate(Resources.Load("Grass"),
                                new Vector2(20 + 40 * x, 20 + 40 * y), Quaternion.identity) as GameObject;

        EditorUtility.SetDirty(grass);
    }
}
