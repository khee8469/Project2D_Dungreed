using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static InventoryUI;
using static UnityEditor.Progress;

public class InventoryUI : WindowUI
{
    [SerializeField] LayerMask itemLayer;

    public Transform slotParent;
    [SerializeField] Slot[] slots;  //start에서 찾아둔 Slot들
    
    //슬롯 추가용
    public delegate void OnSlotCountChange(int value); //델리게이트 정의
    public OnSlotCountChange onSlotCountChange; //델리게이트 인스턴스화
    //아이템 추가용
    /*public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;*/

    List<Item> items = new List<Item>(); // 인벤토리 저장용

    private int sloatCount;
    public int SloatCount { get { return sloatCount; } set { sloatCount = value; onSlotCountChange.Invoke(sloatCount); } }


    private void Awake()
    {
        base.Awake();
        GetUI<Button>("InventoryExitButton").onClick.AddListener(Close);
    }

    private void Start()
    {
        //인벤토리 슬롯 전부 찾기
        slots = slotParent.GetComponentsInChildren<Slot>();
        //델리게이트 이벤트 추가
        onSlotCountChange += SlotChange;
        //onChangeItem += RedrawSlotUI;
        //인벤토리 슬롯겟수
        SloatCount = slots.Length;

        
    }

    private void SlotChange(int value)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotNumber = i;

            if (i < SloatCount) // 슬롯카운트겟수만큼 인벤토리 오픈
            {
                //인벤토리버튼 활성화
                slots[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                slots[i].GetComponent<Button>().interactable = false;
            }
        }
    }


    //슬롯이미지 출력여부 확인
    private void RedrawSlotUI()
    {
        for (int i = 0; i < items.Count; i++)
        {
            //Slot에있는 Item 에 내가먹은 Item 정보를 넣음
            slots[i].item = items[i];
            slots[i].UpdateSlotUI();
        }
        /*for (int i = 0; i < slots.Length;i++)
        {
            //비어진 슬롯 이미지 비활성화
            slots[i].RemoveSlot();
        }*/
    }

    //슬롯추가
    public void AddSlot()
    {
        if (SloatCount == slots.Length)
        {
            return;
        }
        SloatCount++;
    }

    //playermove함수에서 실행
    public bool AddItem(Item item)
    {
        if (item == null)
        {
            return false;
        }
            
        if (items.Count < SloatCount)
        {
            items.Add(item);
            //if (onChangeItem != null)
            //{
                RedrawSlotUI();
            return true;
        }
        return false;
    }

    public void RemoveItem(int index)
    {
        items.RemoveAt(index);
        RedrawSlotUI();
    }
}
