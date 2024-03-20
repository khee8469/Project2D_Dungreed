using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpChecker : MonoBehaviour
{
    [SerializeField] LayerMask isjumpLayer;
    [SerializeField] public bool isJump;

    //���Ʒ� �������ִ���
    private void OnTriggerStay2D(Collider2D collision)
    {
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
