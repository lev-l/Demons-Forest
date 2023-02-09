using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JournalData")]
public class Journal : ScriptableObject
{
    private List<string> _texts;

    private void OnEnable()
    {
        _texts = new List<string>();
    }

    public int TextsCount => _texts.Count;

    public string GetText(int i) => _texts[i];

    public void AddText(string newText)
    {
        if (!_texts.Contains(newText))
        {
            _texts.Add(newText);
        }
    }
}
