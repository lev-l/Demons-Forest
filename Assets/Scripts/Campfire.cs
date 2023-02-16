using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    [SerializeField] private string _saveFileName;
    private PlayerDataCollector _saver;
    private PlayerHealth _playerHealth;
    private SaveNotice _saveText;
    private bool _isPlayerInRange;

    private void Start()
    {
        _saver = FindObjectOfType<PlayerDataCollector>();
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _saveText = FindObjectOfType<SaveNotice>(true);
    }

    private void Update()
    {
        if(_isPlayerInRange
            && Input.GetKeyDown(KeyCode.E))
        {
            if (_saver.SaveData(_saveFileName))
            {
                _saveText.Show();
                _playerHealth.SetHealth(_playerHealth.GetHealthParams().max);
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
