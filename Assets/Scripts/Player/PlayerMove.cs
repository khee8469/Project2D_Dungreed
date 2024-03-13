using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

//������ �޸��� �ִϸ��̼� �����ʿ�

public class PlayerMove : MonoBehaviour, IDamagable
{
    public enum State { Idle, Run, Jump, Dash, Die }

    [SerializeField] ItemData itemData;
    
    [Header("PlayerMotion")]
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] Transform leftRotate;
    [SerializeField] Transform leftFlip;
    [SerializeField] Transform leftHand;
    [SerializeField] float slopeCheak;
    [SerializeField] Vector2 perp;
    [SerializeField] bool isSlope;
    [SerializeField] float maxAngle;
    [SerializeField] Transform frontRayPoint;
    [SerializeField] Transform frontCheak;

    [Header("Effect")]
    [SerializeField] ParticleSystem ghostTrail;
    //[SerializeField] PooledObject attactEffectPrefab;
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
    [SerializeField] bool jumping;

    [Header("PlayerState")]
    [SerializeField] int hp;
    [SerializeField] int maxHp;
    [SerializeField] float speed;
    [SerializeField] float jumpPower;
    [SerializeField] float dashPower;

    [Header("AttackRange")]
    [SerializeField] Transform cursor;
    [SerializeField] int attackAngle;
    float cosAngle;

    [Header("Equipment")]
    [SerializeField] int firstEquipment;
    [SerializeField] int secondEquipment;

    [SerializeField] int equipemntNumber;
    [SerializeField] SpriteRenderer equipment;
    [SerializeField] float coolTime;

    State state = State.Idle; // �ʱ����
    Vector2 moveDir;  // �����Է�
    Vector2 mouseScrollDir;  // ���콺 ��ũ�� �Է�
    Vector3 mouseMove;  // ���콺��ġ �Է�
    Vector3 mousePos;  // ���콺 Z�� ������
    Vector2 dashNormalized; //�뽬����



    //MainScene mainScene; // ����� ������Ʈ ������

    public void Awake()
    {
        mousePos = new Vector3(0, 0, 10);
        cosAngle = Mathf.Cos(itemData.items[equipemntNumber].angleRange * Mathf.Deg2Rad);
        Manager.Pool.CreatePool(jumpEffectPrefab, 2, 4);
        //Manager.Pool.CreatePool(itemData.items[equipemntNumber].effect, 2, 4);
    
        
    }

    private void Start()
    {
        Manager.Game.hpBar.SetHp(hp,maxHp); // hp ����
        //mainScene = FindObjectOfType<MainScene>();
    }

    void Update()
    {
        Mouse();
        coolTime += Time.deltaTime;

        Item a = itemData.items[equipemntNumber].Weapon;
        equipment.sprite = a.transform.GetComponent<SpriteRenderer>().sprite;

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
        if (!isDash && moveDir.x == 0 && isGround)
            rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;




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

    private void EquipmentChangeState()
    {

    }


    private void DieState()
    {
        Debug.Log("����");
        //setactive false�ιٲٰ� ���� �̹����� �����صѱ�
    }




    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }
    private void Move()
    {
        //����
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

        //���
        //�÷��̾��� �Ʒ� ���̾��� ������ ������
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.3f, groundLayer);
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
        if (!jumping && isGround && isSlope && slopeCheak < maxAngle)
        {
            //Perpendicular���� -x���� ��ȯ�ϱ� ������ -1�� �����ش�.
            rigid.velocity = moveDir.x * perp * -1 * speed;
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


    //��� ������, ��� üũ
    private void SlopeChk(RaycastHit2D hit)
    {
        //�Ű������� �ݽð�������� 90���ư� ���͸� ��ȯ == ����� ���
        perp = Vector2.Perpendicular(hit.normal).normalized;
        //RaycastHit2D�� �浹������ �������� nomal�� Vector2.up ���� ���� == ����� ��絵
        slopeCheak = Vector2.Angle(hit.normal, Vector2.up);

        if (slopeCheak != 0) // ���
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
            StartCoroutine(JumpOn());
            //rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
            //rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);  // ����ö󰥶� �����վ velocity�� ��ü�ұ�
        }
    }

    private void OnMouse(InputValue value)
    {
        mouseMove = value.Get<Vector2>();
    }
    private void Mouse()
    {
        cursor.position = Camera.main.ScreenToWorldPoint(mouseMove) + mousePos;

        if (transform.position.x < cursor.position.x && equipemntNumber <4) //equipemntNumber <4 �ӽ� â��¤�� ����
        {
            //x�� �ݳѾ���� ������Ű�� ���
            leftRotate.transform.localScale = new Vector3(1, 1, 1);
            //�÷��̾� �̹�������
            spriteRenderer.flipX = false;
        }
        else if (transform.position.x > cursor.position.x && equipemntNumber < 4)
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
        if(coolTime >= itemData.items[equipemntNumber].coolTime)
        {
            Attack();
            AttactEffect();
            coolTime = 0;
        }
    }

    private void AttactEffect()
    {
        //�¿�� �ֵθ��� ����
        if (equipemntNumber < 4) // �ӽ� �׽�Ʈ
        {
            Debug.Log("�¿칫��");
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
        }


        
        //��� ����
        else
        {
            //â���� ������ �����Ϸ���
            leftHand.up = leftRotate.right;
            
        }


        // ����Ʈ ���콺�������� ȸ��
        Vector2 dir = (cursor.position - leftRotate.position).normalized;
        leftRotate.transform.right = dir;

        //������ ��Ÿ���ŭ�̵��Ѱ��� ����Ʈ ����
        //PooledObject pooledObject = Manager.Pool.GetPool(itemData.items[equipemntNumber].effect, leftRotate.position + (Vector3)(dir * (itemData.items[equipemntNumber].range/2)),leftRotate.rotation);
        GameObject abc = Instantiate(itemData.items[equipemntNumber].effect, leftRotate.position + (Vector3)(dir * (itemData.items[equipemntNumber].range / 2)), leftRotate.rotation);
        Destroy(abc, itemData.items[equipemntNumber].effectPlayTime);


        //effectAngle = Mathf.Atan2(cursor.position.y - leftRotate.position.y, cursor.position.x - leftRotate.position.x) * Mathf.Rad2Deg;
        //pooledObject.transform.rotation = Quaternion.AngleAxis(effectAngle, Vector3.forward);//forward(z�� ����)���� ȸ��

    }
    //����Ÿ������ �� ����
    Collider2D[] colliders = new Collider2D[10];
    private void Attack()
    {
        int size = Physics2D.OverlapCircleNonAlloc(leftRotate.position, itemData.items[equipemntNumber].range, colliders, targetLayer);
        for (int i = 0; i < size; i++)
        {
            Vector2 dir = (colliders[i].transform.position - leftRotate.position).normalized;

            IDamagable monster = colliders[i].GetComponent<IDamagable>();
            if (Vector2.Dot(dir, cursor.position) > cosAngle)
            {
                monster.TakeDamage(itemData.items[equipemntNumber].damage);
            }
        }
    }


    //�뽬
    private void OnRightMouse(InputValue value)
    {
        isDash = true;
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

    IEnumerator JumpOn()
    {
        jumping = true;
        rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
        yield return new WaitForSeconds(0.1f);
        jumping = false;
    }

    
    private void OnMouseScroll(InputValue value)
    {
        mouseScrollDir = value.Get<Vector2>();
        EquipmentChange();
    }

    

    //�ٷ� ���ü
    private void EquipmentChange()
    {
        if(mouseScrollDir.y > 0 || mouseScrollDir.y < 0)
        {
            if (equipemntNumber == firstEquipment)
                equipemntNumber = secondEquipment;
            else if(equipemntNumber != firstEquipment)
            {
                equipemntNumber = firstEquipment;
            }
        }
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
        Manager.Game.hpBar.Damage(hp);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(leftRotate.position, itemData.items[equipemntNumber].range);
        Gizmos.DrawLine(leftRotate.position, cursor.position);
    }
}
