using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CellComponent : MonoBehaviour {
    private Image _cellMainImage;
    private Image _cellSubImage;
    private Text _cellText;

    private Sprite _cellMainSprite;
    private Sprite _cellSubSprite;

    private int _cellMultiplayerX;
    private float _percentage;

    private bool _hasSub;
    private bool _hasMulti;
    private bool _isCleared;
    private bool _isBonus;

    public void Init(Sprite cellSprite = null, Sprite cellSubSprite = null, int cellMulti = 0, bool hasSub = false,
        bool hasMulti = false, float percen = 0,bool bonus = false) {
        _cellMainSprite = cellSprite;
        _cellSubSprite = cellSubSprite;
        switch (cellMulti) {
            case 1:
                _cellMultiplayerX = Random.Range(2, 6);
                break;
            case 2:
                _cellMultiplayerX = Random.Range(10, 30);
                break;
            case 3:
                _cellMultiplayerX = Random.Range(50, 100);
                break;
        }

        _hasSub = hasSub;
        _hasMulti = hasMulti;
        _percentage = percen;
        _isBonus = bonus;
        SetupCell();
        UpdateVisual();
    }

    public bool IsBonusItem() {
        return _isBonus;
    }
    public bool isCleared() => _isCleared;
    public bool SetCleared(bool isCleared) => _isCleared = isCleared;
    public bool GetHasMulti() => _hasMulti;
    public Transform GetMultiplayerText() {
        return _cellText.transform;
    }
    public int GetMultiplayerValue() => _cellMultiplayerX;
    public float GetPercentage() => _percentage;
    public void DestroyMe() {
        transform.DOScale(Vector3.zero, 0.25f).OnComplete(() => {
            Destroy(gameObject);
        });
    }
    public Sprite GetCellSprite() {
        if (_hasSub)
            return _cellSubSprite;
        return _cellMainSprite;
    }
    private void UpdateVisual() {
        _cellMainImage.sprite = _cellMainSprite;
        
        if (_hasSub) {
            _cellSubImage.sprite = _cellSubSprite;
        }
        else {
            _cellSubImage.gameObject.SetActive(false);
        }

        if (_hasMulti) {
            _cellText.text = "x" + _cellMultiplayerX.ToString();
        }
        else {
            _cellText.gameObject.SetActive(false);
        }
    }
    private void SetupCell() {
        _cellMainImage = GetComponent<Image>();
        _cellSubImage = transform.GetChild(0).GetComponent<Image>();
        _cellText = GetComponentInChildren<Text>();
    }
}
