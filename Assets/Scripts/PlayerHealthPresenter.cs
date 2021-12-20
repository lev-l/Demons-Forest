using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthPresenter : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    private RectTransform _healthBarTransform;
    private float _screenWidth;
    private float _widthOnScreen;
    private float _heightOnScreen;

    private void Start()
    {
        _healthBarTransform = _healthBar.GetComponent<RectTransform>();
        _screenWidth = Screen.width;
        _widthOnScreen = -_screenWidth;
        _heightOnScreen = Screen.height / 50;
        _healthBarTransform.sizeDelta = new Vector2(_widthOnScreen, _heightOnScreen);
    }

    public void UpdateView(Health health)
    {
        (float currentHP, float maxHP) healthParams = health.GetHealthParams();
        float healthIndex = 1f - healthParams.currentHP / healthParams.maxHP;

        _healthBarTransform.sizeDelta = new Vector2(
                                        _widthOnScreen + (_screenWidth * healthIndex),
                                        _heightOnScreen);
    }
}
