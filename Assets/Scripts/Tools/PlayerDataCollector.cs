using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class PlayerDataCollector : MonoBehaviour
{
    private PlayerObject _player;
    private Transform _playerTransform;
    private Inventory _playerInventory;
    private UnityNewtonsoftJsonSerializer _jsonSaver;

    private void Start()
    {
        _player = Resources.Load<PlayerObject>("Player");
        _playerTransform = FindObjectOfType<PlayerMovement>().transform;
        _playerInventory = FindObjectOfType<Inventory>();
        _jsonSaver = Resources.Load<UnityNewtonsoftJsonSerializer>("UnityNewtonsoftJsonSerializer");
    }

    public bool SaveData(string filename)
    {
        if (_player.NumberEnemiesSeeYou == 0)
        {
            string fullPath = Application.dataPath + "/" + filename;

            if (!File.Exists(fullPath))
            {
                using FileStream stream = File.Create(fullPath);
                stream.Close();
            }
            PlayerDataSaver data = new PlayerDataSaver(_playerTransform.position,
                                                    _player.Health,
                                                    _playerInventory.GetContent());
            File.WriteAllText(fullPath, _jsonSaver.Serialize(data));

            return true;
        }
        return false;
    }

    public bool LoadData(string filename)
    {
        string fullPath = Application.dataPath + "/" + filename;

        if (File.Exists(fullPath))
        {
            PlayerDataSaver data = JsonConvert.DeserializeObject<PlayerDataSaver>(File.ReadAllText(fullPath));

            _playerTransform.GetComponent<PlayerHealth>().SetHealth(data.Health);
            _playerTransform.position = data.PlayerPosition;
            _playerInventory.AddObjects(GetLoadedItems(data));
            return true;
        }
        else
        {
            Debug.LogWarning("Tried to load a game save but no file has been found.");
            return false;
        }
    }

    private List<CollectableObject> GetLoadedItems(PlayerDataSaver data)
    {
        List<CollectableObject> loadedItems = new List<CollectableObject>();
        foreach (Collectables item in data.Inventory.Keys)
        {
            switch (item)
            {
                case Collectables.HealBottle:
                    for (int i = 0; i < data.Inventory[item]; i++)
                    {
                        loadedItems.Add(new HealBottleObject());
                    }
                    break;

                case Collectables.ThrowingKnife:
                    for (int i = 0; i < data.Inventory[item]; i++)
                    {
                        loadedItems.Add(new ThrowingKnifeObject());
                    }
                    break;

                case Collectables.StaticTorch:
                    for (int i = 0; i < data.Inventory[item]; i++)
                    {
                        loadedItems.Add(new StaticTorchObject());
                    }
                    break;

                default:
                    Debug.LogAssertion("Unintended data type.");
                    break;
            }
        }

        return loadedItems;
    }
}