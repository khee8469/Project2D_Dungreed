using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class EquipmentSlot : MonoBehaviour, IPointerClickHandler //IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    //RectTransformUtility.RectangleContainsScreenPoint ui�浹Ȯ�� �Լ�
    public SlotState slotState;
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
            if (slotState != SlotState.Blank)
            {
                if (itemInfo.itemType == ItemType.Equipment)
                {
                    Debug.Log("����");
                }
            }
        }
    }


    /*//Debug.Log();
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
    }*/
}