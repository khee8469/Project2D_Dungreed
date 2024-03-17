using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlotState { Blank, Fill }
public enum SlotKind { Equipment, Assistant, Accessory, Inventory }


//슬롯은 이미지만 바꾸는거엿구나
public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    //RectTransformUtility.RectangleContainsScreenPoint ui충돌확인 함수
    public SlotKind slotKind;
    public SlotState slotState;
    //public Equipments equipments;
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
        SetColor(0);
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
            //인벤토리에서 장비하기
            if (slotKind == SlotKind.Inventory)
            {
                //Debug.Log("인벤토리");
                if (itemInfo.itemType == ItemType.Equipment)
                {
                    //Debug.Log("장비");
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
                    else if (Manager.Game.inventoryUI.equipmentSlots[0].slotState == SlotState.Fill && Manager.Game.inventoryUI.equipmentSlots[1].slotState == SlotState.Fill)// 둘다 차있을때?
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
                    //Debug.Log("2");
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
                    else // 둘다 차있을때?
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
                    //Debug.Log("3");
                    for (int i = 0; i < Manager.Game.inventoryUI.accessorySlots.Length; i++)
                    {
                        if (Manager.Game.inventoryUI.accessorySlots[i].slotState == SlotState.Blank)
                        {

                            Manager.Game.inventoryUI.accessorySlots[i].itemInfo = this.itemInfo;
                            Manager.Game.inventoryUI.accessorySlots[i].SlotSetImage();
                            Manager.Game.inventoryUI.accessorySlots[i].slotState = SlotState.Fill;

                            ClearSlot();
                            return;
                        }
                    }
                }
            }


            // 장비한 아이템 뺴기
            if (slotState == SlotState.Fill && slotKind == SlotKind.Equipment || slotKind == SlotKind.Assistant || slotKind == SlotKind.Accessory)
            {
                for (int i = 0; i < Manager.Game.inventoryUI.slots.Length; i++)
                {
                    if (Manager.Game.inventoryUI.slots[i].slotState == SlotState.Blank)
                    {
                        //Debug.Log("데이터 입력");
                        //Slot에있는 Item 에 내가먹은 Item 정보를 넣음

                        Manager.Game.inventoryUI.slots[i].itemInfo = itemInfo;
                        Manager.Game.inventoryUI.slots[i].slotState = SlotState.Fill;
                        Manager.Game.inventoryUI.slots[i].SlotSetImage();
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
            //Debug.Log("드래그");
            SetColor(0); // 원래있던거 투명하게
            DragSlot.instance.dragSlot = this;  // 슬롯의 아이템정보, 이미지
            DragSlot.instance.DragSlotSetImage(itemImage.sprite); // 원래이미지를 드래그 슬롯에 이미지복사
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("엔드드래그");
        DragSlot.instance.DragSlotClear(); //드래그 슬롯 이미지 끄기
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null) //드래그슬롯이 참조되어있으면
        {
            //Debug.Log("드롭");
            if (slotState != SlotState.Blank)  // 현재위치에 데이터가 있을때
            {
                //Debug.Log("교체");
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
                //Debug.Log("이동");
                //현재위치에 데이터 입력
                itemInfo = DragSlot.instance.dragSlot.itemInfo;
                slotState = SlotState.Fill;
                SlotSetImage();

                //과거위치에 데이터 삭제
                DragSlot.instance.dragSlot.ClearSlot();
            }
        }
    }
}
