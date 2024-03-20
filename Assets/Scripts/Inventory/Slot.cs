using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public enum SlotState { Blank, Fill }  // ������ ID�� -1�̳� 0 �̸� ����ִ°ŷ� ǥ�Ⱑ��
public enum SlotKind { Equipment, Assistant, Accessory, Inventory }


//������ �̹����� �ٲٴ°ſ�����
public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    //RectTransformUtility.RectangleContainsScreenPoint ui�浹Ȯ�� �Լ�
    public int slotNumber;

    //���̴� �̹���
    public Image slotImage;
    public ItemType itemType;
    public int itemId;

    public SlotKind slotKind;
    public SlotState slotState;

    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] ItemDestoryUI itemDestoryUI;

    

    /*public UnityEvent spriteChange;
    spriteChange?.Invoke();*/

    private void Start()
    {
        inventoryUI = transform.GetComponentInParent<InventoryUI>();
    }


    //���Ե����� �Է�
    public void SlotSet(Sprite image, ItemType type, int id)
    {
        slotImage.sprite = image;
        itemType = type;
        itemId = id;
        slotState = SlotState.Fill;
        SetColor(1);
    }

    //�����ʱ�ȭ
    public void ClearSlot()
    {
        slotImage.sprite = null;
        itemType = ItemType.Null;
        itemId = 0;
        slotState = SlotState.Blank;
        SetColor(0);
    }


    //���� ����
    private void SetColor(float alpha)
    {
        Color color = slotImage.color;
        color.a = alpha;
        slotImage.color = color;

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            //�κ��丮���� Ŭ��
            if (slotKind == SlotKind.Inventory)
            {
                //Debug.Log("�κ��丮");
                if (itemType == ItemType.Equipment)  // ���� ���
                {
                    //Debug.Log("���");
                    if (inventoryUI.slots[0].slotState == SlotState.Blank)
                    {
                        inventoryUI.slots[0].SlotSet(slotImage.sprite, itemType, itemId);

                        
                        ClearSlot();
                    }
                    else if (inventoryUI.slots[0].slotState == SlotState.Fill && inventoryUI.slots[1].slotState == SlotState.Blank)
                    {
                        inventoryUI.slots[1].SlotSet(slotImage.sprite, itemType, itemId);

                        ClearSlot();
                    }
                    else if (inventoryUI.slots[0].slotState == SlotState.Fill && inventoryUI.slots[1].slotState == SlotState.Fill)// �Ѵ� ��������?
                    {
                        //���� ������ ����
                        Sprite spriteTemp = inventoryUI.slots[0].slotImage.sprite;
                        ItemType typeTemp = inventoryUI.slots[0].itemType;
                        int idTemp = inventoryUI.slots[0].itemId;


                        //���� ������ ���Կ� �Է�
                        inventoryUI.slots[0].SlotSet(slotImage.sprite, itemType, itemId);
                        //���� ���Կ� ������ ���Ե����� �Է�
                        SlotSet(spriteTemp, typeTemp, idTemp);
                    }
                }
                else if (itemType == ItemType.Assistant)  // ��ý�Ʈ ���
                {
                    if (inventoryUI.slots[2].slotState == SlotState.Blank)
                    {
                        inventoryUI.slots[2].SlotSet(slotImage.sprite, itemType, itemId);
                        ClearSlot();
                    }
                    else if (inventoryUI.slots[2].slotState == SlotState.Fill && inventoryUI.slots[3].slotState == SlotState.Blank)
                    {
                        inventoryUI.slots[3].SlotSet(slotImage.sprite, itemType, itemId);
                        ClearSlot();
                    }
                    else // �Ѵ� ��������?
                    {
                        Sprite ImageTemp = inventoryUI.slots[2].slotImage.sprite;
                        ItemType typeTemp = inventoryUI.slots[2].itemType;
                        int idTemp = inventoryUI.slots[2].itemId;
                        //���� ������ ���Կ� �Է�
                        inventoryUI.slots[2].SlotSet(slotImage.sprite, itemType, itemId);
                        //���� ���Կ� ������ ���Ե����� �Է�
                        SlotSet(ImageTemp, typeTemp, idTemp);
                    }
                }
                else if (itemType == ItemType.Accessory)
                {
                    for (int i = 4; i < inventoryUI.slots.Length-19; i++)
                    {
                        if (inventoryUI.slots[i].slotState == SlotState.Blank)
                        {
                            inventoryUI.slots[i].SlotSet(slotImage.sprite, itemType, itemId);
                            ClearSlot();
                            return;
                        }
                    }
                }
            }

            // ����� ������ ����
            if (slotState == SlotState.Fill && slotKind != SlotKind.Inventory)
            {
                for (int i = 8; i < inventoryUI.slots.Length-8; i++)
                {
                    if (inventoryUI.slots[i].slotState == SlotState.Blank)
                    {
                        //Debug.Log("������ �Է�");
                        inventoryUI.slots[i].SlotSet(slotImage.sprite, itemType, itemId);
                        ClearSlot();
                        return;
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
            //Debug.Log("�巡��");
            SetColor(0); // �����ִ��� �����ϰ�
            DragSlot.instance.dragSlot = this;  // ������ ���� �Է�
            DragSlot.instance.DragSlotSetImage(slotImage.sprite); // �����̹����� �巡�� ���Կ� �̹�������
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("����巡��");
        if (DragSlot.instance.dragSlot != null && !inventoryUI.overInventory) // �κ��丮 ���̸�
        {
            //������ ������ �˾�����
            DragSlot.instance.slotImage.sprite = null;
            DragSlot.instance.SetColor(0);
            Manager.UI.ShowWindowUI(itemDestoryUI); // �˾� ����
        }
        /*else if (DragSlot.instance.dragSlot != null && inventoryUI.overInventory) // �κ��丮 ��
        {
            DragSlot.instance.dragSlot.SlotSet(slotImage.sprite, itemType, itemId);//�������� �̹��� ����
            DragSlot.instance.DragSlotClear(); //�巡�� ���� �̹��� ����
        }*/

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null) //�巡�׽����� �����Ǿ�������
        {
            Image ImageTemp = slotImage;
            ItemType typeTemp = itemType;
            int idTemp = itemId;
            Debug.Log("���");
            if (slotState != SlotState.Blank)  // ������ġ�� �����Ͱ� ������
            {
                //Debug.Log("��ü");
                //������ġ ������ �̹��� �Է�
                SlotSet(DragSlot.instance.dragSlot.slotImage.sprite, DragSlot.instance.dragSlot.itemType, DragSlot.instance.dragSlot.itemId);

                //ó����ġ ������ �̹��� �Է�
                DragSlot.instance.dragSlot.SlotSet(ImageTemp.sprite, typeTemp, idTemp);
                DragSlot.instance.DragSlotClear();
            }
            else if (slotState == SlotState.Blank)  // ������ġ�� �����Ͱ� ������
            {
                //Debug.Log("�̵�");
                //������ġ�� ������ �Է�
                SlotSet(DragSlot.instance.dragSlot.slotImage.sprite, DragSlot.instance.dragSlot.itemType, DragSlot.instance.dragSlot.itemId);

                //������ġ�� ������ ����
                DragSlot.instance.dragSlot.ClearSlot();
            }
            else // �������� �ƴ� �κ��丮
            {
                DragSlot.instance.dragSlot.SlotSet(ImageTemp.sprite, typeTemp, idTemp);
                DragSlot.instance.DragSlotClear();
            }
        }
    }
}
