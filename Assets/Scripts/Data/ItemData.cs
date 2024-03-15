using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Equipment, accessory, Etc }
[CreateAssetMenu(fileName = "ItemData", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemInfo[] items;
}

//public enum SlotState { Blank, Fill }
[Serializable]
public class ItemInfo
{
    //public SlotState slotState;
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
}
