using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//슬롯은 이미지만 바꾸는거엿구나
public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public ItemInfo itemInfo;
    public Image itemImage;


    Transform parent;


    //슬롯에 아이템 이미지 추가
    public void AddImageSlot()//UpdateSlotUI()
    {
        //itemImage.gameObject.SetActive(true);  // 슬롯 이미지 활성화
        SetColor(1);
        itemImage.sprite = itemInfo.itemImage;  // 슬롯 이미지를 먹은 아이템 이미지로 변경
    }




    //슬롯초기화
    public void ClearSlot()
    {
        itemInfo = null; // 참조 null
        SetColor(0);
        //itemImage.gameObject.SetActive(false);
    }


    //투명도 조절
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
                    //장착
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemInfo.itemImage != null)
        {
            SetColor(0); // 원래있던거 투명하게
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
        Debug.Log("드래그");
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
