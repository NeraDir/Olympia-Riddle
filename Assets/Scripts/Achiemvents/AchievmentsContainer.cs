using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AchievmentsContainer : MonoBehaviour
{
    private AchievmentsData _achievmentsData;
    
    [SerializeField] private Text _achievmentsText;
    private Button _achievmentsButton;
    [SerializeField] private Image _achievementsBlockPanel;
    [SerializeField] private Image _achievementsProgressBar;
    private AudioClip _clickSound;
    public void Init(AchievmentsData achievementData) {
        _clickSound = Resources.Load<AudioClip>("Sounds/click");
        _achievmentsData = achievementData;
        _achievmentsData.Init();
        _achievmentsData.achievementsDone += UpdateVisual;
        SetupComponents();
        _achievmentsText.text = _achievmentsData.GetAchievementTxt();
        _achievmentsButton.onClick.AddListener(OnClickClearAchievemt);
        UpdateVisual();
    }

    private void OnDestroy() {
        _achievmentsData.achievementsDone -= UpdateVisual;
    }

    private void OnClickClearAchievemt() {
        if(_achievmentsData.GetAchiementIsClear())
            return; 
        SettingsBewteenScenesComponent.playEffect?.Invoke(_clickSound);
        DataContainer.PlayerCurrentPointsCount += _achievmentsData.GetAchievmentPointsValue();
        _achievmentsData.SetAchievetClear();
        _achievmentsButton.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() => {
            UpdateVisual();
        });
        
    }
    
    private void SetupComponents() {
        _achievmentsButton = GetComponentInChildren<Button>();
    }

    private void UpdateVisual() {
        _achievementsBlockPanel.gameObject.SetActive(!_achievmentsData.GetAchievmentIsAchieved());
        if (_achievmentsData.GetAchievmentIsAchieved()) {
            _achievementsProgressBar.fillAmount = 1;
            if (_achievmentsData.GetAchiementIsClear()) {
                _achievmentsButton.gameObject.SetActive(false);
            }
            else {
                _achievmentsButton.gameObject.SetActive(true);
            }
        }
        else {
            _achievementsProgressBar.fillAmount = (_achievmentsData.GetAchievmentProgressValue() / _achievmentsData.GetAchievementMaxProgressValue());
            _achievmentsButton.gameObject.SetActive(false);
        }
        
    }
}
