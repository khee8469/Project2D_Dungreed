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

    public Slot[] slots;  //start에서 찾아둔 Slot들
    public Slot[] equipmentSlots;
    public Slot[] accessorySlots;


    List<ItemInfo> items = new List<ItemInfo>(); // 데이터 저장되어잇는데...


    private void Awake()
    {
        /*base.Awake();
        GetUI<Button>("InventoryExitButton").onClick.AddListener(Close);*/
        //인벤토리 슬롯 전부 찾기
        slots = slotParent.GetComponentsInChildren<Slot>();
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<Slot>();
        accessorySlots= accessorySlotParent.GetComponentsInChildren<Slot>();
    }

    //player가 아이템먹으면 실행
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
                    //Debug.Log("데이터 입력");
                    //Slot에있는 Item 에 내가먹은 Item 정보를 넣음
                    
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