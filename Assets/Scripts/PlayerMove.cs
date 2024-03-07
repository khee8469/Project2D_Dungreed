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
    [SerializeField] Transform effectPos;


    Vector2 moveDir;
    Vector3 mouseMove;
    Vector3 mousePos;
    Vector2 dash;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] float maxSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] float brakeSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float dashPower;
    [SerializeField] bool isGround;
    [SerializeField] bool isDash;
    Coroutine runEffect;

    public void Awake()
    {
        mousePos = new Vector3(0, 0, 10);
        Manager.Pool.CreatePool(jumpEffectPrefab, 4, 8);
    }

    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();

        if (isGround)
        {
            //���� ����Ʈ
            if (moveDir.x != 0)
            {
                runEffectPrefab.SetActive(true);
            }
            else
            {
                runEffectPrefab.SetActive(false);
            }

            //�޸��� �ִ�
            if (moveDir.x < 0)
            {
                animator.SetFloat("Run", Mathf.Abs(moveDir.x));
            }
            else if (moveDir.x > 0)
            {
                animator.SetFloat("Run", moveDir.x);
            }
            else
            {
                animator.SetFloat("Run", moveDir.x);
            }
        }
    }

    private void Move()
    {
        if (rigid.velocity.x < maxSpeed && rigid.velocity.x > -maxSpeed)
        {
            rigid.AddForce(Vector2.right * moveDir.x * moveSpeed * Time.deltaTime, ForceMode2D.Force);
            Debug.Log("������");
        }

        if (!isDash)
        {
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
            Jump();
        }
    }

    private void Jump()
    {
        //�÷����� ���ǿ��� ��������
        if(isGround && Input.GetKey(KeyCode.S))
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
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
        Debug.Log(cursor.position);
        if (transform.position.x < cursor.position.x)
        {
            //x�� �ݳѾ���� ������Ű�� ���
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
        //LookAt�� z�� �������� �ٶ󺻴�.
        //leftRotate.LookAt(cursor);
    }

    


    //�뽬
    private void OnRightMouse(InputValue value)
    {
        dash = cursor.position - transform.position;
        rigid.velocity = Vector2.zero;
        StartCoroutine(DashGravity());
    }

    IEnumerator DashGravity()
    {
        isDash = true;
        rigid.AddForce(dash.normalized * dashPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        rigid.velocity *= 0.3f;
        isDash = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((1<<collision.gameObject.layer & groundLayer) !=0) 
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
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
    
    void Start()
    {
        
    }

    void Update()
    {
        Move();
        Mouse();

    }

    
}
