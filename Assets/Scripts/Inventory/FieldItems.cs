using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class FieldItems : MonoBehaviour
{
    public ItemInfo item;  // ���ӽ��۽� SetItem �Լ��� �̸�,�̹���,Ÿ�Ե��� ����Ǿ��ִ� ����
    public SpriteRenderer itemImage;


    public void SetItem(ItemInfo item)  // ���ӽ��۽� ItemDatabase�� ����Ǿ��ִ� ������ �Է�
    {
        this.item.itemName = item.itemName;
        this.item.itemImage = item.itemImage;
        this.item.itemType = item.itemType;

        this.itemImage.sprite = item.itemImage;
    }
}
