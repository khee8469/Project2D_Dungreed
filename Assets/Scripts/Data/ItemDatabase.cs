using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public GameObject fieldItemPrefab;  //필드에 생성할 껍데기 프리팹
    public Vector2[] fieldItemPos;  // 생성위치

    private void Start()
    {
        
        //임시 생성
        for (int i = 0; i < fieldItemPos.Length; i++)
        {
            
            GameObject gameObject = Instantiate(fieldItemPrefab, fieldItemPos[i], Quaternion.identity);
            // 생성된 프리팹에 데이터베이스 데이터 입력
            int id = Random.Range(1, Manager.Resource.itemDic.Count);

            gameObject.GetComponent<FieldItems>().SetItem(Manager.Resource.itemDic[id].itemInfo.itemImage, Manager.Resource.itemDic[id].itemInfo.itemType, id);
        }
    }
}
