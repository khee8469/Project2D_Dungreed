using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMove : MonoBehaviour, IDamagable
{
    public enum State { Idle, Run, Jump, Dash, EquipmentChange, Die }

    [SerializeField] WeaponData WeaponData;

    [Header("PlayerMotion")]
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] Transform leftRotate;
    [SerializeField] Transform leftFlip;
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;
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
    [SerializeField] PooledObject runEffectPrefab;
    [SerializeField] PooledObject jumpEffectPrefab;

    [Header("LayerMask")]
    [SerializeField] LayerMask groundLayer; // �ٴ�Ȯ�ο� ���̾�
    [SerializeField] LayerMask targetLayer;
    [SerializeField] LayerMask itemLayer;

    [Header("Cheaker")]
    [SerializeField] bool isGround;
    [SerializeField] bool isDash;
    [SerializeField] bool weaponWield;//���ݸ�� ��ȯ��

    public bool WeaponWield { get { return weaponWield; } set { weaponWield = value; } }

    [SerializeField] bool jumping;  // ������� �����ϱ��

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
    [SerializeField] Transform equipmentPos;
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
        cosAngle = Mathf.Cos(WeaponData.weapons[equipemntNumber].angleRange * Mathf.Deg2Rad);
        Manager.Pool.CreatePool(jumpEffectPrefab, 2, 4);
        Manager.Pool.CreatePool(runEffectPrefab, 4, 8);
        //Manager.Pool.CreatePool(itemData.weapons[equipemntNumber].effect, 2, 4);


    }

    private void Start()
    {
        Manager.Game.hpBar.SetHp(hp, maxHp); // hp ����
        //mainScene = FindObjectOfType<MainScene>();

        EquipmentChangeState(); // �������
    }

    void Update()
    {
        Mouse();
        if (coolTime > 0)
        {
            coolTime -= Time.deltaTime;
        }


        //��� ��������Ʈ ��ü
        /*Item a = itemData.weapons[equipemntNumber].weapon;
        equipment.sprite = a.transform.GetComponent<SpriteRenderer>().sprite;*/

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
            case State.EquipmentChange:

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
                StartCoroutine(DashGravity());
                break;
            case State.EquipmentChange:

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
        //Debug.Log("��û���");

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

    private void OnMouseScroll(InputValue value)
    {
        mouseScrollDir = value.Get<Vector2>();
        EquipmentChange();
    }



    //�ٷ� ���ü
    private void EquipmentChange()
    {
        if (mouseScrollDir.y > 0 || mouseScrollDir.y < 0)
        {
            if (equipemntNumber == firstEquipment)
                equipemntNumber = secondEquipment;
            else if (equipemntNumber != firstEquipment)
            {
                equipemntNumber = firstEquipment;
            }
            EquipmentChangeState();
        }
    }

    private void EquipmentChangeState()
    {
        //Debug.Log("�������");
        //����ִ���������� �ı�
        if (equipmentPos.transform.childCount != 0)
        {
            Destroy(equipmentPos.GetComponentInChildren<Weapon>().gameObject);
        }

        //������ �ڽ����� �����
        Weapon weapon = Instantiate(WeaponData.weapons[equipemntNumber].weapon, equipmentPos.position, equipmentPos.rotation);
        weapon.transform.parent = equipmentPos.transform;
        //��ġ����
        weapon.transform.rotation = leftHand.rotation * Quaternion.Euler(WeaponData.weapons[equipemntNumber].weapon.WeaponPosition());
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

            effectPos.transform.localScale = new Vector3(1, 1, 1);
            frontRayPoint.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (moveDir.x > 0)
        {

            effectPos.transform.localScale = new Vector3(-1, 1, 1);
            frontRayPoint.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {

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

        if (transform.position.x < cursor.position.x) //equipemntNumber <4 �ӽ� â��¤�� ����
        {
            //x�� �ݳѾ���� ������Ű�� ���
            leftRotate.transform.localScale = new Vector3(1, 1, 1);
            //leftFlip.rotation = Quaternion.Euler(180, 0, 0);
            //�÷��̾� �̹�������
            spriteRenderer.flipX = false;
        }
        else if (transform.position.x > cursor.position.x)
        {
            leftRotate.transform.localScale = new Vector3(1, -1, 1);
            //leftFlip.rotation = Quaternion.Euler(0, 0, 0);
            spriteRenderer.flipX = true;
        }

        //right������ ���콺������ �ٶ󺸰��ϱ�
        Vector2 dir = (cursor.position - leftRotate.position).normalized;
        leftRotate.right = dir;
    }

    //WeaponData.weapons[equipemntNumber].weapon.WeaponAttack();
    //����
    private void OnLeftMouse(InputValue value)
    {
        //Debug.Log("���콺Ŭ��");
        if (coolTime < 0.1f)
        {
            Attack();
            AttactEffect();
            coolTime = WeaponData.weapons[equipemntNumber].coolTime;
        }
    }

    private void Attack()
    {
        //Debug.Log("����");
        int size = Physics2D.OverlapCircleNonAlloc(leftRotate.position, WeaponData.weapons[equipemntNumber].range, colliders, targetLayer);
        for (int i = 0; i < size; i++)
        {
            Vector2 dir = (colliders[i].transform.position - leftRotate.position).normalized;

            IDamagable monster = colliders[i].GetComponent<IDamagable>();
            if (Vector2.Dot(dir, cursor.position) > cosAngle)
            {
                monster.TakeDamage(WeaponData.weapons[equipemntNumber].damage);
            }
        }
    }

    private void AttactEffect()
    {
        //Debug.Log("����ȿ��");
        //�¿�� �ֵθ��� ����
        if (equipemntNumber < 4) // �ӽ� �׽�Ʈ
        {
            if (!weaponWield)
            {
                leftFlip.localScale = new Vector3(1, -1, 1);
                weaponWield = true;
            }
            else if (weaponWield)
            {
                leftFlip.localScale = new Vector3(1, 1, 1);
                weaponWield = false;
            }
        }

        else if(equipemntNumber >= 4)
        {
            StartCoroutine(SpearAttack());
        }





        // ����Ʈ ���콺�������� ȸ��
        Vector2 dir = (cursor.position - leftRotate.position).normalized;
        leftRotate.transform.right = dir;

        // ������ ��Ÿ���ŭ�̵��Ѱ��� ����Ʈ ����
        //PooledObject pooledObject = Manager.Pool.GetPool(WeaponData.items[equipemntNumber].effect, leftRotate.position + (Vector3)(dir * (WeaponData.items[equipemntNumber].range/2)),leftRotate.rotation);
        GameObject abc = Instantiate(WeaponData.weapons[equipemntNumber].effect, leftRotate.position + (Vector3)(dir * (WeaponData.weapons[equipemntNumber].range / 2)), leftRotate.rotation);
        Destroy(abc, WeaponData.weapons[equipemntNumber].effectPlayTime);
        //effectAngle = Mathf.Atan2(cursor.position.y - leftRotate.position.y, cursor.position.x - leftRotate.position.x) * Mathf.Rad2Deg;
        //pooledObject.transform.rotation = Quaternion.AngleAxis(effectAngle, Vector3.forward);//forward(z�� ����)���� ȸ��

    }
    //����Ÿ������ �� ����
    Collider2D[] colliders = new Collider2D[10];



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

    IEnumerator SpearAttack()
    {
        leftHand.Translate(Vector2.right * 3);
        yield return new WaitForSeconds(0.1f);
        leftHand.Translate(Vector2.left * 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("FieldItem"))
        if ((1 << collision.gameObject.layer & itemLayer) != 0)
        {
            FieldItems fieldItems = collision.GetComponent<FieldItems>();
            
            if (Manager.Game.inventoryUI.AddItem(fieldItems?.GetItem()))
            {
                Destroy(fieldItems.gameObject);
                //Destroy(fieldItems); //�̰� ������Ʈ�� �����ϴ°���
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
        Gizmos.DrawWireSphere(leftRotate.position, WeaponData.weapons[equipemntNumber].range);
        Gizmos.DrawLine(leftRotate.position, cursor.position);
    }
}
