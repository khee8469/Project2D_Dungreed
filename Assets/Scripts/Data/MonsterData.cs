using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/Monster")]
public class MonsterData : ScriptableObject
{
    public MonsterInfo[] monsters;


    [Serializable]
    public struct MonsterInfo
    {
        public string name;
        public int damage;
        public int hp;
        public float speed;
        public float jumpPower;
        public float findRange;
        public float attackRange;
        public float attackAngle; 
        public float coolTime;
        public float attackTime;
    }
}
