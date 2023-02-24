using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class EnemiesSaver : MonoBehaviour
{
    public EnemiesData Data;

    public void Awake()
    {
        Data = new EnemiesData(new List<string>(), new List<string>());
    }

    public void Save(string filename)
    {
        string fullPath = Application.dataPath + "/" + filename;

        if (!File.Exists(fullPath))
        {
            using FileStream stream = File.Create(fullPath);
            stream.Close();
        }

        File.WriteAllText(fullPath, JsonConvert.SerializeObject(Data));
    }

    public void Load(string filename)
    {
        string fullPath = Application.dataPath + "/" + filename;

        if (File.Exists(fullPath))
        {
            Data = JsonConvert.DeserializeObject<EnemiesData>(File.ReadAllText(fullPath));
        }
        else
        {
            Data = new EnemiesData(new List<string>(), new List<string>());
        }
    }
}
