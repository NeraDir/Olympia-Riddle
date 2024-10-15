using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWindowManager : MonoBehaviour
{
    [SerializeField] private List<GameDatas> _gameDatas = new List<GameDatas>();
    [SerializeField] private GameContainer _gameContainerPrefab;
    [SerializeField] private Transform _containersSpawnPosition;
    [SerializeField] private Animator _gameAniamtor;
    
    private void Start() {
        foreach (var item in _gameDatas) {
            GameContainer tempContainer = Instantiate(_gameContainerPrefab, _containersSpawnPosition);
            tempContainer.Init(item,_gameAniamtor);
        }

        _gameDatas[0].Buy();
        _gameDatas[1].Buy();
    }
}
