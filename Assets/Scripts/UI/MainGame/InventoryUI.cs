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
    [SerializeField] Slot[] slots;  //start���� ã�Ƶ� Slot��
    
    //���� �߰���
    public delegate void OnSlotCountChange(int value); //��������Ʈ ����
    public OnSlotCountChange onSlotCountChange; //��������Ʈ �ν��Ͻ�ȭ
    //������ �߰���
    /*public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;*/

    List<Item> items = new List<Item>(); // �κ��丮 �����

    private int sloatCount;
    public int SloatCount { get { return sloatCount; } set { sloatCount = value; onSlotCountChange.Invoke(sloatCount); } }


    private void Awake()
    {
        base.Awake();
        GetUI<Button>("InventoryExitButton").onClick.AddListener(Close);
    }

    private void Start()
    {
        //�κ��丮 ���� ���� ã��
        slots = slotParent.GetComponentsInChildren<Slot>();
        //��������Ʈ �̺�Ʈ �߰�
        onSlotCountChange += SlotChange;
        //onChangeItem += RedrawSlotUI;
        //�κ��丮 ���԰ټ�
        SloatCount = slots.Length;

        
    }

    private void SlotChange(int value)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotNumber = i;

            if (i < SloatCount) // ����ī��Ʈ�ټ���ŭ �κ��丮 ����
            {
                //�κ��丮��ư Ȱ��ȭ
                slots[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                slots[i].GetComponent<Button>().interactable = false;
            }
        }
    }


    //�����̹��� ��¿��� Ȯ��
    private void RedrawSlotUI()
    {
        for (int i = 0; i < items.Count; i++)
        {
            //Slot���ִ� Item �� �������� Item ������ ����
            slots[i].item = items[i];
            slots[i].UpdateSlotUI();
        }
        /*for (int i = 0; i < slots.Length;i++)
        {
            //����� ���� �̹��� ��Ȱ��ȭ
            slots[i].RemoveSlot();
        }*/
    }

    //�����߰�
    public void AddSlot()
    {
        if (SloatCount == slots.Length)
        {
            return;
        }
        SloatCount++;
    }

    //playermove�Լ����� ����
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
