using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.SceneTemplate;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

using UnityEngine;

public class SceneProcessor : EditorWindow
{
    private IntegerField _numberOfScenes;
    private TextField _firstSceneName;
    private VisualElement _root;

    [MenuItem("Tools/SceneProcessor")]
    public static void ShowSceneProcessor()
    {
        SceneProcessor processorWindow = GetWindow<SceneProcessor>();
        processorWindow.titleContent = new UnityEngine.GUIContent("Scene Processor");
    }

    private void CreateScenes()
    {
        if (!_firstSceneName.text.Contains("_")
            || !_firstSceneName.text.Contains("-"))
        {
            HelpBox nameError = new HelpBox("The name of the first scene should be in format \"ForestD_3-7\"!", HelpBoxMessageType.Error);
            _root.Add(nameError);
            return;
        }
        if (Convert.ToInt32(_numberOfScenes.text) < 1)
        {
            HelpBox numberError = new HelpBox("The number should not be less than one!", HelpBoxMessageType.Error);
            _root.Add(numberError);
            return;
        }

        string[] splitName = _firstSceneName.text.Split('_');
        string[] splitNumbers = splitName[1].Split('-');
        int number1 = Convert.ToInt32(splitNumbers[0]);
        int number2 = Convert.ToInt32(splitNumbers[1]);

        string path = GetSelectedPathOrFallback() + "/";

        for (number1++; number1 < Convert.ToInt32(_numberOfScenes.text); number1++)
        {
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            newScene.name = $"ForestD_{number1}-{number2}";
            EditorSceneManager.SaveScene(newScene, path + $"ForestD_{number1}-{number2}.unity");
        }
    }

    public static string GetSelectedPathOrFallback()
    {
        string path = "Assets";

        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }

    public void CreateGUI()
    {
        _root = rootVisualElement;

        HelpBox explanation = new HelpBox("First set the number of scenes you want to create, then enter name of the first one." +
                                        "Create the first scene in the target folder, make sure you select it. After all that, push the button!",
                                        HelpBoxMessageType.Info);
        _root.Add(explanation);

        _numberOfScenes = new IntegerField("Number of Scenes", maxLength: 30);
        _root.Add(_numberOfScenes);

        _firstSceneName = new TextField("The name of the first scene");
        _root.Add(_firstSceneName);

        Button createScenesButton = new Button(CreateScenes);
        createScenesButton.name = "Create Scenes";
        createScenesButton.text = "Create Scenes";
        _root.Add(createScenesButton);

        HelpBox path = new HelpBox(GetSelectedPathOrFallback(), HelpBoxMessageType.None);
        _root.Add(path);
    }
}
