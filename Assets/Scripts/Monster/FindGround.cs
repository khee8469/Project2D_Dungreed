using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGround : MonoBehaviour
{
    [SerializeField] LayerMask isjumpLayer;
    [SerializeField] public bool isJump;

    //���Ʒ� �������ִ���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.layer);
        //���� ���Ʒ��� �׶��� �浹ü�� �ִٸ�
        if ((1 << collision.gameObject.layer & isjumpLayer) != 0)
        {
            isJump = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //���� ���Ʒ��� �׶��� �浹ü�� �ִٸ�
        if ((1 << collision.gameObject.layer & isjumpLayer) != 0)
        {
            isJump = false;
        }
    }
}
