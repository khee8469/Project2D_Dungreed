using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

//������ �޸��� �ִϸ��̼� �����ʿ�

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
    [SerializeField] LayerMask groundLayer; // �ٴ�Ȯ�ο� ���̾�
    [SerializeField] LayerMask targetLayer;

    [Header("Cheaker")]
    [SerializeField] bool isGround;
    [SerializeField] bool isDash;
    [SerializeField] bool attack;//���ݸ�� ��ȯ��

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


    State state = State.Idle; // �ʱ����
    Vector2 moveDir;  // �����Է�
    Vector3 mouseMove;  // ���콺��ġ �Է�
    Vector3 mousePos;  // ���콺 Z�� ������
    Vector2 dashNormalized; //�뽬����



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
        //���º�ȯ�� �ִϸ��̼� ���
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





        //���Ϳ� �÷��̾������ �Ÿ�
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
        Move(); // ���������϶� �ӵ� ����
        //Debug.Log("jump");
        




        if (moveDir.x == 0 && isGround && rigid.velocity.y < 0.01f) //������ ���� �ε��� �������°� ����,
        {                                                           //������ �������� 0���� ū��찡 �־ 0.01
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
        Debug.Log("��û���");

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
        //Debug.Log("����");
        //setactive false�ιٲٰ� ���� �̹����� �����صѱ�
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

        //�������϶�
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
            //��������Ʈ
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
        //rigid�� x�� ȸ������ �������� ��� �̲����� ����
        if (!isDash && moveDir.x == 0 && isGround)          //0001(1) |  0100(4) = 0101(5)
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else 
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        //�÷��̾��� �Ʒ� ���̾��� ������ ������
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, attackRange, groundLayer);
        RaycastHit2D frontHit = Physics2D.Raycast(frontCheak.position, frontCheak.right, 0.1f, groundLayer);

        if (hit || frontHit)
        {
            if (frontHit) // �� ������� üũ
                SlopeChk(frontHit);
            else if (hit)
                SlopeChk(hit);
        }


        Debug.DrawLine(hit.point, hit.point + perp, Color.red);
        Debug.DrawLine(hit.point, hit.point + hit.normal, Color.blue);
        Debug.DrawLine(frontCheak.position, frontCheak.position + frontCheak.right, Color.blue);

        //����϶�
        if (isSlope && isGround && state == State.Run && angle < maxAngle)
        {                 //Perpendicular���� -x���� ��ȯ�ϱ� ������ -1�� �����ش�.
            Debug.Log("���");
            /*if (hit && rigid.velocity.y < 0) 
            {
                transform.position = new Vector2(transform.position.x, hit.point.y);
            }*/
            rigid.velocity = moveDir.x * perp * -1 * speed;
        }
    }

    //��� ������, ��� üũ
    private void SlopeChk(RaycastHit2D hit)
    {
        //�Ű������� �ݽð�������� 90���ư� ���͸� ��ȯ == ����� ���
        perp = Vector2.Perpendicular(hit.normal).normalized;
        //RaycastHit2D�� �浹������ �������� nomal�� Vector2.up ���� ���� == ����� ����
        angle = Vector2.Angle(hit.normal, Vector2.up);

        if (angle != 0) // ���
        {
            isSlope = true;
        }
        else // ����
        {
            isSlope = false;
        }
    }

    //�̰ŵ� ���·� ������ �ֳ�?
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
        //�÷����� ���ǿ��� ��������
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
            //x�� �ݳѾ���� ������Ű�� ���
            leftRotate.transform.localScale = new Vector3(1, 1, 1);
            //�÷��̾� �̹�������
            spriteRenderer.flipX = false;
        }
        else if (transform.position.x > cursor.position.x)
        {
            leftRotate.transform.localScale = new Vector3(1, -1, 1);
            spriteRenderer.flipX = true;
        }

        //right������ ���콺������ �ٶ󺸰��ϱ�
        Vector2 dir = (cursor.position - leftRotate.position).normalized;
        leftRotate.right = dir;
    }


    //����
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

        // �� ����Ʈ Ǯ�� ���
        PooledObject pooledObject = Manager.Pool.GetPool(attactEffectPrefab, leftRotate.position, leftRotate.rotation);

        // ����Ʈ ���콺�������� ȸ��
        effectAngle = Mathf.Atan2(cursor.position.y - leftRotate.position.y, cursor.position.x - leftRotate.position.x) * Mathf.Rad2Deg;
        pooledObject.transform.rotation = Quaternion.AngleAxis(effectAngle, Vector3.forward);//forward(z�� ����)���� ȸ��

    }
    //����Ÿ������ �� ����
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


    //�뽬
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




    private void OnTriggerStay2D(Collider2D collision) // ��� �̲����� Stay�ιٲٴ� ��������
    {
        if ((1 << collision.gameObject.layer & groundLayer) != 0)
        {
            gameObject.layer = LayerMask.NameToLayer("Player"); // �ٿ������Ŀ� �ٽ� ���̾� ����
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
