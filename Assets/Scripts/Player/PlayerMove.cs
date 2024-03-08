using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class PlayerMove : MonoBehaviour
{

    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] Transform cursor;
    [SerializeField] Transform leftRotate;
    [SerializeField] GameObject runEffectPrefab;
    [SerializeField] PooledObject jumpEffectPrefab;
    [SerializeField] PooledObject dashEffectPrefab;
    [SerializeField] Transform effectPos;
    [SerializeField] ParticleSystem ghostTrail;
    //
    Vector2 moveDir;
    Vector3 mouseMove;
    Vector3 mousePos;
    Vector2 dash;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] float Speed;
    [SerializeField] float brakeSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float dashPower;
    [SerializeField] bool isGround;
    [SerializeField] bool isDash;

    public void Awake()
    {
        mousePos = new Vector3(0, 0, 10);
        Manager.Pool.CreatePool(jumpEffectPrefab, 2, 4);
        Manager.Pool.CreatePool(dashEffectPrefab, 8, 16);
    }

    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();

        if (isGround)
        {
            if (moveDir.x < 0)
            {
                //달리기 애니
                animator.SetFloat("Run", Mathf.Abs(moveDir.x));
                //먼지이펙트
                runEffectPrefab.SetActive(true);
                effectPos.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (moveDir.x > 0)
            {
                animator.SetFloat("Run", moveDir.x);
                runEffectPrefab.SetActive(true);
                effectPos.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                animator.SetFloat("Run", moveDir.x);
                runEffectPrefab.SetActive(false);
            }
        }
    }

    private void Move()
    {
        if (!isDash)
        {
            if (moveDir.x > 0 && rigid.velocity.x < Speed)
            {
                rigid.velocity = new Vector2(Speed, rigid.velocity.y);
            }
            else if (moveDir.x < 0 && rigid.velocity.x > -Speed)
            {
                rigid.velocity = new Vector2(-Speed, rigid.velocity.y);
            }

            if (moveDir.x > 0 && rigid.velocity.x < -0.1f)
            {
                rigid.AddForce(Vector2.right * brakeSpeed);
            }
            else if (moveDir.x < 0 && rigid.velocity.x > 0.1f)
            {
                rigid.AddForce(Vector2.left * brakeSpeed);
            }
            else if (moveDir.x == 0 && rigid.velocity.x > 0.1f)
            {
                rigid.AddForce(Vector2.left * brakeSpeed);
            }
            else if (moveDir.x == 0 && rigid.velocity.x < -0.1f)
            {
                rigid.AddForce(Vector2.right * brakeSpeed);
            }
        }
    }

    private void OnJump(InputValue value)
    {
        if (isGround)
        {
            Manager.Pool.GetPool(jumpEffectPrefab, effectPos.position, effectPos.rotation);
            Jump();
        }
    }

    private void Jump()
    {
        //플랫포머 발판에서 내려가기
        if(isGround && Input.GetKey(KeyCode.S))
        {
            gameObject.layer = LayerMask.NameToLayer("DownJump");
        }
        else if (isGround)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            
        }
    }

    private void OnMouse(InputValue value)
    {
        mouseMove = value.Get<Vector2>();
    }

    private void Mouse()
    {
        cursor.position = Camera.main.ScreenToWorldPoint(mouseMove) + mousePos;

        if (transform.position.x < cursor.position.x)
        {
            //x축 반넘어갔을때 반전시키는 모션
            leftRotate.transform.localScale = new Vector3(1, 1, 1);
            spriteRenderer.flipX = false;
        }
        else if (transform.position.x > cursor.position.x)
        {
            leftRotate.transform.localScale = new Vector3(1, -1, 1);
            spriteRenderer.flipX = true;
        }

        Vector2 dir = (cursor.position - transform.position).normalized;
        leftRotate.right = dir;
        //LookAt은 z축 기준으로 바라본다.
        //leftRotate.LookAt(cursor);
    }


    //대쉬
    private void OnRightMouse(InputValue value)
    {
        isDash = true;
        dash = (cursor.position - transform.position).normalized;
        rigid.velocity = Vector2.zero;
        ghostTrail.gameObject.SetActive(true);
        StartCoroutine(DashGravity());
    }

    IEnumerator DashGravity()
    {
        rigid.AddForce(dash * dashPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        ghostTrail.gameObject.SetActive(false);
        rigid.velocity *= 0.3f;
        isDash = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((1<<collision.gameObject.layer & groundLayer) !=0) 
        {
            gameObject.layer = LayerMask.NameToLayer("Player"); // 다운점프후에 다시 레이어 변경
            animator.SetBool("Jump", false);
            isGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if((1 << collision.gameObject.layer &  groundLayer) != 0)
        {
            animator.SetBool("Jump", true);
            isGround = false;
        }
    }

    void Update()
    {
        Move();
        Mouse();
    }
}
