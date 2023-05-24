using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthPresenter : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    private RectTransform _healthBarTransform;
    private float _screenHeight;
    private float _widthOnScreen;
    private float _heightOnScreen;

    private void Start()
    {
        _healthBarTransform = _healthBar.GetComponent<RectTransform>();
        _screenHeight = GetComponent<RectTransform>().sizeDelta.y;
        _widthOnScreen = Screen.width / 80;
        _heightOnScreen = 0;
        _healthBarTransform.sizeDelta = new Vector2(_widthOnScreen, _heightOnScreen);
    }

    public void UpdateView(Health health)
    {
        (float currentHP, float maxHP) healthParams = health.GetHealthParams();
        float healthIndex = 1f - healthParams.currentHP / healthParams.maxHP;

        _healthBarTransform.sizeDelta = new Vector2(
                                        _widthOnScreen,
                                        _heightOnScreen + (_screenHeight * healthIndex));
    }
}
