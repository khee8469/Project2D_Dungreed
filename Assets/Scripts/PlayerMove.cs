using System.Collections;
using System.Collections.Generic;
using System.Data;
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

    Vector3 moveDir;
    Vector3 mouseMove;
    Vector3 mousePos;
    Vector3 dash;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] float maxSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] float brakeSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float dashPower;
    [SerializeField] bool isGround;


    public void Awake()
    {
        mousePos = new Vector3(0, 0, 10);
    }

    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();

        if (isGround)
        {
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
        if(rigid.velocity.x < maxSpeed && rigid.velocity.x > -maxSpeed)
        {
            rigid.AddForce(Vector3.right * moveDir.x * moveSpeed, ForceMode2D.Force);
        }
       
        if(moveDir.x > 0 && rigid.velocity.x < -0.1f)
        {
            rigid.AddForce(Vector3.right * brakeSpeed);       
        }
        else if (moveDir.x < 0 && rigid.velocity.x > 0.1f)
        {
            rigid.AddForce(Vector3.left * brakeSpeed);
        }
        else if (moveDir.x == 0 && rigid.velocity.x > 0.1f)
        {
            rigid.AddForce(Vector3.left * brakeSpeed);
        }
        else if (moveDir.x == 0 && rigid.velocity.x < -0.1f)
        {
            rigid.AddForce(Vector3.right * brakeSpeed);
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
        if(isGround && Input.GetKey(KeyCode.S))
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            Debug.Log(gameObject.layer);
        }
        else if (isGround)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode2D.Impulse);
        }
        
    }

    private void OnMouse(InputValue value)
    {
        mouseMove = value.Get<Vector2>();
    }

    private void Mouse()
    {
        cursor.position = Camera.main.ScreenToWorldPoint(mouseMove)+ mousePos;
        if(transform.position.x < cursor.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (transform.position.x > cursor.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        leftRotate.LookAt(cursor);
    }

    private void OnLeftMouse(InputValue value)
    {
        //공격
    }

    private void OnRightMouse(InputValue value)
    {
        dash = cursor.position - transform.position;
        rigid.velocity = Vector3.zero;
        StartCoroutine(DashGravity());
    }

    IEnumerator DashGravity()
    {
        rigid.gravityScale = 0f;
        Debug.Log(dash.normalized);

        //rigid.velocity = dash.normalized * dashPower;
        rigid.AddForce(dash.normalized * dashPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        rigid.gravityScale = 1f;

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
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();  // 대쉬 좌우 감속 확인필요 
        Mouse();
    }
}
