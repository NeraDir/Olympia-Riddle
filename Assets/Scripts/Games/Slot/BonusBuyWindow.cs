using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusBuyWindow : MonoBehaviour
{
    [SerializeField] private TableMainComponent _tableMainComponent;

    [Space(10)]
    [Header("BuyCellsTxt")]
    [SerializeField] private Text _instantPrizePriceText;
    [SerializeField] private Text _freeSpinsPriceText;
    [SerializeField] private Text _freeSpinsX2PriceText;
    [SerializeField] private Text _freeSpinsX10PriceText;

    [Space(10)] 
    [Header("BuyCellsBtn")] 
    [SerializeField] private Button _instantPrizeButton;
    [SerializeField] private Button _freeSpinsButton;
    [SerializeField] private Button _freeSpinsX2Button;
    [SerializeField] private Button _freeSpinsX10Button;

    [SerializeField] private AchievmentsData _firstBuyInstant;
    [SerializeField] private AchievmentsData _firstFreeSpins;

    private void Awake() {
        SetBuyWindow();
        UpdateVisual();
    }
    
    private void OnEnable() {
        UpdateVisual();
    }

    private void UpdateVisual() {
        _instantPrizePriceText.text = (_tableMainComponent.GetBetAmount() * 50).ToString();
        _freeSpinsPriceText.text = (_tableMainComponent.GetBetAmount() * 100).ToString();
        _freeSpinsX2PriceText.text = (_tableMainComponent.GetBetAmount() * 250).ToString();
        _freeSpinsX10PriceText.text = (_tableMainComponent.GetBetAmount() * 500).ToString();
        _instantPrizeButton.interactable = DataContainer.PlayerCurrentPointsCount > _tableMainComponent.GetBetAmount() * 50;
        _freeSpinsButton.interactable = DataContainer.PlayerCurrentPointsCount > _tableMainComponent.GetBetAmount() * 100;
        _freeSpinsX2Button.interactable = DataContainer.PlayerCurrentPointsCount > _tableMainComponent.GetBetAmount() * 250;
        _freeSpinsX10Button.interactable = DataContainer.PlayerCurrentPointsCount > _tableMainComponent.GetBetAmount() * 500;
    }
    
    private void SetBuyWindow() {
        _instantPrizeButton.onClick.AddListener(() => BuyBonusPrize(BonusType.Instant,1,1,_tableMainComponent.GetBetAmount() * 50));
        _freeSpinsButton.onClick.AddListener(() => BuyBonusPrize(BonusType.FreeSpins,15,1,_tableMainComponent.GetBetAmount() * 100));
        _freeSpinsX2Button.onClick.AddListener(() => BuyBonusPrize(BonusType.FreeSpins,10,2,_tableMainComponent.GetBetAmount() * 250));
        _freeSpinsX10Button.onClick.AddListener(() => BuyBonusPrize(BonusType.FreeSpins,5,10,_tableMainComponent.GetBetAmount() * 500));
    }

    private void BuyBonusPrize(BonusType type, int amount,int multiplier,int price) {
        if (DataContainer.PlayerCurrentPointsCount - price < 0) {
            return;
        }

        if (type == BonusType.Instant) {
            if (!PlayerPrefs.HasKey("FIRSTBUYINSTANTSAVEKTEITRIFDDLE")) {
                if (_firstBuyInstant != null) {
                    _firstBuyInstant.UpdateAchievementsProgress(1);
                    PlayerPrefs.SetInt("FIRSTBUYINSTANTSAVEKTEITRIFDDLE",1);
                }
            }
        }
        else if (type == BonusType.FreeSpins) {
            if (!PlayerPrefs.HasKey("FIRSTBUYINSTANTSAVFSSGSGGDFDEKTEITRIFDDLE")) {
                if (_firstFreeSpins != null) {
                    _firstFreeSpins.UpdateAchievementsProgress(1);
                    PlayerPrefs.SetInt("FIRSTBUYINSTANTSAVFSSGSGGDFDEKTEITRIFDDLE",1);
                }
            }
        }
        DataContainer.PlayerCurrentPointsCount -= price;
        _tableMainComponent.onBuyBonus?.Invoke(type, amount, multiplier);
    }
}
