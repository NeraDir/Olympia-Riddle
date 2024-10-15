using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FindMatchOfCardsManager : MonoBehaviour
{
    [SerializeField] private Text _timerTxt;
    [SerializeField] private Image _timer;
    [SerializeField] private GameObject[] _boards;
    [SerializeField] private GameObject _resultPage;
    [SerializeField] private Text _resultTxt;
    [SerializeField] private AchievmentsData _firstMatch;
    [SerializeField] private List<Sprite> _riddleCardsSpritesPool;
    
    private float _timerMaxValue = 30;
    private float _timerValue = 0;
    private int _boardIndex = 0;
    private bool _riddleIsGameOver = false;
    
    private List<CardComponent> riddleCardsPool = new List<CardComponent>();

    public static Action<CardComponent, CardComponent> onCheckCards;
    public static bool RiddlefindMatchGameStarted;

    private void Awake() {
        RiddlefindMatchGameStarted = false;
        riddleCardsPool.Clear();
        _riddleIsGameOver = false;
        _boardIndex = 0;
        _timerValue = 0;
        _timerValue = _timerMaxValue;
        _boardIndex = Random.Range(0, _boards.Length);
        _boards[_boardIndex].SetActive(true);
        CardComponent.currentCardsPool.Clear();
        onCheckCards += CheckCards;
        StartCoroutine(SetupCurrentBoard());
    }

    private void OnDestroy() {
        onCheckCards -= CheckCards;
    }

    private void CheckCards(CardComponent firstCard, CardComponent secondCard) {
        StartCoroutine(Checking(firstCard, secondCard));
    }

    private IEnumerator Checking(CardComponent firstCard, CardComponent secondCard) {
        yield return new WaitForSeconds(0.5f);
        if (firstCard.myCardSprite.name == secondCard.myCardSprite.name)
        {
            if (!PlayerPrefs.HasKey("PlayerRiddleREACHFIRSTMATCHSAVEKEY")) {
                if (_firstMatch != null) {
                    _firstMatch.UpdateAchievementsProgress(10);
                    PlayerPrefs.SetInt("PlayerRiddleREACHFIRSTMATCHSAVEKEY",1);
                }
            }
            riddleCardsPool.Remove(firstCard);
            riddleCardsPool.Remove(secondCard);
            CardComponent.currentCardsPool.Clear();
            if (riddleCardsPool.Count <= 0)
            {
               yield return new WaitForSeconds(0.5f);
               _resultTxt.text = "YOU GET +1000 POINTS";
               _resultPage.SetActive(true);
               RiddlefindMatchGameStarted = false;
               _riddleIsGameOver = true;
            }
            CardComponent.canClick = false;
        }
        else
        {
            firstCard.SetDefaultCardFace();
            secondCard.SetDefaultCardFace();
            CardComponent.canClick = false;
        }
    }

    private void LateUpdate() {
        if(_riddleIsGameOver)
            return;
        _timerValue -= Time.deltaTime;
        _timer.fillAmount = Mathf.Lerp(_timer.fillAmount,(_timerValue / _timerMaxValue),10 * Time.deltaTime);
        _timerTxt.text = _timerValue.ToString("0.0") + "s";
        if (_timerValue <= 0) {
            _resultTxt.text = "YOU LOOSE";
            _resultPage.SetActive(true);
            RiddlefindMatchGameStarted = false;
            _riddleIsGameOver = true;
        }
    }

    private IEnumerator SetupCurrentBoard() {
        List<CardComponent> tempCards = new List<CardComponent>();
        foreach (var item in _boards[_boardIndex].GetComponentsInChildren<CardComponent>())
        {
            tempCards.Add(item);
        }
        int currentRiddleCardsSettedCount = 0;
        int riddleCardsSelecetedIndex = Random.Range(0, _riddleCardsSpritesPool.Count);
        while (tempCards.Count > 0)
        {
            if (currentRiddleCardsSettedCount < 2)
            {
                CardComponent card = tempCards[Random.Range(0, tempCards.Count)];
                if (card != null)
                {
                    if (card.myCardSprite == null)
                    {
                        card.Init(_riddleCardsSpritesPool[riddleCardsSelecetedIndex]);
                        tempCards.Remove(card);
                        riddleCardsPool.Add(card);
                        currentRiddleCardsSettedCount++;
                    }
                }
            }
            else
            {
                _riddleCardsSpritesPool.RemoveAt(riddleCardsSelecetedIndex);
                riddleCardsSelecetedIndex = Random.Range(0, _riddleCardsSpritesPool.Count);
                currentRiddleCardsSettedCount = 0;
            }
            yield return null;
        }
        StartCoroutine(LaunchGame(riddleCardsPool));
    }
    
    private IEnumerator LaunchGame(List<CardComponent> cards)
    {
        foreach (var item in cards)
        {
            item.SetShowCardFace();
        }
        yield return new WaitForSeconds(1);

        foreach (var item in cards)
        {
            item.SetDefaultCardFace();
        }

        _riddleIsGameOver = false;
        RiddlefindMatchGameStarted = true;
    }

    public void OnClickGetPoints() {
        if(_resultTxt.text == "YOU GET +1000 POINTS")
            DataContainer.PlayerCurrentPointsCount += 1000;
    }
}
