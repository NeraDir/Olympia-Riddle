using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoPlaySpinsWindow : MonoBehaviour
{
    [SerializeField] private TableMainComponent _tableMainComponent;
    [SerializeField] private Text _spinsAmountText;
    [SerializeField] private Slider _spinsAmountSlider;
    [SerializeField] private UiButtonComponent _uiButtonComponent;
    [SerializeField] private Button _launchButton;
    
    private void Awake() {
        _spinsAmountSlider.value = 10;
        _spinsAmountSlider.maxValue = 300;
        _spinsAmountSlider.minValue = 0;
        _launchButton.onClick.AddListener(OnClickLaunchButton);
        _spinsAmountSlider.onValueChanged.AddListener(OnVlaueChange);
        OnVlaueChange(_spinsAmountSlider.value);
    }

    private void OnVlaueChange(float value) {
        _spinsAmountText.text = value.ToString("0");
    }
    
    private void OnClickLaunchButton() {
        _tableMainComponent.launchAutoSpin?.Invoke((int)_spinsAmountSlider.value);
    }
}
