using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnTouch : MonoBehaviour
{
    private GameObject _lastObject;

    public void Show(GameObject gObject)
    {
        if (_lastObject)
        {
            _lastObject.SetActive(false);
        }
        
        gObject.SetActive(true);
        _lastObject = gObject;
    }
}
