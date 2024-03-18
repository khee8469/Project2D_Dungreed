using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryData
{
    //�κ��丮 ������
    List<ItemInfo> inventoryItem = new List<ItemInfo>();


    public int inventoryIndex = 15;


    public bool AddItemData(Sprite Image, ItemType type, int id)  //�κ��丮 ������ ����
    {
        if (Image == null)
        {
            return false;
        }
        if (inventoryItem.Count < inventoryIndex && type != ItemType.Etc)
        {
            inventoryItem.Add(Manager.Resource.itemDic[id].itemInfo);
            return true;
        }
        else if (type == ItemType.Etc)
        {
            //��� ����
            Manager.Data.GameData.gold += 10;  //�׽�Ʈ
            return true;
        }
        return false;
    }

    public void RemoveItem(int index)
    {
        inventoryItem.RemoveAt(index);
    }
}
