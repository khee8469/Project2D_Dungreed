using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [SerializeField] ItemData itemData;

    public GameObject fieldItemPrefab;  //�ʵ忡 ������ ������ ������
    public Vector2[] fieldItemPos;  // ������ġ

    private void Start()
    {
        
        //�ӽ� ����
        for (int i = 0; i < 6; i++)
        {
            
            GameObject gameObject = Instantiate(fieldItemPrefab, fieldItemPos[i], Quaternion.identity);
            // ������ �����տ� �����ͺ��̽� ������ �Է�
            gameObject.GetComponent<FieldItems>().SetItem(itemData.items[Random.Range(0,3)]);
        }
    }
}
