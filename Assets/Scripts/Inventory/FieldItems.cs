using UnityEngine;

public class Item : MonoBehaviour
{

    public Sprite image; // 이미지
    public ItemType type;
    public int id; // 데이터를 찾기위한 번호


    //public ItemData data;

    public SpriteRenderer itemImage;

    public void SetItem(Sprite image, ItemType type,int id)  // 게임시작시 ItemDatabase에 저장되어있던 데이터 입력
    {

        this.image = image;
        this.type = type;
        this.id = id;

        itemImage.sprite = image;
    }
}

