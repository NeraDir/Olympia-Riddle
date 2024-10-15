using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeBonus : MonoBehaviour {
    [SerializeField] private Button _chestButton;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _tapTxt;
    [SerializeField] private Animator _infoPage;
    [SerializeField] private Animator _welcomePage;
[SerializeField] private AchievmentsData _achievmentsData;
    
    private bool _isClicked;
    
    private void Start() {
        _isClicked = true;
        UiButtonComponent.isClicked = true;
        _chestButton.onClick.AddListener(OnChestClick);
    }

    public void OnOpenFinished() {
        _isClicked = false;
    }

    private void OnChestClick() {
        if(_isClicked)
            return;
        _isClicked = true;
        DataContainer.PlayerFirstEnter = 1;
        _chestButton.transform.DORotateQuaternion(Quaternion.Euler(0, 0, -2400), 0.5f);
        _tapTxt.transform.DOMoveY(-10, 0.5f);
        _chestButton.transform.DOScale(0, 0.5f).OnComplete(() => {
            _winPanel.transform.DOScale(Vector3.one, 0.5f).OnComplete(() => {
                StartCoroutine(GoOut());
            });
            _winPanel.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0.5f);
        });
    }

    private IEnumerator GoOut() {
        DataContainer.PlayerCurrentPointsCount += 5000;
        MneuManager.updatePointsTxt?.Invoke();
        yield return new WaitForSeconds(0.5f);
        _welcomePage.SetBool("RiddlePageState",true);
        _achievmentsData.UpdateAchievementsProgress(1);
        yield return new WaitForSeconds(0.5f);
        _infoPage.gameObject.SetActive(true);
        _welcomePage.gameObject.SetActive(false);
        UiButtonComponent.isClicked = false;
    }
}
