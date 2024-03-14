using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Data/Weapon")]  //
public class WeaponData : ScriptableObject
{
    //public Item prefab;
    public WeaponInfo[] weapons;


    [Serializable]
    public struct WeaponInfo
    {
        public Weapon weapon;
        public int weaponPos;
        public GameObject effect;
        public float effectPlayTime;

        public string name;
        public int damage;
        public float range;
        public float angleRange;
        public float coolTime;
    }
}
