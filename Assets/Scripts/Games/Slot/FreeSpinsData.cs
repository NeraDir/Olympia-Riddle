using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FreeSpinsData", menuName = "FreeSpinsData", order = 1)]
public class FreeSpinsData : ScriptableObject
{
    public List<LineData> lineData;
}

[System.Serializable]
public class LineData {
    public List<SlotCellData> cellDatas;
}
