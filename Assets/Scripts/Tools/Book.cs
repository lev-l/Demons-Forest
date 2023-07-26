using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Book : MonoBehaviour
{
    [TextArea][SerializeField] private string _text;
    [SerializeField] private Vector2[] _compassPositions;
    [Header("Important! Positions and names must be synced!")]
    [SerializeField] private string[] _compassNames;
    private GameObject _bookPanel;
    private Journal _journalData;
    private CompassPresenter _compass;
    private bool _playerInRange;

    private void Start()
    {
        _bookPanel = FindObjectOfType<BookFieldContainer>().BooksField;
        _journalData = Resources.Load<Journal>("JournalData");
        _compass = FindObjectOfType<CompassPresenter>();
        _playerInRange = false;

        if(_compassPositions.Length != _compassNames.Length)
        {
            Debug.LogError("The names and positions aren't synced! (Chech notes, script \"Book\")");
        }
    }

    private void Update()
    {
        if(_playerInRange
            && Input.GetKeyDown(KeyCode.E))
        {
            _bookPanel.SetActive(true);
            _bookPanel.GetComponentInChildren<TextMeshProUGUI>().text = _text;
            _journalData.AddText(_text);

            for(int i = 0; i < _compassNames.Length; i++)
            {
                _compass.AddAMarkExternal(_compassNames[i], _compassPositions[i]);
            }
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
