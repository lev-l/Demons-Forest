using System.Collections;
using UnityEngine;

public class ChestStateLoader : MonoBehaviour
{
    private Chest _chest;
    private ChestsStateSaver _stateSave;

    private void Start()
    {
        _chest = GetComponent<Chest>();
        _stateSave = FindObjectOfType<ChestsStateSaver>();

        bool isEmpty = false;
        if (_stateSave.ChestsStates.TryGetValue(_chest.Hesh, out isEmpty)
            && isEmpty)
        {
            _chest.Empty();
        }
    }
}
