using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BonusType {
    Instant,
    FreeSpins,
}

public class TableMainComponent : MonoBehaviour {
    [SerializeField] private SlotMachineSettingsData _slotData;
    [SerializeField] private CellComponent _cellPrefab;
    [SerializeField] private LiveFallContainer _liveContainerPrefab;
    [SerializeField] private Transform _multiplayerPlace;
    [SerializeField] private GameObject _totalMultiplayerPlace;
    [SerializeField] private GameObject _freeSpinsPage;
    [SerializeField] private GameObject _instantPage;
    [SerializeField] private List<int> _betsList = new List<int>();
    [SerializeField] private GameObject _spinsCountShow;

    [Space(5)] [Header("TABLE TXT")] [SerializeField]
    private Text _betAmountText;

    [SerializeField] private Text _spinsCountText;
    [SerializeField] private Text _freeSpinsCountText;

    [SerializeField] private Text _playerPointsBalanceText;
    [SerializeField] private Text _playerWinText;
    [SerializeField] private Text _totalMultiplayerText;

    [Space(5)] [Header("TABLE BUTTONS")] [SerializeField]
    private Button _spinButton;

    [SerializeField] private Button _freeSpinsLaunchButton;
    [SerializeField] private Button _instantLaunchButton;

    [Space(10)] [Header("ACHIEVEMTNTS")] 
    [SerializeField] private AchievmentsData _fistSpin;
    [SerializeField] private AchievmentsData _first10000Win;
    [SerializeField] private AchievmentsData _first100000Win;
    [SerializeField] private AchievmentsData _first1000000Win;
    [SerializeField] private AchievmentsData _first250000Win;
    [SerializeField] private AchievmentsData _first50xWin;
    
    [SerializeField] private Button _plusBet;
    [SerializeField] private Button _minusBet;

    private List<LineComponent> _tableLines = new List<LineComponent>();

    private List<CellComponent> _multiCells = new List<CellComponent>();

    private List<CellComponent> _bonusCells = new List<CellComponent>();

    private LineComponent _lastLine;
    private LiveFallManager _liveFallManager;

    public Action onClickSpin;
    public Action onSpinFinish;
    public Action canSpin;
    public Action<int> launchAutoSpin;
    public Action<BonusType, int, int> onBuyBonus;

    private int _betAmount;
    private int _winAmount;
    private int _totalMultiplayer;
    private int _freeSpins;
    private int _betIndex;
    private int _autoSpinAmount;

    private bool _canSpin;
    private bool _isAutoSpin;
    private bool _isFreeSpins;

    private List<WinCellsStruct> _winCellsStruct = new List<WinCellsStruct>();

    [Space(10)] [Header("Prizes Data")] [SerializeField]
    private FreeSpinsData _instantPrizesData;

    [SerializeField] private FreeSpinsData _freeSpinData;

    private void Awake() {
        _betAmount = _betsList[0];
        _canSpin = true;
        SetupSlot();
        onClickSpin += Spin;
        canSpin += CanSpin;
        onSpinFinish += OnSpinFinished;
        onBuyBonus += LaunchBonus;
        launchAutoSpin += AutoSpin;
        OnBetChanged();
        OnBlanceChanged();
        OnWinsChanged();
    }

    private void OnDestroy() {
        onClickSpin -= Spin;
        launchAutoSpin -= AutoSpin;
        canSpin -= CanSpin;
        onSpinFinish -= OnSpinFinished;
        onBuyBonus -= LaunchBonus;
    }

    public int GetBetAmount() => _betAmount;
    private void CanSpin() => _canSpin = !_canSpin;

    private void SetupSlot() {
        _spinButton.onClick.AddListener(OnClickSpin);
        _freeSpinsLaunchButton.onClick.AddListener(() => FreeSpin(_freeSpins));
        _liveFallManager = GetComponentInChildren<LiveFallManager>();
        _tableLines = GetComponentsInChildren<LineComponent>().ToList();
        _plusBet.onClick.AddListener(() => OnChangeBet(1));
        _minusBet.onClick.AddListener(() => OnChangeBet(-1));
        _liveFallManager.Init(_liveContainerPrefab);
    }

    private void OnChangeBet(int value) {
        if (_isAutoSpin)
            return;
        if (!_canSpin)
            return;
        if (_betIndex + value >= _betsList.Count) {
            return;
        }

        if (_betIndex + value <= -1) {
            return;
        }

        _betIndex += value;
        _betAmount = _betsList[_betIndex];
        OnBetChanged();
    }

    private void AutoSpin(int spinsCount) {
        _autoSpinAmount = spinsCount;
        _spinsCountShow.SetActive(true);
        _freeSpinsCountText.text = _autoSpinAmount.ToString();
        StartCoroutine(AutoSpinCoroutine("AUTO SPINS LEFT"));
    }

    private void FreeSpin(int spinsCount) {
        _autoSpinAmount = spinsCount;
        _spinsCountShow.SetActive(true);
        _freeSpinsCountText.text = _autoSpinAmount.ToString();
        StartCoroutine(AutoSpinCoroutine("FREE SPINS LEFT"));
    }

    private IEnumerator AutoSpinCoroutine(string spinsTxt) {
        while (_autoSpinAmount > 0) {
            _isAutoSpin = true;
            if (_isFreeSpins) {
                _totalMultiplayerPlace.SetActive(true);
                _totalMultiplayerText.text = "x" + _totalMultiplayer.ToString();
            }

            if (DataContainer.PlayerCurrentPointsCount <= 0) {
                _autoSpinAmount = 0;
            }

            OnClickSpin();
            _spinsCountText.text = spinsTxt;
            UiButtonComponent.isClicked = true;
            _freeSpinsCountText.text = _autoSpinAmount.ToString();
            yield return null;
        }

        if (_isFreeSpins) {
            _totalMultiplayerPlace.SetActive(false);
            _totalMultiplayerText.text = "x" + _totalMultiplayer.ToString();
        }

        _totalMultiplayer = 1;
        UiButtonComponent.isClicked = false;
        _spinsCountShow.SetActive(false);
        _spinsCountText.text = spinsTxt;
        _freeSpinsCountText.text = _autoSpinAmount.ToString();
        _isAutoSpin = false;
        _isFreeSpins = false;
    }

    private void OnBetChanged() {
        _betAmountText.text = _betAmount.ToString();
    }

    private void OnBlanceChanged() {
        _playerPointsBalanceText.text = DataContainer.PlayerCurrentPointsCount.ToString();
    }

    private void OnWinsChanged() {
        _playerWinText.text = _winAmount.ToString();
    }

    private void OnSpinFinished() {
        StartCoroutine(WaitAndDo(GetWinCells, 1));
    }

    private void GetWinCells() {
        _winCellsStruct.Clear();

        foreach (var item in _tableLines) {
            foreach (var item2 in item.GetCellComponents()) {
                if (item2.IsBonusItem()) {
                    _bonusCells.Add(item2);
                }

                if (item2.GetHasMulti() && !_multiCells.Contains(item2)) {
                    _multiCells.Add(item2);
                }
                else {
                    WinCellsStruct findSprite = _winCellsStruct.Find(x => x.cellSprite == item2.GetCellSprite());
                    if (findSprite != null) {
                        findSprite.cellsCount += 1;
                        findSprite.howMuchToGet += (int)((_betAmount / item2.GetPercentage()) / 4);
                    }
                    else {
                        WinCellsStruct tempWin = new WinCellsStruct();
                        tempWin.cellSprite = item2.GetCellSprite();
                        tempWin.cellsCount = 1;
                        tempWin.percent = item2.GetPercentage();
                        tempWin.howMuchToGet += (int)((_betAmount / item2.GetPercentage()) / 4);
                        _winCellsStruct.Add(tempWin);
                    }
                }
            }
        }

        foreach (var item2 in _winCellsStruct) {
            if (item2.cellsCount >= 8) {
                foreach (var item in _tableLines) {
                    item.DestroyCell(item2.cellSprite);
                }

                _winAmount += item2.howMuchToGet;
                OnWinsChanged();
                _liveFallManager.AddNewLiveFall(item2);
            }
        }

        if (_winAmount > 0) {
            for (int i = 0; i < _multiCells.Count; i++) {
                if (!_multiCells[i].isCleared()) {
                    _multiCells[i].GetMultiplayerText().DOMove(_multiplayerPlace.position, 0.15f).OnComplete(() => {
                        _multiCells[i].GetMultiplayerText().DOScale(Vector3.zero, 0.15f);
                    });
                    _totalMultiplayer += _multiCells[i].GetMultiplayerValue();
                    if (_totalMultiplayer > 0) {
                        _multiplayerPlace.GetComponent<Text>().text = "x" + _totalMultiplayer.ToString();
                    }

                    _multiCells[i].SetCleared(true);
                }
            }
        }

        if (_winCellsStruct.Find(x => x.cellsCount >= 8) != null) {
            for (int i = 0; i < _tableLines.Count; i++) {
                _tableLines[i]._isBonus = false;
                _tableLines[i].Init(this, _cellPrefab, _slotData, false);
            }

            Invoke(nameof(OnFinisher), 0.25f);
            return;
        }
        else {
            if (_bonusCells.Count == 4) {
                _freeSpinsPage.SetActive(true);
                _bonusCells.Clear();
            }
            else if (_bonusCells.Count == 3) {
                _instantPage.SetActive(true);
                _bonusCells.Clear();
            }

            if (_multiCells.Count > 0 && _isFreeSpins) {
                _winAmount *= _totalMultiplayer;
            }
            else {
                _winAmount *= _totalMultiplayer;
            }

            OnWinsChanged();
            if (_first50xWin != null) {
                _first50xWin.UpdateAchievementsProgress(_totalMultiplayer);
            }
            if (_first1000000Win != null) {
                _first1000000Win.UpdateAchievementsProgress(_winAmount);
            }
            if (_first250000Win != null) {
                _first250000Win.UpdateAchievementsProgress(_winAmount);
            }
            if (_first100000Win != null) {
                _first100000Win.UpdateAchievementsProgress(_winAmount);
            }
            if (_first10000Win != null) {
                _first10000Win.UpdateAchievementsProgress(_winAmount);
            }

            DataContainer.PlayerCurrentPointsCount += _winAmount;
            OnBlanceChanged();
            UiButtonComponent.isClicked = false;
            canSpin?.Invoke();
        }
    }

    private void LaunchBonus(BonusType type = BonusType.FreeSpins, int spinsAmount = 10, int multiplier = 1) {
        switch (type) {
            case BonusType.Instant:
                _freeSpins = 1;
                StartCoroutine(GetBonus(_instantPrizesData.lineData));
                break;
            case BonusType.FreeSpins:
                _freeSpins = spinsAmount;
                _totalMultiplayer = multiplier;
                _isFreeSpins = true;
                StartCoroutine(GetBonus(_freeSpinData.lineData));
                break;
        }
    }

    private IEnumerator GetBonus(List<LineData> datas) {
        OnBlanceChanged();
        _canSpin = false;
        if (!_isFreeSpins) {
            _totalMultiplayer = 1;
        }

        _bonusCells.Clear();
        _multiplayerPlace.GetComponent<Text>().text = "";
        _winAmount = 0;
        OnWinsChanged();
        _multiCells.Clear();
        UiButtonComponent.isClicked = true;
        _liveFallManager.ClearLiveFall();
        for (int i = 0; i < _tableLines.Count; i++) {
            if (_tableLines[i].GetCellComponents().Count > 0) {
                _tableLines[i].needCellsData = datas[i].cellDatas;
                _tableLines[i]._isBonus = true;
                _tableLines[i].ClearLine();
                yield return new WaitForSeconds(0.45f);
                _tableLines[i].Init(this, _cellPrefab, _slotData, false);
                yield return new WaitForSeconds(0.05f);
            }
            else {
                _tableLines[i]._isBonus = true;
                _tableLines[i].needCellsData = datas[i].cellDatas;
                _tableLines[i].Init(this, _cellPrefab, _slotData, false);
                yield return new WaitForSeconds(0.05f);
            }
        }

        yield return new WaitForSeconds(0.25f);
        onSpinFinish?.Invoke();
    }

    private void OnFinisher() {
        onSpinFinish?.Invoke();
    }

    private IEnumerator WaitAndDo(Action actio, float delay) {
        yield return new WaitForSeconds(delay);
        actio?.Invoke();
    }

    private void Spin() {
        StartCoroutine(Spining());
    }

    private IEnumerator Spining() {
        for (int i = 0; i < _tableLines.Count; i++) {
            if (_tableLines[i].GetCellComponents().Count > 0) {
                _tableLines[i].ClearLine();
                _tableLines[i]._isBonus = false;
                yield return new WaitForSeconds(0.45f);
                _tableLines[i].Init(this, _cellPrefab, _slotData, false);
                yield return new WaitForSeconds(0.05f);
            }
            else {
                _tableLines[i]._isBonus = false;
                _tableLines[i].Init(this, _cellPrefab, _slotData, false);
                yield return new WaitForSeconds(0.05f);
            }
        }

        yield return new WaitForSeconds(0.25f);
        onSpinFinish?.Invoke();
    }

    private void OnClickSpin() {
        if (!_canSpin)
            return;
        if (DataContainer.PlayerCurrentPointsCount < _betAmount && _isAutoSpin) {
            _canSpin = true;
            return;
        }

        if (DataContainer.PlayerCurrentPointsCount < _betAmount)
            return;
        if (!_isFreeSpins) {
            _totalMultiplayer = 1;
        }

        if (!PlayerPrefs.HasKey("FIRSTSPININGRIDDLESAVEKEY")) {
            _fistSpin.UpdateAchievementsProgress(1);
            PlayerPrefs.SetInt("FIRSTSPININGRIDDLESAVEKEY", 1);
        }

        _bonusCells.Clear();
        _multiplayerPlace.GetComponent<Text>().text = "";
        _winAmount = 0;
        OnWinsChanged();
        _multiCells.Clear();
        UiButtonComponent.isClicked = true;
        _liveFallManager.ClearLiveFall();
        if (!_isFreeSpins) {
            DataContainer.PlayerCurrentPointsCount -= _betAmount;
        }

        if (_isAutoSpin) {
            _autoSpinAmount--;
        }

        OnBlanceChanged();
        canSpin?.Invoke();
        onClickSpin?.Invoke();
    }
}

[System.Serializable]
public class WinCellsStruct {
    public Sprite cellSprite;
    public int cellsCount;
    public float percent;
    public int howMuchToGet;
}