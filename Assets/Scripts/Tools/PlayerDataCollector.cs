using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class PlayerDataCollector : MonoBehaviour
{
    private PlayerObject _player;
    private Transform _playerTransform;
    private InventoryContents _playerInventory;

    private void Start()
    {
        _player = Resources.Load<PlayerObject>("Player");
        _playerTransform = FindObjectOfType<PlayerMovement>().transform;
        _playerInventory = Resources.Load<InventoryContents>("PlayerInventory");

        LoadData("MainSave");
    }

    public void SaveData(string filename)
    {
        string fullPath = Application.dataPath + "/" + filename;

        if (!File.Exists(fullPath))
        {
            using FileStream stream = File.Create(fullPath);
            stream.Close();
        }
        PlayerDataSaver data = new PlayerDataSaver(_playerTransform.position,
                                                _player.Health,
                                                _playerInventory.GetInventoryContent());
        File.WriteAllText(fullPath, JsonConvert.SerializeObject(data));
    }

    public void LoadData(string filename)
    {
        string fullPath = Application.dataPath + "/" + filename;

        if (File.Exists(fullPath))
        {
            PlayerDataSaver data = JsonConvert.DeserializeObject<PlayerDataSaver>(File.ReadAllText(fullPath));

            _playerTransform.GetComponent<PlayerHealth>().SetHealth(data.Health);
            _playerTransform.position = data.PlayerPosition;
            AddHealsBottlesToInventory(data.Inventory);
            AddThrowingKnivesToInventory(data.Inventory);
            AddStaticTorchesToInventory(data.Inventory);
        }
        else
        {
            Debug.LogWarning("Tried to load a game save but no file has been found.");
            return;
        }
    }

    private void AddHealsBottlesToInventory(Dictionary<Collectables, int> data)
    {
        for (int i = 0; i < data[Collectables.HealBottle]; i++)
        {
            _playerInventory.AddHealthBottle();
        }
    }

    private void AddThrowingKnivesToInventory(Dictionary<Collectables, int> data)
    {
        for (int i = 0; i < data[Collectables.ThrowingKnife]; i++)
        {
            _playerInventory.AddThrowingKnife();
        }
    }

    private void AddStaticTorchesToInventory(Dictionary<Collectables, int> data)
    {
        for (int i = 0; i < data[Collectables.StaticTorch]; i++)
        {
            _playerInventory.AddStaticTorch();
        }
    }
}