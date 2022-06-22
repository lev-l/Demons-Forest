using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPresenter : MonoBehaviour
{
    [SerializeField] private Sprite _bottlesImageTrue, _knifesImageTrue, _torchesImageTrue;
    [SerializeField] private Sprite _bottlesImageFalse, _knifesImageFalse, _torchesImageFalse;
    private Image _healBottles, _throwingKnifes, _staticTorches;
    private int _healBottlesNumber, _throwingKnifesNumber, _staticTorchesNumber;
    private TextMeshProUGUI _bottlesNumberText, _knifesNumberText, _torchesNumberText;

    private void Start()
    {
        _healBottles = transform.GetChild(0).GetComponent<Image>();
        _throwingKnifes = transform.GetChild(1).GetComponent<Image>();
        _staticTorches = transform.GetChild(2).GetComponent<Image>();

        _bottlesNumberText = _healBottles.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _knifesNumberText = _throwingKnifes.GetComponentInChildren<TextMeshProUGUI>();
        _torchesNumberText = _staticTorches.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void AddHealBottle(int bottlesNumber)
    {
        _healBottlesNumber += bottlesNumber;
        UpdateHealBottlesText();
        _healBottles.sprite = _bottlesImageTrue;
    }

    public void RemoveHealBottle()
    {
        _healBottlesNumber--;
        UpdateHealBottlesText();

        if(_healBottlesNumber == 0)
        {
            _healBottles.sprite = _bottlesImageFalse;
        }
    }

    private void UpdateHealBottlesText()
    {
        _bottlesNumberText.text = _healBottlesNumber.ToString();
    }
}