using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class InventoryUI : PopUpUI
{

    [SerializeField] Transform slotParent;
    public Slot[] slots;  //인벤토리 슬롯찾은거

    // 마우스가 UI창 위인지 확인용
    [SerializeField] RectTransform inventoryRect;
    public bool overInventory;

    [SerializeField] SpriteRenderer equipmentImage;  // 초반무기 이미지
    [SerializeField] InventoryUI inventoryUI;


    private void Awake()
    {
        //인벤토리 슬롯 전부 찾기
        slots = slotParent.GetComponentsInChildren<Slot>();
    }

    private void Start()
    {
        Debug.Log("초기데이터 저장");
        if (Manager.Data.GameData.inventoryData.Count < 1) //최초에 초기값 -1 넣기
        {
            //Manager.Data.GameData.inventoryData = new List<int>();

            Manager.Data.GameData.inventoryData.Add(1);
            //무기칸 1번에 데이터 입력
            inventoryUI.slots[0].SlotSet(Manager.Resource.itemDic[1].itemInfo.itemImage, Manager.Resource.itemDic[1].itemInfo.itemType, Manager.Resource.itemDic[1].itemInfo.itemNumber);
            // 케릭터 손에 무기이미지 입력
            equipmentImage.sprite = inventoryUI.slots[0].slotImage.sprite;

            for (int i = 1; i < 23; i++)
            {
                Manager.Data.GameData.inventoryData.Add(0);
            }
        }
        else
        {
            Debug.Log("인벤데이터 로드");
            for (int i = 0; i < 23; i++)
            {
                if (Manager.Data.GameData.inventoryData[i] != 0) //세이브데이터로 이미지 입력
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
        //마우스가 UI위인지 확인용
        Vector2 mousePosition = Input.mousePosition;  // 디스플레이의 비율 좌표
        if (RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition))
        {
            overInventory = true;
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
                    //이미지 입력
                    slots[i].SlotSet(item.image, item.type, item.id);
                    return true;
                }
            }
        }
        else if (item.type == ItemType.Etc)
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






    public void OnSaveData() //인벤토리 세이브
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