using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CompassDataBase")]
public class CompassData : ScriptableObject
{
    public Dictionary<GameObject, Vector2> Marks;
    public List<GameObject> MarksObjects;

    private void OnEnable()
    {
        Marks = new Dictionary<GameObject, Vector2>();
        MarksObjects = new List<GameObject>();
    }
}
