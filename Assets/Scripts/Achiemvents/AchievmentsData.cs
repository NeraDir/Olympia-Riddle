using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Achievments Data", menuName = "Create new Achievments Data")]
public class AchievmentsData : ScriptableObject {
    [SerializeField] private string achievmetTxt;
    [SerializeField] private string achievmentSaveKey;

    [SerializeField] private float achievmentProgressValue;
    [SerializeField] private float achievementMaxProgressValue;

    [SerializeField] private int achievementGetPointsValue;
    [SerializeField] private AchievmentsData _achievmentsData;
    
    public Action achievementsDone;

    public void Init() {
        CheckAchievmentStatus();
    }
    
    public string GetAchievementTxt() {
        return achievmetTxt;
    }

    public string GetAchievmentSaveKey() {
        return achievmentSaveKey;
    }

    public float GetAchievmentProgressValue() {
        return achievmentProgressValue;
    }

    public float GetAchievementMaxProgressValue() {
        return achievementMaxProgressValue;
    }
    
    public int GetAchievmentPointsValue() {
        return achievementGetPointsValue;
    }
    
    public void UpdateAchievementsProgress(float value) {
        if (GetAchievmentIsAchieved()) {
            return;
        }
        else {
            achievmentProgressValue += value;
            if (_achievmentsData != null) {
                _achievmentsData.UpdateAchievementsProgress(1000);
            }
            PlayerPrefs.SetFloat($"{GetAchievmentSaveKey()}value", achievmentProgressValue);
            CheckAchievmentStatus();
        }
    }

    private void CheckAchievmentStatus() {
        if (achievmentProgressValue >= achievementMaxProgressValue) {
            PlayerPrefs.SetInt(GetAchievmentSaveKey(), 1);
            achievementsDone?.Invoke();
        }
    }

    public void SetAchievetClear() {
        PlayerPrefs.SetInt($"{GetAchievmentSaveKey()}Cleared",1);
    }

    public bool GetAchiementIsClear() {
        if (PlayerPrefs.GetInt($"{GetAchievmentSaveKey()}Cleared") == 1)
            return true;
        return false;
    }

    public bool GetAchievmentIsAchieved() {
        if (PlayerPrefs.HasKey(GetAchievmentSaveKey()))
            return true;
        return false;
    }
}
