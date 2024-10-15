using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineComponent : MonoBehaviour {
    private CellComponent _cellComponentPrefab;
    private TableMainComponent _mainTableComponent;
    private SlotMachineSettingsData _slotCellData;
    private EdgeCollider2D _edgeCollider2D;

    private int _maxCellsCount = 5;
    private float _ySpace = 2f;

    private bool isLaster;
    public bool _isBonus;

    private List<CellComponent> _cellComponents = new List<CellComponent>();
    public List<SlotCellData> needCellsData = new List<SlotCellData>();

    public void Init(TableMainComponent mainTableComponent, CellComponent cellComponentPrefab,
        SlotMachineSettingsData slotcellData, bool isLast) {
        SetupLineData();
        _mainTableComponent = mainTableComponent;
        _slotCellData = slotcellData;
        _cellComponentPrefab = cellComponentPrefab;
        isLaster = isLast == true ? true : false;
        FillLine();
        mainTableComponent.onClickSpin += FillLine;
    }

    private void SetupLineData() {
        _edgeCollider2D = GetComponent<EdgeCollider2D>();
        _edgeCollider2D.enabled = true;
    }

    private int GetRandomCellData() {
        if (_slotCellData.slotCells == null || _slotCellData.slotCells.Count == 0) return -1;

        float w;
        float t = 0;

        for (int i = 0; i < _slotCellData.slotCells.Count; i++) {
            t += _slotCellData.slotCells[i].dropChance;
        }

        float r = Random.value;
        float s = 0f;

        for (int i = 0; i < _slotCellData.slotCells.Count; i++) {
            w = _slotCellData.slotCells[i].dropChance;
            s += w / t;
            if (s >= r) return i;
        }

        return -1;
    }

    private void FillLine() {
        if (_isBonus) {
            SlotCellData needCell = _slotCellData.slotCells[Random.Range(0, _slotCellData.slotCells.Count-4)];
            for (int i = _cellComponents.Count; i < _maxCellsCount; i++) {
                SpawnCell(false,needCellsData[i],needCell);
            }
        }
        else {
            for (int i = _cellComponents.Count; i < _maxCellsCount; i++) {
                SpawnCell(false);
            }
        }
    }

    public void ClearLine() {
        _edgeCollider2D.enabled = false;
        for (int i = 0; i < _cellComponents.Count; i++) {
            Destroy(_cellComponents[i].gameObject, 0.5f);
        }

        _cellComponents.Clear();
    }

    private void SpawnCell(bool lastCell = false, SlotCellData slotCellData = null, SlotCellData targetCellData = null) {
        SlotCellData tempCellData = slotCellData != null ? slotCellData : _slotCellData.slotCells[GetRandomCellData()];
        if (_cellComponents.Count <= 0) {
            CellComponent tempcell = Instantiate(_cellComponentPrefab,
                new Vector3(transform.position.x, transform.position.y + _ySpace, transform.position.z),
                Quaternion.identity, transform);
            tempcell.Init(tempCellData.sprite, tempCellData.subSprite, tempCellData.cellMulti, tempCellData.hasSub,
                tempCellData.hasMulti, tempCellData.percentOfBet,tempCellData.isBonus);
            _cellComponents.Add(tempcell);
        }
        else {
            CellComponent tempcell = Instantiate(_cellComponentPrefab,
                new Vector3(transform.position.x,
                    _cellComponents[_cellComponents.Count - 1].transform.position.y + _ySpace, transform.position.z),
                Quaternion.identity, transform);
            tempcell.Init(tempCellData.sprite, tempCellData.subSprite,
                tempCellData.cellMulti, tempCellData.hasSub, tempCellData.hasMulti, tempCellData.percentOfBet,tempCellData.isBonus);
            _cellComponents.Add(tempcell);
        }
    }

    public void DestroyCell(Sprite sprite) {
        foreach (var item in _cellComponents.FindAll(x => x.GetCellSprite() == sprite)) {
            item.DestroyMe();
            _cellComponents.Remove(item);
        }
    }

    public List<CellComponent> GetCellComponents() {
        return _cellComponents;
    }
}