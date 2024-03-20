using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[Serializable]
public class GameData
{
    [Header("PlayerState")]
    public int hp = 100;
    public int maxHp = 100;
    public int speed = 10;
    public int jumpPower = 20;
    public int dashPower = 25;
    public int maxAngle = 60;

    public int gold = 0;

    //����
    //�κ��丮 �����۳ѹ�
    public List<int> inventoryData = new List<int>();



}
