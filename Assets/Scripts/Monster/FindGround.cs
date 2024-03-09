using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGround : MonoBehaviour
{
    [SerializeField] LayerMask isjumpLayer;
    [SerializeField] public bool isJump;

    //위아래 발판이있는지
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.layer);
        //몬스터 위아래에 그라운드 충돌체가 있다면
        if ((1 << collision.gameObject.layer & isjumpLayer) != 0)
        {
            isJump = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //몬스터 위아래에 그라운드 충돌체가 있다면
        if ((1 << collision.gameObject.layer & isjumpLayer) != 0)
        {
            isJump = false;
        }
    }
}
