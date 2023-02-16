using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "JournalData")]
public class Journal : ScriptableObject
{
    public string SaveFileName;
    private List<string> _texts;

    private void OnEnable()
    {
        _texts = Load(SaveFileName);
    }

    public int TextsCount => _texts.Count;

    public string GetText(int i) => _texts[i];

    public void AddText(string newText)
    {
        if (!_texts.Contains(newText))
        {
            _texts.Add(newText);
            Save(SaveFileName);
        }
    }

    private void Save(string filename)
    {
        string fullPath = Application.dataPath + "/" + filename;

        if (!File.Exists(fullPath))
        {
            using FileStream stream = File.Create(fullPath);
            stream.Close();
        }

        File.WriteAllText(fullPath, JsonConvert.SerializeObject(_texts));
    }

    private List<string> Load(string filename)
    {
        string fullPath = Application.dataPath + "/" + filename;

        if (File.Exists(fullPath))
        {
            return JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(fullPath));
        }
        else
        {
            return new List<string>();
        }
    }
}
