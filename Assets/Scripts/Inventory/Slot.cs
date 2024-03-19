using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public enum SlotState { Blank, Fill }
public enum SlotKind { Equipment, Assistant, Accessory, Inventory }


//������ �̹����� �ٲٴ°ſ�����
public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    //RectTransformUtility.RectangleContainsScreenPoint ui�浹Ȯ�� �Լ�


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
    public void SlotSet(Sprite Image, ItemType type, int id)
    {
        slotImage.sprite = Image;
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
            //�κ��丮���� ����ϱ�
            if (slotKind == SlotKind.Inventory)
            {
                Debug.Log("�κ��丮");
                if (itemType == ItemType.Equipment)  // ���� ���
                {
                    Debug.Log("���");
                    if (inventoryUI.equipmentSlots[0].slotState == SlotState.Blank)
                    {
                        inventoryUI.equipmentSlots[0].SlotSet(slotImage.sprite, itemType, itemId);
                        ClearSlot();
                    }
                    else if (inventoryUI.equipmentSlots[0].slotState == SlotState.Fill && inventoryUI.equipmentSlots[1].slotState == SlotState.Blank)
                    {

                        inventoryUI.equipmentSlots[1].SlotSet(slotImage.sprite, itemType, itemId);
                        ClearSlot();
                    }
                    else if (inventoryUI.equipmentSlots[0].slotState == SlotState.Fill && inventoryUI.equipmentSlots[1].slotState == SlotState.Fill)// �Ѵ� ��������?
                    {
                        //���� ������ ����
                        Image ImageTemp = inventoryUI.equipmentSlots[0].slotImage;
                        ItemType typeTemp = inventoryUI.equipmentSlots[0].itemType;
                        int idTemp = inventoryUI.equipmentSlots[0].itemId;
                        //���� ������ ���Կ� �Է�
                        inventoryUI.equipmentSlots[0].SlotSet(slotImage.sprite, itemType, itemId);
                        //���� ���Կ� ������ ���Ե����� �Է�
                        SlotSet(ImageTemp.sprite, typeTemp, idTemp);
                    }
                }
                else if (itemType == ItemType.Assistant)  // ��ý�Ʈ ���
                {
                    Debug.Log("2");
                    if (inventoryUI.equipmentSlots[2].slotState == SlotState.Blank)
                    {
                        inventoryUI.equipmentSlots[2].SlotSet(slotImage.sprite, itemType, itemId);
                        ClearSlot();
                    }
                    else if (inventoryUI.equipmentSlots[2].slotState == SlotState.Fill && inventoryUI.equipmentSlots[3].slotState == SlotState.Blank)
                    {
                        inventoryUI.equipmentSlots[3].SlotSet(slotImage.sprite, itemType, itemId);
                        ClearSlot();
                    }
                    else // �Ѵ� ��������?
                    {
                        Image ImageTemp = inventoryUI.equipmentSlots[2].slotImage;
                        ItemType typeTemp = inventoryUI.equipmentSlots[2].itemType;
                        int idTemp = inventoryUI.equipmentSlots[2].itemId;
                        //���� ������ ���Կ� �Է�
                        inventoryUI.equipmentSlots[2].SlotSet(slotImage.sprite, itemType, itemId);
                        //���� ���Կ� ������ ���Ե����� �Է�
                        SlotSet(ImageTemp.sprite, typeTemp, idTemp);
                    }
                }
                else if (itemType == ItemType.Accessory)
                {
                    Debug.Log("3");
                    for (int i = 0; i < inventoryUI.accessorySlots.Length; i++)
                    {
                        if (inventoryUI.accessorySlots[i].slotState == SlotState.Blank)
                        {
                            inventoryUI.accessorySlots[i].SlotSet(slotImage.sprite, itemType, itemId);
                            ClearSlot();
                            return;
                        }
                    }
                }
            }


            // ����� ������ ����
            if (slotState == SlotState.Fill && slotKind == SlotKind.Equipment || slotKind == SlotKind.Assistant || slotKind == SlotKind.Accessory)
            {
                Debug.Log(1);
                for (int i = 0; i < inventoryUI.inventorySlots.Length; i++)
                {
                    if (inventoryUI.inventorySlots[i].slotState == SlotState.Blank)
                    {
                        //Debug.Log("������ �Է�");
                        inventoryUI.inventorySlots[i].SlotSet(slotImage.sprite, itemType, itemId);
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
        /*else if (DragSlot.instance.dragSlot != null && inventoryUI.overInventory)
        {
            DragSlot.instance.dragSlot.SlotSet(ImageTemp.sprite, typeTemp, idTemp);
            DragSlot.instance.DragSlotClear(); //�巡�� ���� �̹��� ����
        }*/
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null) //�巡�׽����� �����Ǿ�������
        {
            Debug.Log("���");
            if (slotState != SlotState.Blank)  // ������ġ�� �����Ͱ� ������
            {
                //Debug.Log("��ü");
                Image ImageTemp = slotImage;
                ItemType typeTemp = itemType;
                int idTemp = itemId;

                //������ġ ������ �̹��� �Է�
                SlotSet(DragSlot.instance.dragSlot.slotImage.sprite, DragSlot.instance.dragSlot.itemType, DragSlot.instance.dragSlot.itemId);

                //ó����ġ ������ �̹��� �Է�
                DragSlot.instance.dragSlot.SlotSet(ImageTemp.sprite, typeTemp, idTemp);
            }
            else if (slotState == SlotState.Blank)  // ������ġ�� �����Ͱ� ������
            {
                //Debug.Log("�̵�");
                //������ġ�� ������ �Է�
                SlotSet(DragSlot.instance.dragSlot.slotImage.sprite, DragSlot.instance.dragSlot.itemType, DragSlot.instance.dragSlot.itemId);

                //������ġ�� ������ ����
                DragSlot.instance.dragSlot.ClearSlot();
            }
        }
    }
}
