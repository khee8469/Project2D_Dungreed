using UnityEngine;

public class Item : MonoBehaviour
{

    public Sprite image; // �̹���
    public ItemType type;
    public int id; // �����͸� ã������ ��ȣ


    //public ItemData data;

    public SpriteRenderer itemImage;

    public void SetItem(Sprite image, ItemType type,int id)  // ���ӽ��۽� ItemDatabase�� ����Ǿ��ִ� ������ �Է�
    {

        this.image = image;
        this.type = type;
        this.id = id;

        itemImage.sprite = image;
    }
}

