using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CompassDataBase")]
public class CompassData : ScriptableObject
{
    public Dictionary<GameObject, Vector2> Marks = new Dictionary<GameObject, Vector2>();
    public List<GameObject> MarksObjects = new List<GameObject>();

    //private void OnEnable()
    //{
    //    if(Marks == null)
    //    {
    //        Marks = new Dictionary<GameObject, Vector2>();
    //    }
    //}
}
