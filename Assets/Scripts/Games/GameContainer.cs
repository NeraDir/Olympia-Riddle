using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameContainer : MonoBehaviour {
    private GameObject _lockPanel;
    [SerializeField] private Text _gameNameTxt;
    [SerializeField] private Text _gameStateTxt;
    private Button _motionButton;
    private Image _image;
    private Animator _gameAnimator;
    private AudioClip _clickSound;
    private GameDatas _myData;

    private bool _isClicked;
    
    public void Init(GameDatas data, Animator animator) {
        _clickSound = Resources.Load<AudioClip>("Sounds/click");
        _myData = data;
        _gameAnimator = animator;
        SetupComponents();
        UpdateVisual();
    }

    private void SetupComponents() {
        _lockPanel = transform.GetChild(0).GetChild(1).gameObject;
        _image = transform.GetChild(0).GetChild(0).transform.GetComponent<Image>();
        _image.sprite = _myData.GetGameSprite();
        _motionButton = GetComponentInChildren<Button>();
        _motionButton.onClick.AddListener(OnClickMotionButton);
    }

    private void OnClickMotionButton() {
        if(_myData.GetGameClosed())
            return;
        SettingsBewteenScenesComponent.playEffect?.Invoke(_clickSound);
        if (_myData.GetGameData() == 0) {
            Buy();
        }
        else {
            LoadGame();
        }
    }

    private void UpdateVisual() {
        _lockPanel.SetActive(_myData.GetGameData() == 1 ? false : true);
        if (_myData.GetGameClosed()) {
            _gameStateTxt.text = "CLOSED";
            _gameNameTxt.text = "COMING SOON!";
            return;
        }
        if (_myData.GetGameData() == 0) {
            _gameStateTxt.text = "BUY";
            _gameNameTxt.text = "POINTS \n" + _myData.GetGameOpenPrice().ToString();
        }
        else {
            _gameNameTxt.text = _myData.GetGameName();
            _gameStateTxt.text = "LOAD";
        }
    }

    private void Buy() {
        bool buying = _myData.Buy();
        if (buying) {
            if (!PlayerPrefs.HasKey($"{_myData.GetGameName()}ACHIEMVENTSBUYAVEKEY")) {
                if (_myData.GetAchievmentsData() != null) {
                    _myData.GetAchievmentsData().UpdateAchievementsProgress(1);
                    PlayerPrefs.SetInt($"{_myData.GetGameName()}ACHIEMVENTSBUYAVEKEY", 1);
                }
            }
            UpdateVisual();
        }
    }

    private void LoadGame() {
        if(_isClicked)
            return;
        _isClicked = true;
        StartCoroutine(Load());
    }

    private IEnumerator Load() {
        if(_gameAnimator != null)
            _gameAnimator.SetBool("RiddlePageState", true);
        if (!PlayerPrefs.HasKey($"{_myData.GetGameName()}ACHIEMVENTSAVEKEY")) {
            if (_myData.GetAchievmentsData() != null) {
                _myData.GetAchievmentsData().UpdateAchievementsProgress(1);
                PlayerPrefs.SetInt($"{_myData.GetGameName()}ACHIEMVENTSAVEKEY", 1);
            }
        }
        yield return new WaitForSeconds(0.5f);
        DataContainer.LastLaunchedGameIndex = _myData.GetGameIndex();
        SceneManager.LoadScene("Game");
    }
}