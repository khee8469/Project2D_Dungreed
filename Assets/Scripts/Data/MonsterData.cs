using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType { Nomal, Boss}
[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/Monster")]
public class MonsterData : ScriptableObject
{
    public MonsterInfo monsterInfo;


    [Serializable]
    public struct MonsterInfo
    {
        public Sprite monsterImage;
        public MonsterType monsterType;
        public int monsterNumber;


        public string name;
        public int damage;
        public int hp;
        public float speed;
        public float jumpPower;
        public float findRange;
        public float attackRange;
        public float attackAngle; 
        public float coolTime;
        public float attackTime; //공격중 시간
    }
}
