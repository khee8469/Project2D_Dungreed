using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Equipment, Assistant, Accessory, Etc , Null }
[CreateAssetMenu(fileName = "ItemData", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemInfo itemInfo;
}
[Serializable]
public class ItemInfo
{
    public Sprite itemImage;
    public ItemType itemType;
    public int itemNumber;

    [Header("Weapon")]
    public string itemName;
    //public Vector3 weaponPos;
    public GameObject effect;
    public float effectPlayTime;
    public int damage;
    public float range;
    public float cosAngle;
    public float angleRange;
    public float coolTime;
}
