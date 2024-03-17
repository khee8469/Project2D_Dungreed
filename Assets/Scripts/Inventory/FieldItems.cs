using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public SpriteRenderer itemImage;
    public ItemInfo item;  // ���ӽ��۽� SetItem �Լ��� �̸�,�̹���,Ÿ�Ե��� ����Ǿ��ִ� ����



    public void SetItem(ItemInfo item)  // ���ӽ��۽� ItemDatabase�� ����Ǿ��ִ� ������ �Է�
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

