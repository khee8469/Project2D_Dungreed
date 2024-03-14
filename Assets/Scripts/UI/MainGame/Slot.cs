using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//������ �̹����� �ٲٴ°ſ�����
public class Slot : MonoBehaviour
{
    public int slotNumber;
    public Item item;
    public Image slotIcon;


    //�������� ���� �� Slot�� ���� Update
    public void UpdateSlotUI()
    {
        slotIcon.sprite = item.itemImage;  // ���� �̹����� ���� ������ �̹����� ����
        slotIcon.gameObject.SetActive(true);  // ���� �̹��� Ȱ��ȭ
    }

    public void RemoveSlot()
    {
        if(item == null)
        {
            slotIcon.gameObject.SetActive(false);
        }
    }
}
