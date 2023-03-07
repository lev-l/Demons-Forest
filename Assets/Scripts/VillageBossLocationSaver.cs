using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Newtonsoft.Json;

public class VillageBossLocationSaver : MonoBehaviour
{
    public Sprite DestroyedSprite;
    public DestroyableHouse[] Houses;
    private List<string> _destroyedHouses;

    private void Start()
    {
        _destroyedHouses = new List<string>();

        if (!Load("VillageBoss.add"))
        {
            Load("VillageBoss");
        }

        foreach(string destroyedHesh in _destroyedHouses)
        {
            print(destroyedHesh);
            foreach(DestroyableHouse house in Houses)
            {
                if (house.Hesh == destroyedHesh)
                {
                    Destroy(house.GetComponent<Collider2D>());
                    Destroy(house.GetComponent<ShadowCaster2D>());
                    house.GetComponent<SpriteRenderer>().sprite = DestroyedSprite;
                }
            }
        }
    }

    public void Save(string filename)
    {
        string fullPath = Application.dataPath + "/" + filename;

        if (!File.Exists(fullPath))
        {
            using FileStream stream = File.Create(fullPath);
            stream.Close();
        }

        File.WriteAllText(fullPath, JsonConvert.SerializeObject(GetHeshes()));
    }

    public bool Load(string filename)
    {
        string fullPath = Application.dataPath + "/" + filename;

        if (File.Exists(fullPath))
        {
            _destroyedHouses.AddRange(JsonConvert.DeserializeObject<string[]>(File.ReadAllText(fullPath)));
            return true;
        }
        else
        {
            return false;
        }
    }

    private string[] GetHeshes()
    {
        string[] heshes = new string[Houses.Length];
        for (int i = 0; i < Houses.Length; i++)
        {
            if (Houses[i] != null)
            {
                heshes[i] = Houses[i].Hesh;
            }
        }

        return heshes;
    }

    public void AddDestroyed(string hesh)
    {
        print(hesh);
        _destroyedHouses.Add(hesh);
    }
}
