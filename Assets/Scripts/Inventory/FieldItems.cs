using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public SpriteRenderer itemImage;
    public ItemInfo item;  // 게임시작시 SetItem 함수로 이름,이미지,타입등이 저장되어있는 상태



    public void SetItem(ItemInfo item)  // 게임시작시 ItemDatabase에 저장되어있던 데이터 입력
    {
        this.itemImage.sprite = item.itemImage;

        this.item.itemName = item.itemName;
        this.item.itemImage = item.itemImage;
        this.item.itemType = item.itemType;

        this.item.weaponNumber = item.weaponNumber;
        this.item.effect = item.effect;
        this.item.effectPlayTime = item.effectPlayTime;
        this.item.damage = item.damage;
        this.item.range = item.range;
        this.item.angleRange = item.angleRange;
        this.item.coolTime = item.coolTime;
    }
}

