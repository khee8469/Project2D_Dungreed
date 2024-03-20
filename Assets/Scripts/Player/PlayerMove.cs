using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMove : MonoBehaviour, IDamagable
{
    public enum State { Idle, Run, Jump, Dash, EquipmentChange, Die }


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
    [SerializeField] Transform frontRayPoint;
    [SerializeField] Transform frontCheak;

    [Header("Effect")]
    [SerializeField] ParticleSystem ghostTrail;
    //[SerializeField] PooledObject attactEffectPrefab;
    [SerializeField] PooledObject dashEffectPrefab;
    [SerializeField] Transform effectPos;
    [SerializeField] PooledObject runEffectPrefab;
    [SerializeField] PooledObject jumpEffectPrefab;
    [SerializeField] DamageText damageTextPrifab;
    [SerializeField] Transform damageTextPos;

    [SerializeField] HpBar hpBar;

    [Header("LayerMask")]
    [SerializeField] LayerMask groundLayer; // �ٴ�Ȯ�ο� ���̾�
    [SerializeField] LayerMask targetLayer;
    [SerializeField] LayerMask itemLayer;

    [Header("Cheaker")]
    [SerializeField] bool isGround;
    [SerializeField] bool isDash;
    [SerializeField] bool weaponWield;//���ݸ�� ��ȯ��
    //public bool WeaponWield { get { return weaponWield; } set { weaponWield = value; } }
    [SerializeField] bool jumping;  // ������� �����ϱ��
    [SerializeField] Transform cursor;

    [Header("Equipment")]
    //[SerializeField] int EquipNumChange;
    [SerializeField] SpriteRenderer equipmentImage;
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] int curEquipemnt;
    [SerializeField] float coolTime;


    State state = State.Idle; // �ʱ����
    Vector2 moveDir;  // �����Է�
    Vector2 mouseScrollDir;  // ���콺 ��ũ�� �Է�
    Vector3 mouseMove;  // ���콺��ġ �Է�
    Vector3 mousePos;  // ���콺 Z�� ������
    Vector2 dashNormalized; //�뽬����



    public void Awake()
    {
        mousePos = new Vector3(0, 0, 10);
        Manager.Pool.CreatePool(jumpEffectPrefab, 2, 4);
        Manager.Pool.CreatePool(runEffectPrefab, 4, 8);
        inventoryUI = FindObjectOfType<InventoryUI>();
    }


    void Update()
    {
        if (Time.timeScale == 0f) // �Ͻ������� ���콺�̵�����
        {
            return;
        }

        Mouse();


        if (coolTime > 0)
        {
            coolTime -= Time.deltaTime;
        }

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

        EquipmentScrollChange();  // ��ũ�ѷ� ���� �̹��� ����
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

        if (Manager.Data.GameData.hp <= 0)
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

        if (Manager.Data.GameData.hp <= 0)
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

        if (Manager.Data.GameData.hp <= 0)
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

        if (Manager.Data.GameData.hp <= 0)
        {
            ChangeState(State.Die);
        }
    }

    //�ٷ� ���ü
    private void OnMouseScroll(InputValue value)
    {
        mouseScrollDir = value.Get<Vector2>();
        if (mouseScrollDir.y > 0)
        {
            curEquipemnt = 0;
        }
        else if (mouseScrollDir.y < 0)
        {
            curEquipemnt = 1;
        }
    }

    //��ũ�ѷ� ����̹����� ��ü
    private void EquipmentScrollChange()
    {
        if (curEquipemnt == 0)
        {
            equipmentImage.sprite = inventoryUI.slots[0].slotImage.sprite;
        }
        else if (curEquipemnt == 1)
        {
            equipmentImage.sprite = inventoryUI.slots[1].slotImage.sprite;
        }
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
            if (moveDir.x > 0 && rigid.velocity.x < Manager.Data.GameData.speed)
            {
                rigid.velocity = new Vector2(Manager.Data.GameData.speed, rigid.velocity.y);
            }
            else if (moveDir.x < 0 && rigid.velocity.x > -Manager.Data.GameData.speed)
            {
                rigid.velocity = new Vector2(-Manager.Data.GameData.speed, rigid.velocity.y);
            }
        }

        //�������϶�
        if (!isGround)
        {
            if (moveDir.x > 0 && rigid.velocity.x < Manager.Data.GameData.speed)
            {
                rigid.AddForce(Vector2.right * Manager.Data.GameData.speed * 2, ForceMode2D.Force);
            }
            else if (moveDir.x < 0 && rigid.velocity.x > -Manager.Data.GameData.speed)
            {
                rigid.AddForce(Vector2.left * Manager.Data.GameData.speed * 2, ForceMode2D.Force);
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
        if (!jumping && isGround && isSlope && slopeCheak < Manager.Data.GameData.maxAngle)
        {
            //Perpendicular���� -x���� ��ȯ�ϱ� ������ -1�� �����ش�.
            rigid.velocity = moveDir.x * perp * -1 * Manager.Data.GameData.speed;
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

        //Manager.Data.GameData.

    }
    private void Mouse()
    {
        cursor.position = Camera.main.ScreenToWorldPoint(mouseMove) + mousePos;

        //if (curEquipemnt == 0) //0�������϶�
        //{
        if (transform.position.x < cursor.position.x)
        {
            //x�� �ݳѾ���� ������Ű�� ���
            if (inventoryUI.slots[0].itemId < 2)
            {
                leftRotate.transform.localScale = new Vector3(1, 1, 1);
            }
            //�÷��̾� �̹�������
            spriteRenderer.flipX = false;
        }
        else if (transform.position.x > cursor.position.x)
        {
            if (inventoryUI.slots[0].itemId < 2)
            {
                leftRotate.transform.localScale = new Vector3(1, -1, 1);
            }
            //leftFlip.rotation = Quaternion.Euler(0, 0, 0);
            spriteRenderer.flipX = true;
        }

        //right������ ���콺������ �ٶ󺸰��ϱ�
        Vector2 dir = (cursor.position - leftRotate.position).normalized;
        leftRotate.right = dir;
    }

    //����
    private void OnLeftMouse(InputValue value)
    {
        //Debug.Log("���콺Ŭ��");
        if (!inventoryUI.overInventory && coolTime < 0.1f)
        {
            Attack();
        }
    }


    //����Ÿ������ �� ����
    Collider2D[] colliders = new Collider2D[10];
    private void Attack()
    {
        //Debug.Log("����");
        if (inventoryUI.slots[curEquipemnt].itemId > 0) // �Ǽ��� �ƴҶ�
        {
            int size = Physics2D.OverlapCircleNonAlloc(leftRotate.position, Manager.Resource.itemDic[inventoryUI.slots[curEquipemnt].itemId].itemInfo.range, colliders, targetLayer);
            for (int i = 0; i < size; i++)
            {
                //���� ������ �߹ٴ��̶� �´� ������ �߾ӱ��������ϰ�
                Vector3 monDirOffset = colliders[i].transform.position;
                monDirOffset.y += 0.7f;

                //���͹���
                Vector2 monDir = (monDirOffset - leftRotate.position).normalized;
                //���콺����
                Vector2 curDir = (cursor.position - leftRotate.position).normalized;
                IDamagable monster = colliders[i].GetComponent<IDamagable>();

                if (Vector3.Dot(monDir, curDir) > Mathf.Cos(Manager.Resource.itemDic[inventoryUI.slots[curEquipemnt].itemId].itemInfo.angleRange * Mathf.Deg2Rad))
                {
                    monster.TakeDamage(Manager.Resource.itemDic[inventoryUI.slots[curEquipemnt].itemId].itemInfo.damage);
                }
            }
            AttactEffect();
            coolTime = Manager.Resource.itemDic[inventoryUI.slots[curEquipemnt].itemId].itemInfo.coolTime;
        }
    }

    private void AttactEffect()
    {
        //Debug.Log("����ȿ��");

        //�¿�� �ֵθ��� ����, ��� ����

        if (inventoryUI.slots[curEquipemnt].itemId < 3) // �ӽ� �׽�Ʈ
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

        else if (inventoryUI.slots[curEquipemnt].itemId >= 3)
        {
            StartCoroutine(SpearAttack());
        }


        // ����Ʈ
        // ���콺�������� ȸ��
        Vector2 dir = (cursor.position - leftRotate.position).normalized;
        leftRotate.transform.right = dir;

        if (inventoryUI.slots[curEquipemnt].itemId > 0)
        {
            GameObject abc = Instantiate(Manager.Resource.itemDic[inventoryUI.slots[curEquipemnt].itemId].itemInfo.effect,
            leftRotate.position + (Vector3)(dir * (Manager.Resource.itemDic[inventoryUI.slots[curEquipemnt].itemId].itemInfo.range / 2)), leftRotate.rotation);
            Destroy(abc, Manager.Resource.itemDic[inventoryUI.slots[curEquipemnt].itemId].itemInfo.effectPlayTime);
        }


        //effectAngle = Mathf.Atan2(cursor.position.y - leftRotate.position.y, cursor.position.x - leftRotate.position.x) * Mathf.Rad2Deg;
        //pooledObject.transform.rotation = Quaternion.AngleAxis(effectAngle, Vector3.forward);//forward(z�� ����)���� ȸ��
    }


    //�뽬
    private void OnRightMouse(InputValue value)
    {
        if (!inventoryUI.overInventory)
        {
            isDash = true;
        }
    }

    IEnumerator DashGravity()
    {
        dashNormalized = (cursor.position - transform.position).normalized;

        ghostTrail.gameObject.SetActive(true);
        rigid.AddForce(dashNormalized * Manager.Data.GameData.dashPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        ghostTrail.gameObject.SetActive(false);
        rigid.velocity *= 0.3f;
        isDash = false;
    }

    IEnumerator JumpOn()
    {
        jumping = true;
        rigid.velocity = new Vector2(rigid.velocity.x, Manager.Data.GameData.jumpPower);
        yield return new WaitForSeconds(0.1f);
        jumping = false;
    }

    IEnumerator SpearAttack()
    {
        leftHand.Translate(Vector2.right * 2.5f);
        yield return new WaitForSeconds(0.1f);
        leftHand.Translate(Vector2.left * 2.5f);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer & itemLayer) != 0)
        {
            Item item = collision.GetComponent<Item>();
            //�κ��丮�� ������������ ������ ����
            if (inventoryUI.AddItemData(item))
            {
                Destroy(item.gameObject);
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
        DamageText damageText = Instantiate(damageTextPrifab, damageTextPos.position, damageTextPos.rotation);
        damageText.damage = damage;
        Manager.Data.GameData.hp -= damage;
        hpBar.SetHp(Manager.Data.GameData.hp, Manager.Data.GameData.maxHp);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (inventoryUI.slots[curEquipemnt].itemId > -1) //����� -1�� ����
        {
            Gizmos.DrawWireSphere(leftRotate.position, Manager.Resource.itemDic[inventoryUI.slots[curEquipemnt].itemId].itemInfo.range);
        }
        Gizmos.DrawLine(leftRotate.position, cursor.position);
    }
}
