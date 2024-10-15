using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FindOneOfMoreContainer : MonoBehaviour, IPointerClickHandler {
    [SerializeField] private Image _displayImage;

    public void Init(Sprite sprite) {
        _displayImage.sprite = sprite;
    }

    public void OnPointerClick(PointerEventData eventData) {
        FindOneOfMoreGameManager.OnEnd?.Invoke(_displayImage.sprite);
    }
}


