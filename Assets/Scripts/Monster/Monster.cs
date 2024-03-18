using System.Collections;
using UnityEngine;


public class Monster : MonoBehaviour, IDamagable
{
    public enum State { Idle, Trace, Jump, Attack, Die }


    [SerializeField] MonsterData monsterData;
    [SerializeField] int monsterNumber;
    [SerializeField] DamageText damageTextPrifab;
    [SerializeField] Transform damageTextPos;
    [SerializeField] float attackCoolTime;
    [SerializeField] bool isAttacking;
    [SerializeField] bool isGround;
    [SerializeField] bool isJump;
    private float cosRange;
    [SerializeField] int hp;

    [SerializeField] Transform player;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] FindGround groundCheck;

    [Header("Slope")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform frontRayPoint;
    [SerializeField] Transform frontCheak;
    [SerializeField] Vector2 perp;
    [SerializeField] float slopeCheak;
    [SerializeField] bool isSlope;



    private State state = State.Idle;


    private void Start()
    {
        hp = Manager.Resource.monsterDic[monsterNumber].monsterInfo.hp;
        cosRange = Mathf.Cos(Manager.Resource.monsterDic[monsterNumber].monsterInfo.attackAngle * Mathf.Deg2Rad);   //탐색 범위  cosRange == 0.5
    }

    private void Update()
    {
        if (attackCoolTime > -0.5f)
        {
            attackCoolTime -= Time.deltaTime;
        }


        switch (state)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Trace:
                TraceState();
                break;
            case State.Jump:
                JumpState();
                break;
            case State.Attack:
                AttackState();
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
                animator.Play("Idle");
                break;
            case State.Trace:
                animator.Play("Trace");
                break;
            case State.Jump:

                break;
            case State.Attack:
                animator.Play("Attack");
                break;
            case State.Die:
                animator.Play("Die");
                break;
        }
        this.state = state;
    }



    private void IdleState()
    {
        Debug.Log("idle");
        rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            

        //발견거리보다 가까우면
        if ((player.position - transform.position).sqrMagnitude < Manager.Resource.monsterDic[monsterNumber].monsterInfo.findRange * Manager.Resource.monsterDic[monsterNumber].monsterInfo.findRange)
        {
            ChangeState(State.Trace);
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        
        //사망
        if (hp <= 0)
        {
            ChangeState(State.Die);
        }
    }




    private void TraceState()
    {
        Debug.Log("trace");
        //언덕체크
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, groundLayer);
        RaycastHit2D frontHit = Physics2D.Raycast(frontCheak.position, frontCheak.right, 0.3f, groundLayer);
        if (hit || frontHit)
        {
            if (frontHit) // 앞 언덕먼저 체크
                SlopeChk(frontHit);
            else if (hit)
                SlopeChk(hit);
        }
        /*Debug.DrawLine(hit.point, hit.point + perp, Color.red);
        Debug.DrawLine(hit.point, hit.point + hit.normal, Color.blue);
        Debug.DrawLine(frontCheak.position, frontCheak.position + frontCheak.right, Color.blue);*/

        Vector2 dir = (player.position - transform.position).normalized;

        if(dir.x >0)  // frontCheak 좌우 방향용
            frontRayPoint.rotation = Quaternion.Euler(0, 0, 0);
        else
            frontRayPoint.rotation = Quaternion.Euler(0, 180, 0);

        Debug.Log(perp);

        //언덕일때
        if (dir.x >0 && isGround && isSlope && slopeCheak < Manager.Resource.monsterDic[monsterNumber].monsterInfo.maxAngle)
        {
            rigid.velocity = perp * -1 * Manager.Data.GameData.speed/4;
            spriteRenderer.flipX = false;
        }
        else if (dir.x < 0 && isGround && isSlope && slopeCheak < Manager.Resource.monsterDic[monsterNumber].monsterInfo.maxAngle)
        {
            rigid.velocity = perp  * Manager.Data.GameData.speed /4;
            spriteRenderer.flipX = true;
        }

        else if (dir.x > 0 && (player.position - transform.position).sqrMagnitude > Manager.Resource.monsterDic[monsterNumber].monsterInfo.attackRange * Manager.Resource.monsterDic[monsterNumber].monsterInfo.attackRange)
        {
            rigid.velocity = new Vector2(Manager.Resource.monsterDic[monsterNumber].monsterInfo.speed, rigid.velocity.y);
            spriteRenderer.flipX = false;
        }
        else if (dir.x < 0 && (player.position - transform.position).sqrMagnitude > Manager.Resource.monsterDic[0].monsterInfo.attackRange * Manager.Resource.monsterDic[monsterNumber].monsterInfo.attackRange)
        {
            rigid.velocity = new Vector2(-Manager.Resource.monsterDic[monsterNumber].monsterInfo.speed, rigid.velocity.y);
            spriteRenderer.flipX = true;
        }



        //언덕일때
        if (attackCoolTime > 0 && dir.x > 0 && isGround && isSlope && slopeCheak < Manager.Resource.monsterDic[monsterNumber].monsterInfo.maxAngle)
        {
            rigid.velocity = perp * -1 * Manager.Data.GameData.speed/4;
            spriteRenderer.flipX = false;
        }
        else if (attackCoolTime > 0 && dir.x < 0 && isGround && isSlope && slopeCheak < Manager.Resource.monsterDic[monsterNumber].monsterInfo.maxAngle)
        {
            rigid.velocity = perp * Manager.Data.GameData.speed/4;
            spriteRenderer.flipX = true;
        }
        else if (attackCoolTime > 0 && dir.x > 0 && (player.position - transform.position).sqrMagnitude < Manager.Resource.monsterDic[monsterNumber].monsterInfo.attackRange * Manager.Resource.monsterDic[monsterNumber].monsterInfo.attackRange)
        {
            rigid.velocity = new Vector2(Manager.Resource.monsterDic[monsterNumber].monsterInfo.speed, rigid.velocity.y);
            spriteRenderer.flipX = false;
        }
        else if (attackCoolTime > 0 && dir.x < 0 && (player.position - transform.position).sqrMagnitude < Manager.Resource.monsterDic[monsterNumber].monsterInfo.attackRange * Manager.Resource.monsterDic[monsterNumber].monsterInfo.attackRange)
        {
            rigid.velocity = new Vector2(-Manager.Resource.monsterDic[monsterNumber].monsterInfo.speed, rigid.velocity.y);
            spriteRenderer.flipX = true;
        }





        if ((player.position - transform.position).sqrMagnitude > Manager.Resource.monsterDic[monsterNumber].monsterInfo.findRange * Manager.Resource.monsterDic[monsterNumber].monsterInfo.findRange)
        {
            ChangeState(State.Idle);
        }

        //플레이어가 위에있고, 탐색가능하고, 발판이있을때
        if (!isJump && isGround && groundCheck.isJump && player.position.y > transform.position.y + 1 && (player.position - transform.position).sqrMagnitude < Manager.Resource.monsterDic[monsterNumber].monsterInfo.findRange * Manager.Resource.monsterDic[monsterNumber].monsterInfo.findRange)
        {
            ChangeState(State.Jump);
            StartCoroutine(JumpCoolTime());
        }
        //플레이어가 아래있고, 탐색가능하고, 발판이있을때
        else if (isGround && groundCheck.isJump && player.position.y < transform.position.y - 1 && (player.position - transform.position).sqrMagnitude < Manager.Resource.monsterDic[monsterNumber].monsterInfo.findRange * Manager.Resource.monsterDic[monsterNumber].monsterInfo.findRange)
        {
            ChangeState(State.Jump);
            StartCoroutine(DownJump());
        }

        //바닥위일때만 공격하기
        if (attackCoolTime < 0 && Vector2.Dot(transform.up, (player.position - transform.position).normalized) > cosRange && (player.position - transform.position).sqrMagnitude < Manager.Resource.monsterDic[monsterNumber].monsterInfo.attackRange * Manager.Resource.monsterDic[monsterNumber].monsterInfo.attackRange)
        {
            ChangeState(State.Attack);
            StartCoroutine(Attacking());
        }

        if (hp <= 0)
        {
            ChangeState(State.Die);
        }
    }




    private void JumpState()
    {
        //Debug.Log("점프상태");




        if ((player.position - transform.position).sqrMagnitude < Manager.Resource.monsterDic[monsterNumber].monsterInfo.findRange * Manager.Resource.monsterDic[monsterNumber].monsterInfo.findRange)
        {
            ChangeState(State.Trace);
        }

        //바닥위일때만 공격하기
        if (attackCoolTime < 0 && Vector2.Dot(transform.up, (player.position - transform.position).normalized) > cosRange && (player.position - transform.position).sqrMagnitude < Manager.Resource.monsterDic[monsterNumber].monsterInfo.attackRange * Manager.Resource.monsterDic[monsterNumber].monsterInfo.attackRange)
        {
            ChangeState(State.Attack);
            //공격딜레이
            StartCoroutine(Attacking());
        }

        if (hp <= 0)
        {
            ChangeState(State.Die);
        }
    }





    private void AttackState()
    {
        //Debug.Log("공격중");
        rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;


        //공격이 끝나면
        if (!isAttacking)
        {
            ChangeState(State.Trace);
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (hp <= 0)
        {
            ChangeState(State.Die);
        }
    }



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


    private void DieState()
    {
        Destroy(gameObject, 0.4f);
        //Debug.Log("죽음");
    }


    public void TakeDamage(int damage)
    {
        DamageText damageText = Instantiate(damageTextPrifab, damageTextPos.position, damageTextPos.rotation);
        damageText.damage = damage; // DamageText damageText에 표시할 데미지 입력
        //Debug.Log("데미지");
        hp -= damage;

    }

    //애니메이션 이벤트에 실행
    private void AttackTimeing()
    {
        //Debug.Log("공격");

        if (spriteRenderer.flipX)
        {
            //오른쪽기준 180도와 위쪽기준 180도에 겹치는 90도만 떄리기
            if (Vector2.Dot(transform.right * -1, (player.position - transform.position).normalized) > cosRange && Vector2.Dot(transform.up, (player.position - transform.position).normalized) > cosRange && (player.position - transform.position).sqrMagnitude < Manager.Resource.monsterDic[monsterNumber].monsterInfo.attackRange * Manager.Resource.monsterDic[monsterNumber].monsterInfo.attackRange)
            {
                //IDamagable 인터페이스를 가진 오브젝트면 데미지 적용
                IDamagable damagable = player.GetComponent<IDamagable>();
                damagable?.TakeDamage(Manager.Resource.monsterDic[monsterNumber].monsterInfo.damage);
            }
        }
        else if (!spriteRenderer.flipX)
        {
            if (Vector2.Dot(transform.right, (player.position - transform.position).normalized) > cosRange && Vector2.Dot(transform.up, (player.position - transform.position).normalized) > cosRange && (player.position - transform.position).sqrMagnitude < Manager.Resource.monsterDic[monsterNumber].monsterInfo.attackRange * Manager.Resource.monsterDic[monsterNumber].monsterInfo.attackRange)
            {
                IDamagable damagable = player.GetComponent<IDamagable>();
                damagable?.TakeDamage(Manager.Resource.monsterDic[monsterNumber].monsterInfo.damage);
            }
        }
    }


    IEnumerator JumpCoolTime()
    {
        isJump = true;
        rigid.AddForce(Vector2.up * Manager.Resource.monsterDic[monsterNumber].monsterInfo.jumpPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        isJump = false;
    }

    IEnumerator DownJump()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.layer = LayerMask.NameToLayer("DownJump");
        yield return new WaitForSeconds(0.5f);
        gameObject.layer = LayerMask.NameToLayer("Monster");
    }

    //공격쿨타임으로 변경 필요
    IEnumerator Attacking()
    {
        isAttacking = true;
        attackCoolTime = Manager.Resource.monsterDic[monsterNumber].monsterInfo.coolTime; // 공격쿨타임
        yield return new WaitForSeconds(Manager.Resource.monsterDic[monsterNumber].monsterInfo.attackTime); // 공격모션시간
        //공격범위안에서 뒤로갈떄 방향전환
        Vector2 dir = (player.position - transform.position).normalized;
        if (dir.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (dir.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        isAttacking = false;
    }


    //바닥인지 판단
    private void OnCollisionStay2D(Collision2D collision)
    {
        isGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGround = false;
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Manager.Resource.monsterDic[monsterNumber].monsterInfo.findRange);
        Gizmos.DrawWireSphere(transform.position, Manager.Resource.monsterDic[monsterNumber].monsterInfo.attackRange);
    }
}
