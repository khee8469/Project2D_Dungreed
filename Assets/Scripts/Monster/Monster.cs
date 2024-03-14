using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class Monster : MonoBehaviour, IDamagable
{
    public enum State { Idle, Trace, Jump, Attack, Die }

    [SerializeField] MonsterData monsterData;
    [SerializeField] int monsterNumber;

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



    private State state = State.Idle;


    private void Awake()
    {
        hp = monsterData.monsters[monsterNumber].hp;
        cosRange = Mathf.Cos(monsterData.monsters[monsterNumber].attackAngle * Mathf.Deg2Rad);   //탐색 범위  cosRange == 0.5
    }

    private void Update()
    {
        attackCoolTime -= Time.deltaTime;

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
        //Debug.Log("idle");


        //발견거리보다 가까우면
        if ((player.position - transform.position).sqrMagnitude < monsterData.monsters[monsterNumber].findRange * monsterData.monsters[monsterNumber].findRange)
        {
            ChangeState(State.Trace);
        }
        //사망
        if (hp <= 0)
        {
            ChangeState(State.Die);
        }
    }




    private void TraceState()
    {
        //Debug.Log("trace");
        

        Vector2 dir = (player.position - transform.position).normalized;

        if (dir.x > 0 && (player.position - transform.position).sqrMagnitude > monsterData.monsters[monsterNumber].attackRange * monsterData.monsters[monsterNumber].attackRange)
        {
            if (dir.x > 0 && rigid.velocity.x < monsterData.monsters[monsterNumber].speed)
            {
                rigid.velocity = new Vector2(monsterData.monsters[monsterNumber].speed, rigid.velocity.y);
                //rigid.AddForce(Vector3.right);
            }
            spriteRenderer.flipX = false;
        }
        else if (dir.x < 0 && (player.position - transform.position).sqrMagnitude > monsterData.monsters[monsterNumber].attackRange * monsterData.monsters[monsterNumber].attackRange)
        {
            if (dir.x < 0 && rigid.velocity.x > -monsterData.monsters[monsterNumber].speed)
            {
                rigid.velocity = new Vector2(-monsterData.monsters[monsterNumber].speed, rigid.velocity.y);
            }
            spriteRenderer.flipX = true;
        }

        //공격범위안이지만 공격쿨타임일때
        if (attackCoolTime > 0 && dir.x > 0 && (player.position - transform.position).sqrMagnitude < monsterData.monsters[monsterNumber].attackRange * monsterData.monsters[monsterNumber].attackRange)
        {
            if (dir.x > 0 && rigid.velocity.x < monsterData.monsters[monsterNumber].speed)
            {
                rigid.velocity = new Vector2(monsterData.monsters[monsterNumber].speed, rigid.velocity.y);
            }
            spriteRenderer.flipX = false;
        }
        else if (attackCoolTime > 0 && dir.x < 0 && (player.position - transform.position).sqrMagnitude < monsterData.monsters[monsterNumber].attackRange * monsterData.monsters[monsterNumber].attackRange)
        {
            if (dir.x < 0 && rigid.velocity.x > -monsterData.monsters[monsterNumber].speed)
            {
                rigid.velocity = new Vector2(-monsterData.monsters[monsterNumber].speed, rigid.velocity.y);
            }
            spriteRenderer.flipX = true;
        }




        if ((player.position - transform.position).sqrMagnitude > monsterData.monsters[monsterNumber].findRange * monsterData.monsters[monsterNumber].findRange)
        {
            ChangeState(State.Idle);
        }

        //플레이어가 위에있고, 탐색가능하고, 발판이있을때
        if (!isJump && isGround && groundCheck.isJump && player.position.y > transform.position.y + 1 && (player.position - transform.position).sqrMagnitude < monsterData.monsters[monsterNumber].findRange * monsterData.monsters[monsterNumber].findRange)
        {
            ChangeState(State.Jump);
            StartCoroutine(JumpCoolTime());
        }
        //플레이어가 아래있고, 탐색가능하고, 발판이있을때
        else if (isGround && groundCheck.isJump && player.position.y < transform.position.y -1 &&  (player.position - transform.position).sqrMagnitude < monsterData.monsters[monsterNumber].findRange * monsterData.monsters[monsterNumber].findRange)
        {
            ChangeState(State.Jump);
            StartCoroutine(DownJump());
        }

        //바닥위일때만 공격하기
        if (attackCoolTime < 0 && Vector2.Dot(transform.up, (player.position - transform.position).normalized) > cosRange && (player.position - transform.position).sqrMagnitude < monsterData.monsters[monsterNumber].attackRange * monsterData.monsters[monsterNumber].attackRange)
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


        

        if ((player.position - transform.position).sqrMagnitude < monsterData.monsters[monsterNumber].findRange * monsterData.monsters[monsterNumber].findRange)
        {
            ChangeState(State.Trace);
        }

        //바닥위일때만 공격하기
        if (attackCoolTime < 0 && Vector2.Dot(transform.up, (player.position - transform.position).normalized) > cosRange && (player.position - transform.position).sqrMagnitude < monsterData.monsters[monsterNumber].attackRange * monsterData.monsters[monsterNumber].attackRange)
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



        //공격이 끝나면
        if (!isAttacking)
        {
            ChangeState(State.Trace);
        }

        if (hp <= 0)
        {
            ChangeState(State.Die);
        }
    }




    private void DieState()
    {
        Destroy(gameObject, 0.4f);
        //Debug.Log("죽음");
    }


    public void TakeDamage(int damage)
    {
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
            if (Vector2.Dot(transform.right * -1, (player.position - transform.position).normalized) > cosRange && Vector2.Dot(transform.up, (player.position - transform.position).normalized) > cosRange && (player.position - transform.position).sqrMagnitude < monsterData.monsters[monsterNumber].attackRange * monsterData.monsters[monsterNumber].attackRange)
            {
                //IDamagable 인터페이스를 가진 오브젝트면 데미지 적용
                IDamagable damagable = player.GetComponent<IDamagable>();
                damagable?.TakeDamage(monsterData.monsters[monsterNumber].damage);
            }
        }
        else if (!spriteRenderer.flipX)
        {
            if (Vector2.Dot(transform.right, (player.position - transform.position).normalized) > cosRange && Vector2.Dot(transform.up, (player.position - transform.position).normalized) > cosRange && (player.position - transform.position).sqrMagnitude < monsterData.monsters[monsterNumber].attackRange * monsterData.monsters[monsterNumber].attackRange)
            {
                IDamagable damagable = player.GetComponent<IDamagable>();
                damagable?.TakeDamage(monsterData.monsters[monsterNumber].damage);
            }
        }
    }


    IEnumerator JumpCoolTime()
    {
        isJump = true;
        rigid.AddForce(Vector2.up * monsterData.monsters[monsterNumber].jumpPower, ForceMode2D.Impulse);
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
        attackCoolTime = monsterData.monsters[monsterNumber].coolTime; // 공격쿨타임
        yield return new WaitForSeconds(monsterData.monsters[monsterNumber].attackTime); // 공격모션시간
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
        Gizmos.DrawWireSphere(transform.position, monsterData.monsters[monsterNumber].findRange);
        Gizmos.DrawWireSphere(transform.position, monsterData.monsters[monsterNumber].attackRange);
    }
}
