using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LiveFallContainer : MonoBehaviour {
    [SerializeField] private Text _countTxt;
    [SerializeField] private Text _totalTxt;
    [SerializeField] private Image _fallerImage;
    
    public void Init(int count, int total,Sprite fallerSprite) {
        _countTxt.text = count.ToString();
        _totalTxt.text = total.ToString();
        _fallerImage.sprite = fallerSprite;
        AddMe();
    }

    public void DestroyMe() {
        transform.DOScale(Vector3.zero, 0.25f).OnComplete(() => Destroy(gameObject));
    }

    private void AddMe() {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.25f);
    }
}
