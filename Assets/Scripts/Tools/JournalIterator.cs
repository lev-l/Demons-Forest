using UnityEngine;
using TMPro;

public class JournalIterator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pageNumber, _pageText;
    private Journal _journalData;
    private int _currentIndex;

    private void Awake()
    {
        _journalData = Resources.Load<Journal>("JournalData");
        _currentIndex = 0;
    }

    private void OnEnable()
    {
        UpdatePage();
    }

    public void GoForward()
    {
        if (_journalData.TextsCount > 0)
        {
            _currentIndex++;

            if (_currentIndex > _journalData.TextsCount - 1)
            {
                _currentIndex = 0;
            }

            UpdatePage();
        }
    }

    public void GoBack()
    {
        if(_journalData.TextsCount > 0)
        {
            _currentIndex--;

            if(_currentIndex < 0)
            {
                _currentIndex = _journalData.TextsCount - 1;
            }

            UpdatePage();
        }
    }

    private void UpdatePage()
    {
        if (_journalData.TextsCount > 0)
        {
            _pageNumber.text = (_currentIndex + 1).ToString();
            _pageText.text = _journalData.GetText(_currentIndex);
        }
    }
}
