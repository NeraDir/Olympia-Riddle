using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyManager : MonoBehaviour {
    
    [SerializeField] private Text[] _dailyStateTxt;
    [SerializeField] private Button _dailyChestButton;
    [SerializeField] private AchievmentsData _dailyAchievemntData;
    
    
    private int _dailyChestPoints = 10000;
    private AudioClip _clickSound;
    private DateTime? _lastOpenTime {
        get {
            if(PlayerPrefs.HasKey("RiddleBonusDateTimeSaveKey"))
                return DateTime.Parse(PlayerPrefs.GetString("RiddleBonusDateTimeSaveKey"));
            return null;
        }
        set {
            PlayerPrefs.SetString("RiddleBonusDateTimeSaveKey", value.ToString());
        }
    }

    private void Awake() {
        _clickSound = Resources.Load<AudioClip>("Sounds/click");
        StartCoroutine(DailyUpdate());
        _dailyChestButton.onClick.AddListener(OnClickOpenChest);
    }

    private IEnumerator DailyUpdate() {
        while (true) {
            UpdateDailyStatus();
            yield return new WaitForSeconds(1);
        }
    }

    private void UpdateDailyStatus() {
        if (_lastOpenTime == null) {
            foreach (var item in _dailyStateTxt) {
                item.text = "OPEN";
            }
            _dailyChestButton.interactable = true;
            return;
        }
        TimeSpan? timeSpan = _lastOpenTime - DateTime.UtcNow;
        foreach (var item in _dailyStateTxt) {
            if (timeSpan.Value.TotalHours <= 0 && timeSpan.Value.Minutes <= 0 && timeSpan.Value.Seconds <= 0 && timeSpan.Value.Milliseconds <= 0) {
                _dailyChestButton.interactable = true;
                item.text = "OPEN";
            }
            else {
                item.text = $"{timeSpan?.Hours:00}:{timeSpan?.Minutes:00}:{timeSpan?.Seconds:00}";
                _dailyChestButton.interactable = false;
            }
        }
    }

    private void OnClickOpenChest() {
        if (!PlayerPrefs.HasKey("FIRSTOPENDAILYBONUSSAVEKEY")) {
            _dailyAchievemntData.UpdateAchievementsProgress(1);
            PlayerPrefs.SetInt("FIRSTOPENDAILYBONUSSAVEKEY", 1);
        }
        SettingsBewteenScenesComponent.playEffect?.Invoke(_clickSound);
        _lastOpenTime = DateTime.UtcNow.AddHours(24);
        DataContainer.PlayerCurrentPointsCount += _dailyChestPoints;
        MneuManager.updatePointsTxt?.Invoke();
    }
}
