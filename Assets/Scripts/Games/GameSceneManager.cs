using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _gamesPrefabs;

    private void Awake() {
        _gamesPrefabs[DataContainer.LastLaunchedGameIndex].SetActive(true);
    }
}
