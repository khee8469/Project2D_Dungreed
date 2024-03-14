using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//������ �̹����� �ٲٴ°ſ�����
public class Slot : MonoBehaviour
{
    public int slotNumber;
    public Item item;
    public Image itemIcon;

    //�������� ���� �� Slot�� ���� Update
    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemImage;  // ���� �̹����� ���� ������ �̹����� ����
        itemIcon.gameObject.SetActive(true);  // ���� �̹��� Ȱ��ȭ
    }

    public void RemoveSlot()
    {
        if(item != null)
        {
            item = null;  // Slot�� Item ������ ������� �ʴٸ�
        }
        itemIcon.gameObject.SetActive(false);  //  ���� �̹��� ��Ȱ��ȭ
    }
}
