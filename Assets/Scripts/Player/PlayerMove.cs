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
    [SerializeField] LayerMask groundLayer; // 바닥확인용 레이어
    [SerializeField] LayerMask targetLayer;
    [SerializeField] LayerMask itemLayer;

    [Header("Cheaker")]
    [SerializeField] bool isGround;
    [SerializeField] bool isDash;
    [SerializeField] bool weaponWield;//공격모션 변환용

    public bool WeaponWield { get { return weaponWield; } set { weaponWield = value; } }

    [SerializeField] bool jumping;  // 언덕에서 점프하기용

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

    State state = State.Idle; // 초기상태
    Vector2 moveDir;  // 방향입력
    Vector2 mouseScrollDir;  // 마우스 스크롤 입력
    Vector3 mouseMove;  // 마우스위치 입력
    Vector3 mousePos;  // 마우스 Z값 조정용
    Vector2 dashNormalized; //대쉬방향



    //MainScene mainScene; // 퍼즈시 업데이트 정지용

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
        Manager.Game.hpBar.SetHp(hp, maxHp); // hp 설정
        //mainScene = FindObjectOfType<MainScene>();

        EquipmentChangeState(); // 장비장착
    }

    void Update()
    {
        Mouse();
        if (coolTime > 0)
        {
            coolTime -= Time.deltaTime;
        }


        //장비 스프라이트 교체
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
        //Debug.Log("대시상태");

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



    //휠로 장비교체
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
        //Debug.Log("장비장착");
        //들고있는장비가잇으면 파괴
        if (equipmentPos.transform.childCount != 0)
        {
            Destroy(equipmentPos.GetComponentInChildren<Weapon>().gameObject);
        }

        //생성후 자식으로 만들기
        Weapon weapon = Instantiate(WeaponData.weapons[equipemntNumber].weapon, equipmentPos.position, equipmentPos.rotation);
        weapon.transform.parent = equipmentPos.transform;
        //위치조정
        weapon.transform.rotation = leftHand.rotation * Quaternion.Euler(WeaponData.weapons[equipemntNumber].weapon.WeaponPosition());
    }


    private void DieState()
    {
        Debug.Log("죽음");
        //setactive false로바꾸고 죽은 이미지만 생성해둘까
    }




    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }
    private void Move()
    {
        //평지
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

        //언덕
        //플레이어의 아래 레이어의 정보를 가져옴
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.3f, groundLayer);
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
        if (!jumping && isGround && isSlope && slopeCheak < maxAngle)
        {
            //Perpendicular값이 -x값을 반환하기 때문에 -1을 곱해준다.
            rigid.velocity = moveDir.x * perp * -1 * speed;
        }
    }

    private void MoveEffect()
    {
        if (moveDir.x < 0)
        {
            //먼지이펙트

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


    //언덕 평면방향, 경사 체크
    private void SlopeChk(RaycastHit2D hit)
    {
        //매개변수의 반시계방향으로 90돌아간 벡터를 반환 == 언덕의 평면
        perp = Vector2.Perpendicular(hit.normal).normalized;
        //RaycastHit2D의 충돌지점의 법선벡터 nomal과 Vector2.up 사이 각도 == 언덕의 경사도
        slopeCheak = Vector2.Angle(hit.normal, Vector2.up);

        if (slopeCheak != 0) // 언덕
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
            StartCoroutine(JumpOn());
            //rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
            //rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);  // 언덕올라갈때 문제잇어서 velocity로 교체할까
        }
    }

    private void OnMouse(InputValue value)
    {
        mouseMove = value.Get<Vector2>();
    }
    private void Mouse()
    {
        cursor.position = Camera.main.ScreenToWorldPoint(mouseMove) + mousePos;

        if (transform.position.x < cursor.position.x) //equipemntNumber <4 임시 창뒤짚힘 방지
        {
            //x축 반넘어갔을때 반전시키는 모션
            leftRotate.transform.localScale = new Vector3(1, 1, 1);
            //leftFlip.rotation = Quaternion.Euler(180, 0, 0);
            //플레이어 이미지반전
            spriteRenderer.flipX = false;
        }
        else if (transform.position.x > cursor.position.x)
        {
            leftRotate.transform.localScale = new Vector3(1, -1, 1);
            //leftFlip.rotation = Quaternion.Euler(0, 0, 0);
            spriteRenderer.flipX = true;
        }

        //right방향이 마우스방향을 바라보게하기
        Vector2 dir = (cursor.position - leftRotate.position).normalized;
        leftRotate.right = dir;
    }

    //WeaponData.weapons[equipemntNumber].weapon.WeaponAttack();
    //공격
    private void OnLeftMouse(InputValue value)
    {
        //Debug.Log("마우스클릭");
        if (coolTime < 0.1f)
        {
            Attack();
            AttactEffect();
            coolTime = WeaponData.weapons[equipemntNumber].coolTime;
        }
    }

    private void Attack()
    {
        //Debug.Log("공격");
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
        //Debug.Log("공격효과");
        //좌우로 휘두르는 무기
        if (equipemntNumber < 4) // 임시 테스트
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





        // 이펙트 마우스방향으로 회전
        Vector2 dir = (cursor.position - leftRotate.position).normalized;
        leftRotate.transform.right = dir;

        // 무기의 사거리만큼이동한곳에 이펙트 생성
        //PooledObject pooledObject = Manager.Pool.GetPool(WeaponData.items[equipemntNumber].effect, leftRotate.position + (Vector3)(dir * (WeaponData.items[equipemntNumber].range/2)),leftRotate.rotation);
        GameObject abc = Instantiate(WeaponData.weapons[equipemntNumber].effect, leftRotate.position + (Vector3)(dir * (WeaponData.weapons[equipemntNumber].range / 2)), leftRotate.rotation);
        Destroy(abc, WeaponData.weapons[equipemntNumber].effectPlayTime);
        //effectAngle = Mathf.Atan2(cursor.position.y - leftRotate.position.y, cursor.position.x - leftRotate.position.x) * Mathf.Rad2Deg;
        //pooledObject.transform.rotation = Quaternion.AngleAxis(effectAngle, Vector3.forward);//forward(z축 기준)으로 회전

    }
    //공격타겟지정 및 공격
    Collider2D[] colliders = new Collider2D[10];



    //대쉬
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
                //Destroy(fieldItems); //이건 컴포넌트만 삭제하는거임
            }
        }
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
        Manager.Game.hpBar.Damage(hp);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(leftRotate.position, WeaponData.weapons[equipemntNumber].range);
        Gizmos.DrawLine(leftRotate.position, cursor.position);
    }
}
