using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Book : MonoBehaviour
{
    [TextArea][SerializeField] private string _text;
    private GameObject _bookPanel;
    private Journal _journalData;
    private bool _playerInRange;

    private void Start()
    {
        _bookPanel = FindObjectOfType<BookFieldContainer>().BooksField;
        _journalData = Resources.Load<Journal>("JournalData");
        _playerInRange = false;
    }

    private void Update()
    {
        if(_playerInRange
            && Input.GetKeyDown(KeyCode.E))
        {
            _bookPanel.SetActive(true);
            _bookPanel.GetComponentInChildren<TextMeshProUGUI>().text = _text;
            _journalData.AddText(_text);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _playerInRange = false;
    }
}