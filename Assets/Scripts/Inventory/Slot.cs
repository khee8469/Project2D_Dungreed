using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlotState { Blank, Fill }
//public enum Equipments { Equipment, Assistant , Accessory,  }

//������ �̹����� �ٲٴ°ſ�����
public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    //RectTransformUtility.RectangleContainsScreenPoint ui�浹Ȯ�� �Լ�

    public SlotState slotState;
    //public Equipments equipments;
    public ItemInfo itemInfo;

    //���̴� �̹���
    public Image itemImage;

    //���Կ� ������ �̹��� �߰�
    public void SlotSetImage()//UpdateSlotUI()
    {
        //itemImage.gameObject.SetActive(true);  // ���� �̹��� Ȱ��ȭ
        itemImage.sprite = itemInfo.itemImage;  // ���� �̹����� ���� ������ �̹����� ����
        SetColor(1);

    }

    //�����ʱ�ȭ
    public void ClearSlot()
    {
        slotState = SlotState.Blank;
        itemInfo = null; // ���� null
        SetColor(0);
        itemImage.sprite = null;
        //itemImage.gameObject.SetActive(false);
    }


    //���� ����
    private void SetColor(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (itemInfo.itemType == ItemType.Equipment)
            {
                Debug.Log("1");
                if (Manager.Game.inventoryUI.equipmentSlots[0].slotState == SlotState.Blank)
                {
                    Manager.Game.inventoryUI.equipmentSlots[0].itemInfo = this.itemInfo;
                    Manager.Game.inventoryUI.equipmentSlots[0].SlotSetImage();
                    Manager.Game.inventoryUI.equipmentSlots[0].slotState = SlotState.Fill;

                    ClearSlot();
                }
                else if (Manager.Game.inventoryUI.equipmentSlots[0].slotState == SlotState.Fill && Manager.Game.inventoryUI.equipmentSlots[1].slotState == SlotState.Blank)
                {
                    Manager.Game.inventoryUI.equipmentSlots[1].itemInfo = this.itemInfo;
                    Manager.Game.inventoryUI.equipmentSlots[1].SlotSetImage();
                    Manager.Game.inventoryUI.equipmentSlots[1].slotState = SlotState.Fill;

                    ClearSlot();
                }
                else // �Ѵ� ��������?
                {
                    ItemInfo temp = Manager.Game.inventoryUI.equipmentSlots[0].itemInfo;

                    Manager.Game.inventoryUI.equipmentSlots[0].itemInfo = this.itemInfo;
                    Manager.Game.inventoryUI.equipmentSlots[0].SlotSetImage();

                    itemInfo = temp;
                    SlotSetImage();
                }
            }

            else if (itemInfo.itemType == ItemType.Assistant)
            {
                Debug.Log("2");
                if (Manager.Game.inventoryUI.equipmentSlots[3].slotState == SlotState.Blank)
                {
                    Manager.Game.inventoryUI.equipmentSlots[3].itemInfo = this.itemInfo;
                    Manager.Game.inventoryUI.equipmentSlots[3].SlotSetImage();
                    Manager.Game.inventoryUI.equipmentSlots[3].slotState = SlotState.Fill;

                    ClearSlot();
                }
                else if (Manager.Game.inventoryUI.equipmentSlots[3].slotState == SlotState.Fill && Manager.Game.inventoryUI.equipmentSlots[1].slotState == SlotState.Blank)
                {
                    Manager.Game.inventoryUI.equipmentSlots[4].itemInfo = this.itemInfo;
                    Manager.Game.inventoryUI.equipmentSlots[4].SlotSetImage();
                    Manager.Game.inventoryUI.equipmentSlots[4].slotState = SlotState.Fill;

                    ClearSlot();
                }
                else // �Ѵ� ��������?
                {
                    ItemInfo temp = Manager.Game.inventoryUI.equipmentSlots[3].itemInfo;

                    Manager.Game.inventoryUI.equipmentSlots[3].itemInfo = this.itemInfo;
                    Manager.Game.inventoryUI.equipmentSlots[3].SlotSetImage();

                    itemInfo = temp;
                    SlotSetImage();
                }
            }

            else if (itemInfo.itemType == ItemType.Accessory)
            {
                Debug.Log("3");
                for (int i = 0; i < Manager.Game.inventoryUI.accessorySlots.Length; i++)
                {
                    if (Manager.Game.inventoryUI.accessorySlots[i].slotState == SlotState.Blank)
                    {
                       
                        Manager.Game.inventoryUI.accessorySlots[i].itemInfo = this.itemInfo;
                        Manager.Game.inventoryUI.accessorySlots[i].SlotSetImage();
                        Manager.Game.inventoryUI.accessorySlots[i].slotState = SlotState.Fill;

                        ClearSlot();
                    }
                }
            }
        }
    }


    //Debug.Log();
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotState != SlotState.Blank)
        {
            Debug.Log("�巡��");
            SetColor(0); // �����ִ��� �����ϰ�
            DragSlot.instance.dragSlot = this;  // ������ ����������, �̹���
            DragSlot.instance.DragSlotSetImage(itemImage.sprite); // �����̹����� �巡�� ���Կ� �̹�������
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //if (itemInfo.itemImage != null)
        //{
        DragSlot.instance.transform.position = eventData.position;
        //}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("����巡��");
        //Slot temp = DragSlot.instance.dragSlot;
        DragSlot.instance.DragSlotClear(); //�巡�� ���� �̹��� ����

        //temp.SetColor(1);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null) //�巡�׽����� �����Ǿ�������
        {
            //Debug.Log("���");
            if (slotState != SlotState.Blank)  // ������ġ�� �����Ͱ� ������
            {
                Debug.Log("��ü");
                ItemInfo temp = itemInfo; // ����� ��ġ�� ������ ����

                if (temp.itemImage != null) //�������վ�����
                {
                    //������ġ ������ �̹��� �Է�
                    itemInfo = DragSlot.instance.dragSlot.itemInfo;
                    SlotSetImage();

                    //ó����ġ ������ �̹��� �Է�
                    DragSlot.instance.dragSlot.itemInfo = temp;
                    DragSlot.instance.dragSlot.SlotSetImage();
                }
            }
            else if (slotState == SlotState.Blank)  // ������ġ�� �����Ͱ� ������
            {
                Debug.Log("�̵�");
                //������ġ�� ������ �Է�
                itemInfo = DragSlot.instance.dragSlot.itemInfo;
                slotState = SlotState.Fill;
                SlotSetImage();

                //������ġ�� ������ ����
                DragSlot.instance.dragSlot.ClearSlot();
            }
        }
    }
}
