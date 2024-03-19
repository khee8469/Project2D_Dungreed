using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [SerializeField] RectTransform inventoryRect;
    public bool overInventory;

    private void Awake()
    {
        //인벤토리 슬롯 전부 찾기
        inventorySlots = slotParent.GetComponentsInChildren<Slot>();
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<Slot>();
        accessorySlots = accessorySlotParent.GetComponentsInChildren<Slot>();
    }

    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;  // 디스플레이의 비율 좌표

        if (RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition))  
        {
            overInventory = true;
            Debug.Log("ui위"); 
        }
        else
        {
            overInventory = false;
        }
    }
    /*public void AddItemImage(Sprite Image, ItemType type, int id)  // 슬롯 데이터 입력
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (type != ItemType.Etc && inventorySlots[i].slotState == SlotState.Blank)
            {
                //Slot에있는 Item 에 내가먹은 Item 정보를 넣음
                inventorySlots[i].SlotSet(Image, type, id);
                return;
            }
        }   
    }*/

    public bool AddItemData(Sprite Image, ItemType type, int id)  //인벤토리 데이터 저장
    {
        if (Image == null)
        {
            return false;
        }
        if (type != ItemType.Etc)
        {
            for(int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].slotState == SlotState.Blank)
                {
                    //데이터 저장
                    Manager.Data.InventoryData.inventoryItem.Add(Manager.Resource.itemDic[id].itemInfo);
                    //이미지 입력
                    inventorySlots[i].SlotSet(Image, type, id);
                    return true;
                }
            }
            
        }
        else if (type == ItemType.Etc)
        {
            //골드 물약
            Manager.Data.GameData.gold += 10;  //테스트
            return true;
        }
        return false;
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