using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ChestsStateSaver : MonoBehaviour
{
    public string SaveFileName;
    public Dictionary<string, bool> ChestsStates;

    private void Awake()
    {
        ChestsStates = new Dictionary<string, bool>();
    }

    public void Save(string filename)
    {
        string fullPath = Application.dataPath + "/" + filename;

        if (!File.Exists(fullPath))
        {
            using FileStream stream = File.Create(fullPath);
            stream.Close();
        }

        File.WriteAllText(fullPath, JsonConvert.SerializeObject(ChestsStates));
    }

    public Dictionary<string, bool> Load(string filename)
    {
        string fullPath = Application.dataPath + "/" + filename;

        if (File.Exists(fullPath))
        {
            return JsonConvert.DeserializeObject<Dictionary<string, bool>>(File.ReadAllText(fullPath));
        }
        else
        {
            return new Dictionary<string, bool>();
        }
    }
}
