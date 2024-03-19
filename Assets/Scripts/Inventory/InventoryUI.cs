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


    public Slot[] inventorySlots;  //�κ��丮 ����ã����
    public Slot[] equipmentSlots;
    public Slot[] accessorySlots;

    [SerializeField] RectTransform inventoryRect;
    public bool overInventory;

    private void Awake()
    {
        //�κ��丮 ���� ���� ã��
        inventorySlots = slotParent.GetComponentsInChildren<Slot>();
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<Slot>();
        accessorySlots = accessorySlotParent.GetComponentsInChildren<Slot>();
    }

    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;  // ���÷����� ���� ��ǥ

        if (RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition))  
        {
            overInventory = true;
            Debug.Log("ui��"); 
        }
        else
        {
            overInventory = false;
        }
    }
    /*public void AddItemImage(Sprite Image, ItemType type, int id)  // ���� ������ �Է�
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (type != ItemType.Etc && inventorySlots[i].slotState == SlotState.Blank)
            {
                //Slot���ִ� Item �� �������� Item ������ ����
                inventorySlots[i].SlotSet(Image, type, id);
                return;
            }
        }   
    }*/

    public bool AddItemData(Sprite Image, ItemType type, int id)  //�κ��丮 ������ ����
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
                    //������ ����
                    Manager.Data.InventoryData.inventoryItem.Add(Manager.Resource.itemDic[id].itemInfo);
                    //�̹��� �Է�
                    inventorySlots[i].SlotSet(Image, type, id);
                    return true;
                }
            }
            
        }
        else if (type == ItemType.Etc)
        {
            //��� ����
            Manager.Data.GameData.gold += 10;  //�׽�Ʈ
            return true;
        }
        return false;
    }

    public void SlotClear()
    {
        //�󽽷� ������ 
    }

    /*public void InventoryClose()
    {
        inventoryUI.SetActive(false);
    }*/

}