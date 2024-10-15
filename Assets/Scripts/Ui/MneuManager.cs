using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MneuManager : MonoBehaviour {
    public static Action updateShopDatas;
    public static Action updatePointsTxt;

    [SerializeField] private GameObject _welcomePage;
    [SerializeField] private GameObject _menuPage;

    [SerializeField] private ShopData _shopData;
    [SerializeField] private ShopContainer _shopContainer;
    [SerializeField] private Transform _shopContentSpawnPlace;

    [SerializeField] private Text[] _currentPlayerPointsDispalyTxt;

    private List<ShopContainer> _shopContainers = new List<ShopContainer>();

    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;

    [Space(10)] 
    [Header("Achievements")] 
    [SerializeField] private AchievmentsData _firstBg;
    [SerializeField] private AchievmentsData _fourthBg;
    [SerializeField] private AchievmentsData _lastBg;
    
    private void Awake() {
        SetupShop();
        if (DataContainer.PlayerFirstEnter == 0) {
            _welcomePage.SetActive(true);
            _menuPage.SetActive(false);
            _shopContainers[0].PreBuy();
        }
        _musicVolumeSlider.value = DataContainer.MusicVolume;
        _sfxVolumeSlider.value = DataContainer.SfxVolume;
        _musicVolumeSlider.maxValue = 1;
        _sfxVolumeSlider.maxValue = 1;
        updatePointsTxt += OnUpdatePoints;
        updateShopDatas += OnUpdateShop;
        updatePointsTxt?.Invoke();
    }

    public Slider GetMusicVolumeSlider() {
        return _musicVolumeSlider;
    }

    public Slider GetSfxVolumeSlider() {
        return _sfxVolumeSlider;
    }
    
    private void OnDestroy() {
        updateShopDatas -= OnUpdateShop;
        updatePointsTxt -= OnUpdatePoints;
    }

    private void OnUpdatePoints() {
        foreach (var item in _currentPlayerPointsDispalyTxt) {
            item.text = $"POINT BALANCE\n{DataContainer.PlayerCurrentPointsCount}";
        }
    }
    
    private void OnUpdateShop() {
        foreach (var item in _shopContainers) {
            item.OnUpdateVisualOfContent();
        }
    }

    private void SetupShop() {
        for (int i = 0; i < _shopData.shopDatasList.Count; i++) {
            if (i == 0) {
                ShopContainer newShopContainer = Instantiate(_shopContainer, _shopContentSpawnPlace);   
                newShopContainer.SetData(_shopData.shopDatasList[i].sprite,_shopData.shopDatasList[i].price,_firstBg);
                _shopContainers.Add(newShopContainer);
            }
            else if (i == 3) {
                ShopContainer newShopContainer = Instantiate(_shopContainer, _shopContentSpawnPlace);   
                newShopContainer.SetData(_shopData.shopDatasList[i].sprite,_shopData.shopDatasList[i].price,_fourthBg);
                _shopContainers.Add(newShopContainer);
            }
            else if (i == _shopData.shopDatasList.Count - 1) {
                ShopContainer newShopContainer = Instantiate(_shopContainer, _shopContentSpawnPlace);   
                newShopContainer.SetData(_shopData.shopDatasList[i].sprite,_shopData.shopDatasList[i].price,_lastBg);
                _shopContainers.Add(newShopContainer);
            }
            else {
                ShopContainer newShopContainer = Instantiate(_shopContainer, _shopContentSpawnPlace);   
                newShopContainer.SetData(_shopData.shopDatasList[i].sprite,_shopData.shopDatasList[i].price);
                _shopContainers.Add(newShopContainer);
            }
        }
    }
}
