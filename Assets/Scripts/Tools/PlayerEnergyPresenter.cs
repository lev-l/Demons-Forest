using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergyPresenter : MonoBehaviour
{
    [SerializeField] private RectTransform _energyBar;
    private float _minHeight;
    private float _maxHeight;
    private Color _originalColor;

    private void Start()
    {
        _maxHeight = GetComponent<RectTransform>().sizeDelta.y;
        _minHeight = 0;
        _originalColor = _energyBar.GetComponent<Image>().color;

        _energyBar.sizeDelta = new Vector2(Screen.width / 80, _minHeight);
    }

    public void UpdateView(int energy, int energyMax)
    {
        Vector2 barSize = _energyBar.sizeDelta;
        float energyMultiplyer = (float)energy / energyMax;

        float barNewHeight = _maxHeight + (_minHeight - _maxHeight) * energyMultiplyer;

        barSize.y = barNewHeight;
        _energyBar.sizeDelta = barSize;
    }

    public void Exhaust()
    {
        _energyBar.GetComponent<Image>().color = new Color(248, 133, 74);
    }

    public void ResumeRegeneration()
    {
        _energyBar.GetComponent<Image>().color = _originalColor;
    }
}
