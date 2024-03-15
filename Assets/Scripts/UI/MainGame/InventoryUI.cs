using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : PopUpUI
{
    //[SerializeField] LayerMask itemLayer;
    [SerializeField] Transform slotParent;
    public Slot[] slots;  //start���� ã�Ƶ� Slot��


    List<ItemInfo> items = new List<ItemInfo>();


    private void Awake()
    {
        /*base.Awake();
        GetUI<Button>("InventoryExitButton").onClick.AddListener(Close);*/
        //�κ��丮 ���� ���� ã��
        slots = slotParent.GetComponentsInChildren<Slot>();

    }

    //player�� �����۸����� ����
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
                    //Slot���ִ� Item �� �������� Item ������ ����
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