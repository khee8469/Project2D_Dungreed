using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Equipment, Assistant, Accessory, Etc }
[CreateAssetMenu(fileName = "ItemData", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemInfo[] items;
}


[Serializable]
public class ItemInfo
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
}
