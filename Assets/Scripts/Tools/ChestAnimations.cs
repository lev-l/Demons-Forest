using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAnimations : MonoBehaviour
{
    [SerializeField] private Sprite _openedSprite;
    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void AnimationOpen()
    {
        _renderer.sprite = _openedSprite;
    }
}
