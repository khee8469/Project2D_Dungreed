using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public GameObject fieldItemPrefab;  //�ʵ忡 ������ ������ ������
    public Vector2[] fieldItemPos;  // ������ġ

    private void Start()
    {
        
        //�ӽ� ����
        for (int i = 0; i < fieldItemPos.Length; i++)
        {
            
            GameObject gameObject = Instantiate(fieldItemPrefab, fieldItemPos[i], Quaternion.identity);
            // ������ �����տ� �����ͺ��̽� ������ �Է�
            int id = Random.Range(1, Manager.Resource.itemDic.Count);

            gameObject.GetComponent<FieldItems>().SetItem(Manager.Resource.itemDic[id].itemInfo.itemImage, Manager.Resource.itemDic[id].itemInfo.itemType, id);
        }
    }
}
