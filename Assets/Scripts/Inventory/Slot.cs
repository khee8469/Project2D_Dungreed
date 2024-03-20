using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public enum SlotState { Blank, Fill }  // 아이템 ID가 -1이나 0 이면 비어있는거로 표기가능
public enum SlotKind { Equipment, Assistant, Accessory, Inventory }


//슬롯은 이미지만 바꾸는거엿구나
public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    //RectTransformUtility.RectangleContainsScreenPoint ui충돌확인 함수
    public int slotNumber;

    //보이는 이미지
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


    //슬롯데이터 입력
    public void SlotSet(Sprite image, ItemType type, int id)
    {
        slotImage.sprite = image;
        itemType = type;
        itemId = id;
        slotState = SlotState.Fill;
        SetColor(1);
    }

    //슬롯초기화
    public void ClearSlot()
    {
        slotImage.sprite = null;
        itemType = ItemType.Null;
        itemId = 0;
        slotState = SlotState.Blank;
        SetColor(0);
    }


    //투명도 조절
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
            //인벤토리에서 클릭
            if (slotKind == SlotKind.Inventory)
            {
                //Debug.Log("인벤토리");
                if (itemType == ItemType.Equipment)  // 메인 장비
                {
                    //Debug.Log("장비");
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
                    else if (inventoryUI.slots[0].slotState == SlotState.Fill && inventoryUI.slots[1].slotState == SlotState.Fill)// 둘다 차있을때?
                    {
                        //슬롯 데이터 저장
                        Sprite spriteTemp = inventoryUI.slots[0].slotImage.sprite;
                        ItemType typeTemp = inventoryUI.slots[0].itemType;
                        int idTemp = inventoryUI.slots[0].itemId;


                        //현재 데이터 슬롯에 입력
                        inventoryUI.slots[0].SlotSet(slotImage.sprite, itemType, itemId);
                        //현재 슬롯에 저장한 슬롯데이터 입력
                        SlotSet(spriteTemp, typeTemp, idTemp);
                    }
                }
                else if (itemType == ItemType.Assistant)  // 어시스트 장비
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
                    else // 둘다 차있을때?
                    {
                        Sprite ImageTemp = inventoryUI.slots[2].slotImage.sprite;
                        ItemType typeTemp = inventoryUI.slots[2].itemType;
                        int idTemp = inventoryUI.slots[2].itemId;
                        //현재 데이터 슬롯에 입력
                        inventoryUI.slots[2].SlotSet(slotImage.sprite, itemType, itemId);
                        //현재 슬롯에 저장한 슬롯데이터 입력
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

            // 장비한 아이템 뺴기
            if (slotState == SlotState.Fill && slotKind != SlotKind.Inventory)
            {
                for (int i = 8; i < inventoryUI.slots.Length-8; i++)
                {
                    if (inventoryUI.slots[i].slotState == SlotState.Blank)
                    {
                        //Debug.Log("데이터 입력");
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
            //Debug.Log("드래그");
            SetColor(0); // 원래있던거 투명하게
            DragSlot.instance.dragSlot = this;  // 슬롯의 정보 입력
            DragSlot.instance.DragSlotSetImage(slotImage.sprite); // 원래이미지를 드래그 슬롯에 이미지복사
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
        if (DragSlot.instance.dragSlot != null && !inventoryUI.overInventory) // 인벤토리 밖이면
        {
            //데이터 삭제는 팝업에서
            DragSlot.instance.slotImage.sprite = null;
            DragSlot.instance.SetColor(0);
            Manager.UI.ShowWindowUI(itemDestoryUI); // 팝업 실행
        }
        /*else if (DragSlot.instance.dragSlot != null && inventoryUI.overInventory) // 인벤토리 안
        {
            DragSlot.instance.dragSlot.SlotSet(slotImage.sprite, itemType, itemId);//원래슬롯 이미지 복구
            DragSlot.instance.DragSlotClear(); //드래그 슬롯 이미지 끄기
        }*/

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null) //드래그슬롯이 참조되어있으면
        {
            Image ImageTemp = slotImage;
            ItemType typeTemp = itemType;
            int idTemp = itemId;
            Debug.Log("드롭");
            if (slotState != SlotState.Blank)  // 현재위치에 데이터가 있을때
            {
                //Debug.Log("교체");
                //현재위치 데이터 이미지 입력
                SlotSet(DragSlot.instance.dragSlot.slotImage.sprite, DragSlot.instance.dragSlot.itemType, DragSlot.instance.dragSlot.itemId);

                //처음위치 데이터 이미지 입력
                DragSlot.instance.dragSlot.SlotSet(ImageTemp.sprite, typeTemp, idTemp);
                DragSlot.instance.DragSlotClear();
            }
            else if (slotState == SlotState.Blank)  // 현재위치에 데이터가 없을때
            {
                //Debug.Log("이동");
                //현재위치에 데이터 입력
                SlotSet(DragSlot.instance.dragSlot.slotImage.sprite, DragSlot.instance.dragSlot.itemType, DragSlot.instance.dragSlot.itemId);

                //과거위치에 데이터 삭제
                DragSlot.instance.dragSlot.ClearSlot();
            }
            else // 슬롯위가 아닌 인벤토리
            {
                DragSlot.instance.dragSlot.SlotSet(ImageTemp.sprite, typeTemp, idTemp);
                DragSlot.instance.DragSlotClear();
            }
        }
    }
}
