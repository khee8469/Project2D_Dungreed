using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class FieldItems : MonoBehaviour
{
    public ItemInfo item;  // 게임시작시 SetItem 함수로 이름,이미지,타입등이 저장되어있는 상태
    public SpriteRenderer itemImage;


    public void SetItem(ItemInfo item)  // 게임시작시 ItemDatabase에 저장되어있던 데이터 입력
    {
        this.item.itemName = item.itemName;
        this.item.itemImage = item.itemImage;
        this.item.itemType = item.itemType;

        this.itemImage.sprite = item.itemImage;
    }
}
