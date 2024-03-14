using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//슬롯은 이미지만 바꾸는거엿구나
public class Slot : MonoBehaviour
{
    public int slotNumber;
    public Item item;
    public Image slotIcon;


    //아이템을 먹은 후 Slot에 정보 Update
    public void UpdateSlotUI()
    {
        slotIcon.sprite = item.itemImage;  // 슬롯 이미지를 먹은 아이템 이미지로 변경
        slotIcon.gameObject.SetActive(true);  // 슬롯 이미지 활성화
    }

    public void RemoveSlot()
    {
        if(item == null)
        {
            slotIcon.gameObject.SetActive(false);
        }
    }
}
