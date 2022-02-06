using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSet : MonoBehaviour
{
    private void Awake()
    {
        FindObjectOfType<ChestContentsPresenter>()
                .SetCanvas(gameObject.GetComponent<Canvas>());
    }
}
