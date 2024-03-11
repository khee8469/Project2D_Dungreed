using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

//점프후 달리는 애니메이션 정지필요

public class PlayerMove : MonoBehaviour, IDamagable
{
    public enum State { Idle, Run, Jump, Dash, Die }

    [Header("PlayerMotion")]
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] Transform leftRotate;
    [SerializeField] Transform leftFlip;
    [SerializeField] float angle;
    [SerializeField] Vector2 perp;
    [SerializeField] bool isSlope;
    [SerializeField] float maxAngle;
    [SerializeField] Transform frontRayPoint;
    [SerializeField] Transform frontCheak;

    [Header("Effect")]
    [SerializeField] ParticleSystem ghostTrail;
    [SerializeField] PooledObject attactEffectPrefab;
    [SerializeField] PooledObject dashEffectPrefab;
    [SerializeField] Transform effectPos;
    [SerializeField] GameObject runEffectPrefab;
    [SerializeField] PooledObject jumpEffectPrefab;

    [Header("LayerMask")]
    [SerializeField] LayerMask groundLayer; // 바닥확인용 레이어
    [SerializeField] LayerMask targetLayer;

    [Header("Cheaker")]
    [SerializeField] bool isGround;
    [SerializeField] bool isDash;
    [SerializeField] bool attack;//공격모션 변환용

    [Header("PlayerState")]
    [SerializeField] int damage;
    [SerializeField] int hp;
    [SerializeField] float speed;
    //[SerializeField] float brakeSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float dashPower;

    [Header("AttackRange")]
    [SerializeField] Transform cursor;
    [SerializeField] int attackAngle;
    [SerializeField] float attackRange;
    float cosAngle;
    float effectAngle;


    State state = State.Idle; // 초기상태
    Vector2 moveDir;  // 방향입력
    Vector3 mouseMove;  // 마우스위치 입력
    Vector3 mousePos;  // 마우스 Z값 조정용
    Vector2 dashNormalized; //대쉬방향



    public void Awake()
    {
        mousePos = new Vector3(0, 0, 10);
        cosAngle = Mathf.Cos(attackAngle * Mathf.Deg2Rad);
        Manager.Pool.CreatePool(jumpEffectPrefab, 2, 4);
        Manager.Pool.CreatePool(attactEffectPrefab, 2, 4);
    }

    void Update()
    {
        Mouse();
        SlopeMove();

        switch (state)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Run:
                RunState();
                break;
            case State.Jump:
                JumpState();
                break;
            case State.Dash:
                DashState();
                break;
            case State.Die:
                DieState();
                break;
        }
    }

    

    public void ChangeState(State state)
    {
        //상태변환시 애니메이션 출력
        switch (state)
        {
            case State.Idle:
                animator.SetBool("Run", false);
                break;
            case State.Run:
                animator.SetBool("Run", true);
                break;
            case State.Jump:
                animator.SetBool("Jump", true);
                break;
            case State.Dash:
                runEffectPrefab.SetActive(false);
                StartCoroutine(DashGravity());
                break;
            case State.Die:
                animator.SetTrigger("Die");
                break;
        }
        this.state = state;
    }

    private void IdleState()
    {
        //Debug.Log("idle");





        //몬스터와 플레이어까지의 거리
        if (moveDir.x != 0 && isGround)
        {
            ChangeState(State.Run);
        }

        if (!isGround)
        {
            ChangeState(State.Jump);
        }

        if (isDash)
        {
            rigid.velocity = Vector2.zero;
            ChangeState(State.Dash);

        }

        if (hp <= 0)
        {
            ChangeState(State.Die);
        }
    }



    private void RunState()
    {
        //Debug.Log("Run");

        Move();
        MoveEffect();






        if (moveDir.x == 0 && isGround)
        {
            rigid.velocity = Vector2.zero;
            ChangeState(State.Idle);
        }

        if (!isGround)
        {
            ChangeState(State.Jump);
        }

        if (isDash)
        {
            rigid.velocity = Vector2.zero;
            ChangeState(State.Dash);
        }

        if (hp <= 0)
        {
            ChangeState(State.Die);
        }
    }

    private void JumpState()
    {
        Move(); // 점프상태일때 속도 조절
        //Debug.Log("jump");
        




        if (moveDir.x == 0 && isGround && rigid.velocity.y < 0.01f) //점프시 벽에 부딪혀 떨어지는거 방지,
        {                                                           //오차로 떨어질떄 0보다 큰경우가 있어서 0.01
            rigid.velocity = Vector2.zero;
            ChangeState(State.Idle);
        }

        if (moveDir.x != 0 && isGround)
        {
            ChangeState(State.Run);
        }

        if (isDash)
        {
            rigid.velocity = Vector2.zero;
            ChangeState(State.Dash);
        }

        if (hp <= 0)
        {
            ChangeState(State.Die);
        }
    }

    private void DashState()
    {
        Debug.Log("대시상태");

        /*if (!isDash && isGround && moveDir.x ==0)
        {
            rigid.velocity = Vector2.zero;
            ChangeState(State.Idle);
        }*/

        if (!isDash && isGround)
        {
            ChangeState(State.Run);
        }

        if (!isDash && !isGround)
        {
            ChangeState(State.Jump);
        }

        if (hp <= 0)
        {
            ChangeState(State.Die);
        }
    }


    private void DieState()
    {
        //Debug.Log("죽음");
        //setactive false로바꾸고 죽은 이미지만 생성해둘까
    }




    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }
    private void Move()
    {
        if (isGround)
        {
            if (moveDir.x > 0 && rigid.velocity.x < speed)
            {
                rigid.velocity = new Vector2(speed, rigid.velocity.y);
            }
            else if (moveDir.x < 0 && rigid.velocity.x > -speed)
            {
                rigid.velocity = new Vector2(-speed, rigid.velocity.y);
            }
        }

        //점프중일때
        if (!isGround)
        {
            if (moveDir.x > 0 && rigid.velocity.x < speed)
            {
                rigid.AddForce(Vector2.right * speed * 2, ForceMode2D.Force);
            }
            else if (moveDir.x < 0 && rigid.velocity.x > -speed)
            {
                rigid.AddForce(Vector2.left * speed * 2, ForceMode2D.Force);
            }
        }
    }
    private void MoveEffect()
    {
        if (moveDir.x < 0)
        {
            //먼지이펙트
            runEffectPrefab.SetActive(true);
            effectPos.transform.localScale = new Vector3(1, 1, 1);
            frontRayPoint.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (moveDir.x > 0)
        {
            runEffectPrefab.SetActive(true);
            effectPos.transform.localScale = new Vector3(-1, 1, 1);
            frontRayPoint.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            runEffectPrefab.SetActive(false);
        }
    }

    private void SlopeMove()
    {
        //rigid의 x와 회전값을 정지시켜 언덕 미끄러짐 방지
        if (!isDash && moveDir.x == 0 && isGround)          //0001(1) |  0100(4) = 0101(5)
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else 
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        //플레이어의 아래 레이어의 정보를 가져옴
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, attackRange, groundLayer);
        RaycastHit2D frontHit = Physics2D.Raycast(frontCheak.position, frontCheak.right, 0.1f, groundLayer);

        if (hit || frontHit)
        {
            if (frontHit) // 앞 언덕먼저 체크
                SlopeChk(frontHit);
            else if (hit)
                SlopeChk(hit);
        }


        Debug.DrawLine(hit.point, hit.point + perp, Color.red);
        Debug.DrawLine(hit.point, hit.point + hit.normal, Color.blue);
        Debug.DrawLine(frontCheak.position, frontCheak.position + frontCheak.right, Color.blue);

        //언덕일때
        if (isSlope && isGround && state == State.Run && angle < maxAngle)
        {                 //Perpendicular값이 -x값을 반환하기 때문에 -1을 곱해준다.
            Debug.Log("언덕");
            /*if (hit && rigid.velocity.y < 0) 
            {
                transform.position = new Vector2(transform.position.x, hit.point.y);
            }*/
            rigid.velocity = moveDir.x * perp * -1 * speed;
        }
    }

    //언덕 평면방향, 경사 체크
    private void SlopeChk(RaycastHit2D hit)
    {
        //매개변수의 반시계방향으로 90돌아간 벡터를 반환 == 언덕의 평면
        perp = Vector2.Perpendicular(hit.normal).normalized;
        //RaycastHit2D의 충돌지점의 법선벡터 nomal과 Vector2.up 사이 각도 == 언덕의 각도
        angle = Vector2.Angle(hit.normal, Vector2.up);

        if (angle != 0) // 언덕
        {
            isSlope = true;
        }
        else // 평지
        {
            isSlope = false;
        }
    }

    //이거도 상태로 넣을수 있나?
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
        if (isGround && Input.GetKey(KeyCode.S))
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
            //플레이어 이미지반전
            spriteRenderer.flipX = false;
        }
        else if (transform.position.x > cursor.position.x)
        {
            leftRotate.transform.localScale = new Vector3(1, -1, 1);
            spriteRenderer.flipX = true;
        }

        //right방향이 마우스방향을 바라보게하기
        Vector2 dir = (cursor.position - leftRotate.position).normalized;
        leftRotate.right = dir;
    }


    //공격
    private void OnLeftMouse(InputValue value)
    {
        Attack();
        AttactEffect();
    }

    private void AttactEffect()
    {
        if (!attack)
        {
            leftFlip.localScale = new Vector3(1, -1, 1);
            attack = true;
        }
        else if (attack)
        {
            leftFlip.localScale = new Vector3(1, 1, 1);
            attack = false;
        }

        // 검 이펙트 풀링 사용
        PooledObject pooledObject = Manager.Pool.GetPool(attactEffectPrefab, leftRotate.position, leftRotate.rotation);

        // 이펙트 마우스방향으로 회전
        effectAngle = Mathf.Atan2(cursor.position.y - leftRotate.position.y, cursor.position.x - leftRotate.position.x) * Mathf.Rad2Deg;
        pooledObject.transform.rotation = Quaternion.AngleAxis(effectAngle, Vector3.forward);//forward(z축 기준)으로 회전

    }
    //공격타겟지정 및 공격
    Collider2D[] colliders = new Collider2D[10];
    private void Attack()
    {
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, attackRange, colliders, targetLayer);
        for (int i = 0; i < size; i++)
        {
            Vector2 dir = (colliders[i].transform.position - transform.position).normalized;

            IDamagable monster = colliders[i].GetComponent<IDamagable>();
            if (Vector2.Dot(dir, cursor.position) > cosAngle)
            {
                monster.TakeDamage(damage);
            }
        }
    }


    //대쉬
    private void OnRightMouse(InputValue value)
    {
        isDash = true;
        //moveDir.x = cursor.position.x;
    }

    IEnumerator DashGravity()
    {
        dashNormalized = (cursor.position - transform.position).normalized;

        ghostTrail.gameObject.SetActive(true);
        rigid.AddForce(dashNormalized * dashPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        ghostTrail.gameObject.SetActive(false);
        rigid.velocity *= 0.3f;
        isDash = false;
    }




    private void OnTriggerStay2D(Collider2D collision) // 언덕 미끄러짐 Stay로바꾸니 괜찮아짐
    {
        if ((1 << collision.gameObject.layer & groundLayer) != 0)
        {
            gameObject.layer = LayerMask.NameToLayer("Player"); // 다운점프후에 다시 레이어 변경
            animator.SetBool("Jump", false);
            isGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer & groundLayer) != 0)
        {
            isGround = false;
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
