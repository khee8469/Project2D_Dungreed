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


    public Slot[] inventorySlots;  //�κ��丮 ����ã����
    public Slot[] equipmentSlots;
    public Slot[] accessorySlots;


    private void Awake()
    {
        //�κ��丮 ���� ���� ã��
        inventorySlots = slotParent.GetComponentsInChildren<Slot>();
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<Slot>();
        accessorySlots = accessorySlotParent.GetComponentsInChildren<Slot>();
    }

    public void AddItemImage(Sprite Image, ItemType type, int id)  // ���� ������ �Է�
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (type != ItemType.Etc && inventorySlots[1].slotState == SlotState.Blank)
            {
                //Slot���ִ� Item �� �������� Item ������ ����
                inventorySlots[i].SlotSet(Image, type, id);
                return;
            }
        }   
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