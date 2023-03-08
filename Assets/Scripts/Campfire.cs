using System.IO;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    [SerializeField] private string _saveFileName;
    private PlayerDataCollector _playerSaver;
    private ChestsStateSaver _chestSaver;
    private EnemiesSaver _enemySaver;
    private TorchesSaver _torchesSaver;
    private PlayerHealth _playerHealth;
    private SaveNotice _saveText;
    private bool _isPlayerInRange;

    private void Start()
    {
        _playerSaver = FindObjectOfType<PlayerDataCollector>();
        _chestSaver = FindObjectOfType<ChestsStateSaver>();
        _enemySaver = FindObjectOfType<EnemiesSaver>();
        _torchesSaver = FindObjectOfType<TorchesSaver>();

        _playerHealth = FindObjectOfType<PlayerHealth>();
        _saveText = FindObjectOfType<SaveNotice>(true);
    }

    private void Update()
    {
        if(_isPlayerInRange
            && Input.GetKeyDown(KeyCode.E))
        {
            _playerHealth.SetHealth(_playerHealth.GetHealthParams().max);

            if (_playerSaver.SaveData(_saveFileName))
            {
                _saveText.Show();
                _chestSaver.Save("ChestsSave");
                _enemySaver.Save("EnemiesSave");
                _torchesSaver.Save("TorchesSave");

                string path = Application.dataPath + "/VillageBoss";
                if (File.Exists(path + ".add"))
                {
                    if (!File.Exists(path))
                    {
                        using FileStream stream = File.Create(path);
                    }

                    File.WriteAllText(path, File.ReadAllText(path + ".add"));
                }

                Invoke(nameof(HideSaveText), 1.2f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isPlayerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isPlayerInRange = false;
    }

    public void HideSaveText()
    {
        _saveText.Hide();
    }
}
