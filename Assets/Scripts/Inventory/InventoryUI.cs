using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : PopUpUI
{
    //[SerializeField] GameObject inventoryUI;

    [SerializeField] Transform slotParent;
    [SerializeField] Transform equipmentSlotParent;
    [SerializeField] Transform accessorySlotParent;


    public Slot[] inventorySlots;  //인벤토리 슬롯찾은거
    public Slot[] equipmentSlots;
    public Slot[] accessorySlots;


    private void Awake()
    {
        //인벤토리 슬롯 전부 찾기
        inventorySlots = slotParent.GetComponentsInChildren<Slot>();
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<Slot>();
        accessorySlots = accessorySlotParent.GetComponentsInChildren<Slot>();
    }

    public void AddItemImage(Sprite Image, ItemType type, int id)  // 슬롯 데이터 입력
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (type != ItemType.Etc && inventorySlots[1].slotState == SlotState.Blank)
            {
                //Slot에있는 Item 에 내가먹은 Item 정보를 넣음
                inventorySlots[i].SlotSet(Image, type, id);
                return;
            }
        }   
    }

    public void SlotClear()
    {
        //빈슬롯 데이터 
    }

    /*public void InventoryClose()
    {
        inventoryUI.SetActive(false);
    }*/

}