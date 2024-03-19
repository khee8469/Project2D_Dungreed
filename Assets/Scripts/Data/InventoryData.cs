using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryData
{
    //인벤토리 데이터
    public List<ItemInfo> inventoryItem = new List<ItemInfo>();


    /*public bool AddItemData(Sprite Image, ItemType type, int id)  //인벤토리 데이터 저장
    {
        if (Image == null)
        {
            return false;
        }
        if (type != ItemType.Etc)
        {
            inventoryItem.Add(Manager.Resource.itemDic[id].itemInfo);
            return true;
        }
        else if (type == ItemType.Etc)
        {
            //골드 물약
            Manager.Data.GameData.gold += 10;  //테스트
            return true;
        }
        return false;
    }*/

    public void RemoveItem(int index)
    {
        inventoryItem.RemoveAt(index);
    }
}
