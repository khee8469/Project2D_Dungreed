using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//슬롯은 이미지만 바꾸는거엿구나
public class Slot : MonoBehaviour
{
    public int slotNumber;
    public Item item;
    public Image itemIcon;

    //아이템을 먹은 후 Slot에 정보 Update
    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemImage;  // 슬롯 이미지를 먹은 아이템 이미지로 변경
        itemIcon.gameObject.SetActive(true);  // 슬롯 이미지 활성화
    }

    public void RemoveSlot()
    {
        if(item != null)
        {
            item = null;  // Slot에 Item 정보가 담겨있지 않다면
        }
        itemIcon.gameObject.SetActive(false);  //  슬롯 이미지 비활성화
    }
}
