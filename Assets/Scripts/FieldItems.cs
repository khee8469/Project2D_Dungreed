using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public Item item;  // 게임시작시 SetItem 함수로 이름,이미지,타입등이 저장되어있는 상태
    public SpriteRenderer image;

    public void SetItem(Item item)  // 게임시작시 ItemDatabase에 저장되어있던 데이터 입력
    {
        this.item.itemName = item.itemName;
        this.item.itemImage = item.itemImage;
        this.item.itemType = item.itemType;

        image.sprite = item.itemImage;
    }

    public Item GetItem()
    {
        return item; //SetItem 함수로 데이터 저장된 item
    }
}
