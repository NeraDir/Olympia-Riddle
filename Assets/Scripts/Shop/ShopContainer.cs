using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopContainer : MonoBehaviour {
    [SerializeField] private Image _mySpriteDisplay;
    private Sprite _mySprite;
    [SerializeField] private Text _priceTxt;
    [SerializeField] private Text _selecetTxt;
    private Button _motionButton;
    private int _price;
    private AchievmentsData _achievmentsData;
    private AudioClip _clickSound;
    private int _isContainerBuyed {
        get {
            if (PlayerPrefs.HasKey($"RiddleIsBgBuyed{_mySprite.name}")) {
                return PlayerPrefs.GetInt($"RiddleIsBgBuyed{_mySprite.name}");
            }

            return 0;
        }
        set {
            PlayerPrefs.SetInt($"RiddleIsBgBuyed{_mySprite.name}", value);
        }
    }

    private void Start() {
        _clickSound = Resources.Load<AudioClip>("Sounds/click");
        _motionButton = GetComponentInChildren<Button>();
        MneuManager.updateShopDatas?.Invoke();
        _motionButton.onClick.AddListener(OnClickMotion);
    }

    public void SetData(Sprite sprite = null,int price = 0,AchievmentsData achievmentsData = null) {
        _mySprite = sprite;
        _price = price;
        _mySpriteDisplay.sprite = _mySprite;
        _achievmentsData = achievmentsData;
    }

    public void OnUpdateVisualOfContent() {
        if (_isContainerBuyed != 0) {
            _priceTxt.text = "";
            _selecetTxt.text = DataContainer.BgName == _mySprite.name ? "EQUIPPED" : "EQUIP";
        }
        else {
            _priceTxt.text = _price.ToString();
            _selecetTxt.text = "BUY";
        }
    }

    private void OnClickMotion() {
        if (_isContainerBuyed != 0) {
            SettingsBewteenScenesComponent.playEffect?.Invoke(_clickSound);
            Equip();
        }
        else {
            SettingsBewteenScenesComponent.playEffect?.Invoke(_clickSound);
            Buy();
        }
    }

    public void PreBuy() {
        if (DataContainer.PlayerCurrentPointsCount >= _price) {
            DataContainer.PlayerCurrentPointsCount -= _price;
            _isContainerBuyed = 1;
            MneuManager.updatePointsTxt?.Invoke();
            Equip();
        }
        else {
            Handheld.Vibrate();
        }
    }
    
    private void Buy() {
        if (DataContainer.PlayerCurrentPointsCount >= _price) {
            DataContainer.PlayerCurrentPointsCount -= _price;
            _isContainerBuyed = 1;
            MneuManager.updatePointsTxt?.Invoke();
            if (!PlayerPrefs.HasKey("FIRSTBUYEDBGRIDDLESAVEKEY")) {
                if (_achievmentsData != null) {
                    _achievmentsData.UpdateAchievementsProgress(1);
                    PlayerPrefs.SetInt("FIRSTBUYEDBGRIDDLESAVEKEY", 1);
                }
            }

            if (!PlayerPrefs.HasKey("SHOPFOURTHBGUIDEDBGRIDDLESAVEKEY")) {
                if (_achievmentsData != null) {
                    _achievmentsData.UpdateAchievementsProgress(1);
                    PlayerPrefs.SetInt("SHOPFOURTHBGUIDEDBGRIDDLESAVEKEY",1);  
                }
            }

            if (!PlayerPrefs.HasKey("LASTBGDGIGUGUUGRIDDLESAVEKEY")) {
                if (_achievmentsData != null) {
                    _achievmentsData.UpdateAchievementsProgress(1);
                    PlayerPrefs.SetInt("LASTBGDGIGUGUUGRIDDLESAVEKEY",1);  
                }
            }
            Equip();
        }
        else {
            Handheld.Vibrate();
        }
    }

    private void Equip() {
        DataContainer.BgName = _mySprite.name;
        MneuManager.updateShopDatas?.Invoke();
        SettingsBewteenScenesComponent.changeBg?.Invoke();
    }
}
