using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseTouched : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<GameObject> OnMouseEnter;
    public event Action OnMouseExit;
    [SerializeField] private GameObject _text;

    public TextMeshProUGUI Text => _text.GetComponent<TextMeshProUGUI>();

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter?.Invoke(_text);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.SetActive(false);
        OnMouseExit?.Invoke();
    }
}
