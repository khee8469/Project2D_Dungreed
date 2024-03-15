using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//������ �̹����� �ٲٴ°ſ�����
public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public ItemInfo itemInfo;
    public Image itemImage;


    Transform parent;


    //���Կ� ������ �̹��� �߰�
    public void AddImageSlot()//UpdateSlotUI()
    {
        //itemImage.gameObject.SetActive(true);  // ���� �̹��� Ȱ��ȭ
        SetColor(1);
        itemImage.sprite = itemInfo.itemImage;  // ���� �̹����� ���� ������ �̹����� ����
    }




    //�����ʱ�ȭ
    public void ClearSlot()
    {
        itemInfo = null; // ���� null
        SetColor(0);
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
            if (itemInfo.itemImage != null)
            {
                if (itemInfo.itemType == ItemType.Equipment)
                {
                    //����
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemInfo.itemImage != null)
        {
            SetColor(0); // �����ִ��� �����ϰ�
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemInfo.itemImage != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("�巡��");
        DragSlot.instance.OutImage(DragSlot.instance.dragSlot.itemImage);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            ChangeSlot();
        }
    }

    private void ChangeSlot()
    {
        ItemInfo temp = itemInfo;
        //itemInfo = DragSlot.instance.dragSlot.itemInfo;
        Manager.Game.inventoryUI.AddItem(DragSlot.instance.dragSlot.itemInfo); //

        if (temp.itemImage != null)
        {
            DragSlot.instance.dragSlot.itemInfo = temp;
        }
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }
}
