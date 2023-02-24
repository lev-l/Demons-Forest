using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class CompassDataSave
{
    private UnityNewtonsoftJsonSerializer _unitySerializer;

    public CompassDataSave()
    {
        _unitySerializer = Resources.Load<UnityNewtonsoftJsonSerializer>("UnityNewtonsoftJsonSerializer");
    }

    public void Save(Dictionary<string, Vector2> markPosition, string filename)
    {
        string fullPath = Application.dataPath + "/" + filename;

        try
        {
            if (!File.Exists(fullPath))
            {
                using FileStream stream = File.Create(fullPath);
                stream.Close();
            }

            File.WriteAllText(fullPath, _unitySerializer.Serialize(markPosition));
        }
        catch (Exception error)
        {
            Debug.LogError("Saving compass data went wrong due to: " + error.Message);
        }
    }

    public Dictionary<string, Vector2> Load(string filename)
    {
        string fullPath = Application.dataPath + "/" + filename;

        if (!File.Exists(fullPath))
        {
            //Debug.LogWarning("Tryed to load a compas save, but no save exists.");
            return null;
        }

        return _unitySerializer.Deserialize<Dictionary<string,Vector2>>(File.ReadAllText(fullPath));
    }
}
