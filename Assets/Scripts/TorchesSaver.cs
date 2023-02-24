using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class TorchesSaver : MonoBehaviour
{
    public GameObject TorchPrefab;
    private List<Transform> _torches;
    private UnityNewtonsoftJsonSerializer _json;

    private void Awake()
    {
        _torches = new List<Transform>();
        _json = Resources.Load<UnityNewtonsoftJsonSerializer>("UnityNewtonsoftJsonSerializer");
    }

    public void Save(string filename)
    {
        string fullPath = Application.dataPath + "/" + filename;

        if (!File.Exists(fullPath))
        {
            using FileStream stream = File.Create(fullPath);
            stream.Close();
        }

        File.WriteAllText(fullPath, _json.Serialize(TransformToVector(_torches)));
    }

    public void Load(string filename)
    {
        string fullPath = Application.dataPath + "/" + filename;

        if (File.Exists(fullPath))
        {
            InitializeTorches(_json.Deserialize<Vector2[]>(File.ReadAllText(fullPath)));
        }
        else
        {
            _torches = new List<Transform>();
        }
    }

    private void InitializeTorches(Vector2[] positions)
    {
        foreach(Vector2 torchPosition in positions)
        {
            GameObject newTorch = Instantiate(TorchPrefab);
            newTorch.transform.position = torchPosition;
            AddTorch(newTorch.transform);
        }
    }

    private Vector2[] TransformToVector(List<Transform> transforms)
    {
        Vector2[] vectors = new Vector2[transforms.Count];

        for(int i = 0; i < vectors.Length; i++)
        {
            vectors[i] = transforms[i].position;
        }

        return vectors;
    }

    public void AddTorch(Transform torch)
    {
        _torches.Add(torch);
    }

    public void RemoveTorch(Transform torch)
    {
        _torches.Remove(torch);
    }
}
