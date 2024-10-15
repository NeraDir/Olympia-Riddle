using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveFallManager : MonoBehaviour {
    private Transform _placeToSpawn;
    private LiveFallContainer _liveFallContainerPrefab;

    private List<LiveFallContainer> _liveFallContainers = new List<LiveFallContainer>();
    
    public void Init(LiveFallContainer container) {
        _liveFallContainerPrefab = container;
        _placeToSpawn = transform.GetChild(0).transform;
    }

    public void AddNewLiveFall(WinCellsStruct cellStruct) {
        if (_liveFallContainers.Count >= 5) {
            _liveFallContainers[0].DestroyMe();
            _liveFallContainers.RemoveAt(0);
            LiveFallContainer newLiveFall = Instantiate(_liveFallContainerPrefab, _placeToSpawn);
            newLiveFall.Init(cellStruct.cellsCount,cellStruct.howMuchToGet,cellStruct.cellSprite);
            _liveFallContainers.Add(newLiveFall);
        }
        else {
            LiveFallContainer newLiveFall = Instantiate(_liveFallContainerPrefab, _placeToSpawn);
            newLiveFall.Init(cellStruct.cellsCount,cellStruct.howMuchToGet,cellStruct.cellSprite);
            _liveFallContainers.Add(newLiveFall);
        }
    }

    public void ClearLiveFall() {
        foreach (var item in _liveFallContainers) {
            item.DestroyMe();
        }
        _liveFallContainers.Clear();
    }
}
