using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public Item item;  // ���ӽ��۽� SetItem �Լ��� �̸�,�̹���,Ÿ�Ե��� ����Ǿ��ִ� ����
    public SpriteRenderer image;

    public void SetItem(Item item)  // ���ӽ��۽� ItemDatabase�� ����Ǿ��ִ� ������ �Է�
    {
        this.item.itemName = item.itemName;
        this.item.itemImage = item.itemImage;
        this.item.itemType = item.itemType;

        image.sprite = item.itemImage;
    }

    public Item GetItem()
    {
        return item; //SetItem �Լ��� ������ ����� item
    }
}
