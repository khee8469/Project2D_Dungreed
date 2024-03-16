using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class EquipmentSlot : MonoBehaviour, IPointerClickHandler //IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    //RectTransformUtility.RectangleContainsScreenPoint ui충돌확인 함수
    public SlotState slotState;
    public ItemInfo itemInfo;

    //보이는 이미지
    public Image itemImage;



    //슬롯에 아이템 이미지 추가
    public void SlotSetImage()//UpdateSlotUI()
    {
        //itemImage.gameObject.SetActive(true);  // 슬롯 이미지 활성화
        itemImage.sprite = itemInfo.itemImage;  // 슬롯 이미지를 먹은 아이템 이미지로 변경
        SetColor(1);
    }




    //슬롯초기화
    public void ClearSlot()
    {
        slotState = SlotState.Blank;
        itemInfo = null; // 참조 null
        itemImage.sprite = null;
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
            if (slotState != SlotState.Blank)
            {
                if (itemInfo.itemType == ItemType.Equipment)
                {
                    Debug.Log("장착");
                }
            }
        }
    }


    /*//Debug.Log();
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotState != SlotState.Blank)
        {
            Debug.Log("드래그");
            SetColor(0); // 원래있던거 투명하게
            DragSlot.instance.dragSlot = this;  // 슬롯의 아이템정보, 이미지
            DragSlot.instance.DragSlotSetImage(itemImage.sprite); // 원래이미지를 드래그 슬롯에 이미지복사
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
        //Debug.Log("엔드드래그");
        //Slot temp = DragSlot.instance.dragSlot;
        DragSlot.instance.DragSlotClear(); //드래그 슬롯 이미지 끄기

        //temp.SetColor(1);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null) //드래그슬롯이 참조되어있으면
        {
            //Debug.Log("드롭");
            if (slotState != SlotState.Blank)  // 현재위치에 데이터가 있을때
            {
                Debug.Log("교체");
                ItemInfo temp = itemInfo; // 드롭한 위치의 데이터 저장

                if (temp.itemImage != null) //정보가잇엇으면
                {
                    //현재위치 데이터 이미지 입력
                    itemInfo = DragSlot.instance.dragSlot.itemInfo;
                    SlotSetImage();

                    //처음위치 데이터 이미지 입력
                    DragSlot.instance.dragSlot.itemInfo = temp;
                    DragSlot.instance.dragSlot.SlotSetImage();
                }
            }
            else if (slotState == SlotState.Blank)  // 현재위치에 데이터가 없을때
            {
                Debug.Log("이동");
                //현재위치에 데이터 입력
                itemInfo = DragSlot.instance.dragSlot.itemInfo;
                slotState = SlotState.Fill;
                SlotSetImage();

                //과거위치에 데이터 삭제
                DragSlot.instance.dragSlot.ClearSlot();
            }
        }
    }*/
}