using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : PopUpUI
{
    //[SerializeField] LayerMask itemLayer;
    [SerializeField] Transform slotParent;
    public Slot[] slots;  //start에서 찾아둔 Slot들


    List<ItemInfo> items = new List<ItemInfo>();


    private void Awake()
    {
        /*base.Awake();
        GetUI<Button>("InventoryExitButton").onClick.AddListener(Close);*/
        //인벤토리 슬롯 전부 찾기
        slots = slotParent.GetComponentsInChildren<Slot>();

    }

    //player가 아이템먹으면 실행
    public bool AddItem(ItemInfo item)
    {
        if (item == null)
        {
            return false;
        }
        if (items.Count < slots.Length && item.itemType == ItemType.Equipment)
        {
            items.Add(item);
            for (int i = 0; i < items.Count; i++)
            {
                if (slots[i].itemInfo.itemImage == null)
                {
                    //Slot에있는 Item 에 내가먹은 Item 정보를 넣음
                    slots[i].itemInfo = item;
                    slots[i].AddImageSlot();
                }
            }
            return true;
        }

        else if (item.itemType == ItemType.Etc)
        {
            return true;
        }
        return false;
    }


    public void RemoveItem(int index)
    {
        items.RemoveAt(index);
    }

}