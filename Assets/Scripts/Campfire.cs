using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    [SerializeField] private string _saveFileName;
    private PlayerDataCollector _saver;
    private SaveNotice _saveText;
    private bool _isPlayerInRange;

    private void Start()
    {
        _saver = FindObjectOfType<PlayerDataCollector>();
        _saveText = FindObjectOfType<SaveNotice>(true);
    }

    private void Update()
    {
        if(_isPlayerInRange
            && Input.GetKeyDown(KeyCode.E))
        {
            _saver.SaveData(_saveFileName);
            _saveText.Show();
            Invoke(nameof(HideSaveText), 1.2f);
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
