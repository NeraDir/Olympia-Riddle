using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ButtonType {
    BetweenPage,
    Exit,
    Play,
    Menu,
    Scene,
}

public class UiButtonComponent : MonoBehaviour,IPointerClickHandler {
    [SerializeField] private Animator _pageClose;
    [SerializeField] private Animator _pageOpen;
    [SerializeField] private ButtonType _buttonType;
    [SerializeField] private string _sceneName;    
    
    private AudioClip _clickSound;
    public static bool isClicked;
    public Action _action;
    
    private void Start() {
        _clickSound = Resources.Load<AudioClip>("Sounds/click");
    }
    
    public void OnPointerClick(PointerEventData eventData) {
        if(isClicked)
            return;
        isClicked = true;
        AnimationButtonClick();
    }

    private IEnumerator Click() {
        SettingsBewteenScenesComponent.playEffect?.Invoke(_clickSound);
        switch (_buttonType) {
            case ButtonType.BetweenPage:
                if(_pageClose != null)
                    _pageClose.SetBool("RiddlePageState", true);
                yield return new WaitForSeconds(0.5f);
                if(_pageOpen != null)
                    _pageOpen.gameObject.SetActive(true);
                if(_pageClose != null)
                    _pageClose.gameObject.SetActive(false);
                break;
            case ButtonType.Exit:
                if(_pageClose != null)
                    _pageClose.SetBool("RiddlePageState", true);
                yield return new WaitForSeconds(0.5f);
                Application.Quit();
                break;  
            case ButtonType.Scene:
                if(_pageClose != null)
                    _pageClose.SetBool("RiddlePageState", true);
                yield return new WaitForSeconds(0.5f);
                SceneManager.LoadScene(_sceneName);
                break;
            case ButtonType.Menu:
                if(_pageClose != null)
                    _pageClose.SetBool("RiddlePageState", true);
                yield return new WaitForSeconds(0.5f);
                Scene nextScene = SceneManager.CreateScene("MountRiddlesMenuScene");
                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.SetActiveScene(nextScene);
                GameObject menuCanvas = Resources.Load("Prefabs/Menu") as GameObject;
                Instantiate(menuCanvas);
                SceneManager.UnloadScene(currentScene);
                break;
            case ButtonType.Play:
                if(_pageClose != null)
                    _pageClose.SetBool("RiddlePageState", true);
                yield return new WaitForSeconds(0.5f);
                SceneManager.LoadScene("Game");
                break;
        }
        if(_action != null)
            _action?.Invoke();
        isClicked = false;
    }

    private void AnimationButtonClick() {
        transform.DOScale(1.1f, 0.1f).OnComplete(() => {
            transform.DOScale(0.8f, 0.1f).OnComplete(() => {
                transform.DOScale(1, 0.05f).OnComplete(() => {
                    StartCoroutine(Click());
                });
            });
        });
    }
}
