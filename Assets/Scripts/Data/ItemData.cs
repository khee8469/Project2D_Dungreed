using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Data/Item")]  //
public class ItemData : ScriptableObject
{
    //public Item prefab;
    public ItemInfo[] items;


    [Serializable]
    public struct ItemInfo
    {
        public Item Weapon;
        public GameObject effect;
        public float effectPlayTime;
        
        public string name;
        public int damage;
        public float range;
        public float angleRange;
        public float coolTime;
    }
}
