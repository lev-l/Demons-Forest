using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class EnemiesSaver : MonoBehaviour
{
    private EnemiesData _data;

    public void Save(string filename)
    {
        string fullPath = Application.dataPath + "/" + filename;

        if (!File.Exists(fullPath))
        {
            using FileStream stream = File.Create(fullPath);
            stream.Close();
        }

        File.WriteAllText(fullPath, JsonConvert.SerializeObject(_data));
    }

    public void Load(string filename)
    {
        string fullPath = Application.dataPath + "/" + filename;

        if (File.Exists(fullPath))
        {
            _data = JsonConvert.DeserializeObject<EnemiesData>(File.ReadAllText(fullPath));
        }
        else
        {
            _data = new EnemiesData(new List<string>(), new List<string>());
        }
    }
}
