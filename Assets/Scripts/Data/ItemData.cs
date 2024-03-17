using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Equipment, Assistant, Accessory, Etc }
[CreateAssetMenu(fileName = "ItemData", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemInfo[] itemInfo;


}

[Serializable]
public class ItemInfo
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;


    [Header("Weapon")]
    public int weaponNumber;
    public Vector3 weaponPos;
    public GameObject effect;
    public float effectPlayTime;
    public int damage;
    public float range;
    public float cosAngle;
    public float angleRange;
    public float coolTime;
}
