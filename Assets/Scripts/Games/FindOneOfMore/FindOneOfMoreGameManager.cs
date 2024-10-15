using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FindOneOfMoreGameManager : MonoBehaviour
{
   [SerializeField] private List<OneOfMoreData> _oneOfMoreData;
   [SerializeField] private Transform _targetFrom;
   [SerializeField] private Text _displayTimer;
   [SerializeField] private Image _timerTimer;
   [SerializeField] private GameObject _resultScreen;
   [SerializeField] private Text _resultText;
   [SerializeField] private AchievmentsData _firstMatch;
   
   private Sprite _targetSprite;
   private float _timerValue;
   private float _timerMaxValue = 5;
   private bool _isEnd;
   
   public static Action<Sprite> OnEnd;
   
   private void Awake() {
       _isEnd = false;
       _timerValue = _timerMaxValue;
       int targetIndex = Random.Range(0, _oneOfMoreData.Count);
       FindOneOfMoreContainer[] containers = _targetFrom.GetComponentsInChildren<FindOneOfMoreContainer>();
       for (int i = 0; i < containers.Length; i++) {
           containers[i].Init(_oneOfMoreData[targetIndex].otherSprite);
       }
       containers[Random.Range(0, containers.Length)].Init(_oneOfMoreData[targetIndex].targetSprite);
       _targetSprite = _oneOfMoreData[targetIndex].targetSprite;
       OnEnd += OnEndich;
   }

   private void OnDestroy() {
       OnEnd -= OnEnd;
   }

   private void OnEndich(Sprite sprite) {
       if (sprite == _targetSprite) {
           _isEnd = true;
           _resultScreen.SetActive(true);
           if (!PlayerPrefs.HasKey("PlayerRiddleREACHFIRSTMATCrfHSAVEKEY")) {
               if (_firstMatch != null) {
                   _firstMatch.UpdateAchievementsProgress(10);
                   PlayerPrefs.SetInt("PlayerRiddleREACHFIRSTMATCrfHSAVEKEY",1);
               }
           }
           _resultText.text = "YOU GET +3500 POINTS";
       }
       else {
           _resultScreen.SetActive(true);
           _resultText.text = "YOU LOOSE";
           _isEnd = true;
       }
   }
   
   private void LateUpdate() {
       if(_isEnd)
           return;
       _timerValue -= Time.deltaTime;
       _displayTimer.text = _timerValue.ToString("0.0")+"s";
       _timerTimer.fillAmount = Mathf.Lerp(_timerTimer.fillAmount, _timerValue/ _timerMaxValue, 11 * Time.deltaTime);
       if (_timerValue <= 0) {
           _resultScreen.SetActive(true);
           _resultText.text = "YOU LOOSE";
           _isEnd = true;
       }
   }
   
   public void OnClickGetPoints() {
       if(_resultText.text == "YOU GET +3500 POINTS")
           DataContainer.PlayerCurrentPointsCount += 3500;
   }
}

[System.Serializable]
public class OneOfMoreData {
    public Sprite targetSprite;
    public Sprite otherSprite;
}