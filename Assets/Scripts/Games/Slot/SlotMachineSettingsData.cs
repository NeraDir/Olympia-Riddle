using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Slot Settings", menuName = "Create Slot Settings")]
public class SlotMachineSettingsData : ScriptableObject
{
    public List<SlotCellData> slotCells = new List<SlotCellData>();  
}

[System.Serializable]
public class SlotCellData {
    public Sprite sprite;
    public Sprite subSprite;
    public float percentOfBet;
    public float dropChance;
    public int cellMulti;
    public bool hasMulti;
    public bool hasSub;
    public bool isBonus;
}
