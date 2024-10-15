using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardComponent : MonoBehaviour, IPointerClickHandler
{
    
    private Image _myImage;

    private bool _isOnPressed;

    public static List<CardComponent> currentCardsPool = new List<CardComponent>();

    public Sprite myCardSprite;

    public Sprite cardBackSprite;

    public static bool canClick;

    public void Init(Sprite card)
    {
        _myImage = GetComponent<Image>();
        _myImage.sprite = cardBackSprite;
        myCardSprite = card;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canClick)
            return;
        if (_isOnPressed)
            return;
        if (!FindMatchOfCardsManager.RiddlefindMatchGameStarted)
            return;
        _isOnPressed = true;
        SetShowCardFace();
        currentCardsPool.Add(this);
        if (currentCardsPool.Count >= 2)
        {
            canClick = true;
            FindMatchOfCardsManager.onCheckCards?.Invoke(currentCardsPool[0], currentCardsPool[1]);
        }
    }

    public void SetShowCardFace()
    {
        transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 180, 0), 0.25f).OnComplete(() => _myImage.sprite = myCardSprite);
    }

    public void SetDefaultCardFace()
    {
        transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.25f).OnComplete(() => { _myImage.sprite = cardBackSprite; _isOnPressed = false; currentCardsPool.Clear(); });
    }
}

