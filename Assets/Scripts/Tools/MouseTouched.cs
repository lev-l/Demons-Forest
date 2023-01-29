using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseTouched : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _text;
    private ShowOnTouch _textReveal;

    public TextMeshProUGUI Text => _text.GetComponent<TextMeshProUGUI>();

    private void Start()
    {
        _textReveal = GetComponentInParent<ShowOnTouch>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _textReveal.Show(_text);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.SetActive(false);
    }
}
