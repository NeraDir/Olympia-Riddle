using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop Data", menuName = "Shop Data")]
public class ShopData : ScriptableObject {
    
    [System.Serializable]
    public struct Shop {
        public Sprite sprite;
        public int price;
    }

    public List<Shop> shopDatasList = new List<Shop>();
}


    