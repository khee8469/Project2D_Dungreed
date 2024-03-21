using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class InventoryUI : PopUpUI
{

    [SerializeField] Transform slotParent;
    public Slot[] slots;  //�κ��丮 ����ã����

    // ���콺�� UIâ ������ Ȯ�ο�
    [SerializeField] RectTransform inventoryRect;
    public bool overInventory;

    [SerializeField] SpriteRenderer equipmentImage;  // �ʹݹ��� �̹���
    [SerializeField] InventoryUI inventoryUI;


    private void Awake()
    {
        //�κ��丮 ���� ���� ã��
        slots = slotParent.GetComponentsInChildren<Slot>();
    }

    private void Start()
    {
        Debug.Log("�ʱⵥ���� ����");
        if (Manager.Data.GameData.inventoryData.Count < 1) //���ʿ� �ʱⰪ -1 �ֱ�
        {
            //Manager.Data.GameData.inventoryData = new List<int>();

            Manager.Data.GameData.inventoryData.Add(1);
            //����ĭ 1���� ������ �Է�
            inventoryUI.slots[0].SlotSet(Manager.Resource.itemDic[1].itemInfo.itemImage, Manager.Resource.itemDic[1].itemInfo.itemType, Manager.Resource.itemDic[1].itemInfo.itemNumber);
            // �ɸ��� �տ� �����̹��� �Է�
            equipmentImage.sprite = inventoryUI.slots[0].slotImage.sprite;

            for (int i = 1; i < 23; i++)
            {
                Manager.Data.GameData.inventoryData.Add(0);
            }
        }
        else
        {
            Debug.Log("�κ������� �ε�");
            for (int i = 0; i < 23; i++)
            {
                if (Manager.Data.GameData.inventoryData[i] != 0) //���̺굥���ͷ� �̹��� �Է�
                {
                    int itemID = Manager.Data.GameData.inventoryData[i];

                    slots[i].SlotSet(Manager.Resource.itemDic[itemID].itemInfo.itemImage,
                        Manager.Resource.itemDic[itemID].itemInfo.itemType,
                        Manager.Resource.itemDic[itemID].itemInfo.itemNumber);
                }
            }
        }
    }

    private void Update()
    {
        //���콺�� UI������ Ȯ�ο�
        Vector2 mousePosition = Input.mousePosition;  // ���÷����� ���� ��ǥ
        if (RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition))
        {
            overInventory = true;
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

    public bool AddItemData(Item item)
    {
        if (item.image == null)
        {
            return false;
        }
        if (item.type != ItemType.Etc)
        {
            for (int i = 8; i < slots.Length; i++)
            {

                if (slots[i].slotState == SlotState.Blank)
                {
                    //�̹��� �Է�
                    slots[i].SlotSet(item.image, item.type, item.id);
                    return true;
                }
            }
        }
        else if (item.type == ItemType.Etc)
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






    public void OnSaveData() //�κ��丮 ���̺�
    {
        for (int i = 0; i < 23; i++)
        {
            /*if (slots[i].itemId == 0)
                Manager.Data.GameData.inventoryData[i] = 0;
            else*/
                Manager.Data.GameData.inventoryData[i] = slots[i].itemId;
        }
    }

    private void OnDisable()
    {
        OnSaveData();
        Manager.Data.SaveData(0);
    }
}