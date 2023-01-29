using System.Collections;
using UnityEngine;

public class MarkDelete : MonoBehaviour
{
    public void Delete(GameObject mark, CompassData _data)
    {
        _data.Marks.Remove(mark);
        _data.MarksObjects.Remove(mark);
        Destroy(mark);
    }
}
