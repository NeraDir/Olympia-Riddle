using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievmentsManager : MonoBehaviour {
    
    [SerializeField] private AchievmentsContainer _achievmentContainerPrefab;
    [SerializeField] private Transform _achiementsSpawnPOsition;
    [SerializeField] private List<AchievmentsData> _achievmentsDatas;
    
    private void Start() {
        foreach (var item in _achievmentsDatas) {
            AchievmentsContainer newContainer = Instantiate(_achievmentContainerPrefab, _achiementsSpawnPOsition);
            newContainer.Init(item);
        }
    }
}
