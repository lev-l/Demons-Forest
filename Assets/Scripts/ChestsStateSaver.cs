using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ChestsStateSaver : MonoBehaviour
{
    public string SaveFileName;
    public Dictionary<string, bool> ChestsStates;
    private string _savePath;

    private void Start()
    {
        _savePath = Application.dataPath + "/" + SaveFileName;
        ChestsStates = Load();
    }

    public void Save()
    {
        if (!File.Exists(_savePath))
        {
            using FileStream stream = File.Create(_savePath);
            stream.Close();
        }

        File.WriteAllText(_savePath, JsonConvert.SerializeObject(ChestsStates));
    }

    public Dictionary<string, bool> Load()
    {
        if (File.Exists(_savePath))
        {
            return JsonConvert.DeserializeObject<Dictionary<string, bool>>(File.ReadAllText(_savePath));
        }
        else
        {
            return new Dictionary<string, bool>();
        }
    }
}
