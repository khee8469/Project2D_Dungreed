using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    //이걸 스크립트 아이템 데이터로 변경하면 될듯?
    public List<Item> itemDB = new List<Item>();  // 현재 미리 3개넣어둔 상태
    /*public ItemData itemData;
    public WeaponData WeaponData;
    public MonsterData monsterData;*/


    public GameObject fieldItemPrefab;  //필드에 생성할 껍데기 프리팹
    public Vector2[] fieldItemPos;  // 생성위치

    private void Start()
    {
        //임시 생성
        for (int i = 0; i < 6; i++)
        {
            GameObject gameObject = Instantiate(fieldItemPrefab, fieldItemPos[i], Quaternion.identity);
            // 생성된 프리팹에 데이터베이스 데이터 입력
            gameObject.GetComponent<FieldItems>().SetItem(itemDB[Random.Range(0, 3)]);
        }
    }
}
