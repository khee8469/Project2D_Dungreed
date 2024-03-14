using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    //�̰� ��ũ��Ʈ ������ �����ͷ� �����ϸ� �ɵ�?
    public List<Item> itemDB = new List<Item>();  // ���� �̸� 3���־�� ����
    /*public ItemData itemData;
    public WeaponData WeaponData;
    public MonsterData monsterData;*/


    public GameObject fieldItemPrefab;  //�ʵ忡 ������ ������ ������
    public Vector2[] fieldItemPos;  // ������ġ

    private void Start()
    {
        //�ӽ� ����
        for (int i = 0; i < 6; i++)
        {
            GameObject gameObject = Instantiate(fieldItemPrefab, fieldItemPos[i], Quaternion.identity);
            // ������ �����տ� �����ͺ��̽� ������ �Է�
            gameObject.GetComponent<FieldItems>().SetItem(itemDB[Random.Range(0, 3)]);
        }
    }
}
