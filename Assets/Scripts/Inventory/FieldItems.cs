using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public Sprite Image; // �̹���
    public ItemType type;
    public int id; // �����͸� ã������ ��ȣ

    public SpriteRenderer itemImage;

    public void SetItem(Sprite Image,ItemType type,int id)  // ���ӽ��۽� ItemDatabase�� ����Ǿ��ִ� ������ �Է�
    {
        this.Image = Image;
        this.type = type;
        this.id = id;

        itemImage.sprite = Image;
    }
}

