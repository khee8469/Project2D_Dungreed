using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownJumpChecker : MonoBehaviour
{
    [SerializeField] LayerMask isjumpLayer;
    [SerializeField] public bool isDownJump;

    //위아래 발판이있는지
    private void OnTriggerStay2D(Collider2D collision)
    {
        //몬스터 위아래에 그라운드 충돌체가 있다면
        if ((1 << collision.gameObject.layer & isjumpLayer) != 0)
        {
            isDownJump = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //몬스터 위아래에 그라운드 충돌체가 있다면
        if ((1 << collision.gameObject.layer & isjumpLayer) != 0)
        {
            isDownJump = false;
        }
    }
}
