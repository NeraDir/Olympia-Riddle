using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Game Data", menuName = "Game Data")]
public class GameDatas : ScriptableObject
{
    [SerializeField] private string _gameName;
    [SerializeField] private int _gamePrefabIndex;
    [SerializeField] private Sprite _gameSprite;
    [SerializeField] private string _gameOpenSaveKey;
    [SerializeField] private int _gameOpenPrice;
    [SerializeField] private AchievmentsData _achievmentsData;
    [SerializeField] private bool _gameClosed;
    public string GetGameName() => _gameName;
    public int GetGameIndex() => _gamePrefabIndex;
    public bool GetGameClosed() => _gameClosed;
    public Sprite GetGameSprite() => _gameSprite;
    public int GetGameOpenPrice() => _gameOpenPrice;
    public AchievmentsData GetAchievmentsData() => _achievmentsData;
    
    public int GetGameData() {
        return PlayerPrefs.GetInt(_gameOpenSaveKey);
    }
    
    public void SaveGameData(int value) {
        PlayerPrefs.SetInt(_gameOpenSaveKey, value);
    }

    public bool Buy() {
        if (DataContainer.PlayerCurrentPointsCount >= _gameOpenPrice) {
            DataContainer.PlayerCurrentPointsCount -= _gameOpenPrice;
            MneuManager.updatePointsTxt?.Invoke();
            SaveGameData(1);
            return true;
        } 
        Handheld.Vibrate();
        return false;
    }
}
