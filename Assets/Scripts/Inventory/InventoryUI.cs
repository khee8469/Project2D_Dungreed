using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : PopUpUI
{
    //[SerializeField] LayerMask itemLayer;
    [SerializeField] Transform slotParent;
    [SerializeField] Transform equipmentSlotParent;
    [SerializeField] Transform accessorySlotParent;

    public Slot[] slots;  //start���� ã�Ƶ� Slot��
    public Slot[] equipmentSlots;
    public Slot[] accessorySlots;


    List<ItemInfo> items = new List<ItemInfo>(); // ������ ����Ǿ��մµ�...


    private void Awake()
    {
        /*base.Awake();
        GetUI<Button>("InventoryExitButton").onClick.AddListener(Close);*/
        //�κ��丮 ���� ���� ã��
        slots = slotParent.GetComponentsInChildren<Slot>();
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<Slot>();
        accessorySlots= accessorySlotParent.GetComponentsInChildren<Slot>();
    }

    //player�� �����۸����� ����
    public bool AddItem(ItemInfo item)
    {
        if (item == null)
        {
            return false;
        }
        if (items.Count < slots.Length && item.itemType != ItemType.Etc )
        {
            items.Add(item);
            for (int i = 0; i < items.Count; i++)
            {
                if (slots[i].slotState == SlotState.Blank)
                {
                    //Debug.Log("������ �Է�");
                    //Slot���ִ� Item �� �������� Item ������ ����
                    
                    slots[i].itemInfo = item;
                    slots[i].slotState = SlotState.Fill;
                    slots[i].SlotSetImage();
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