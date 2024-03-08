using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour, IDamagable
{
    public enum State { Idle, Trace, Jump, Attack, Die }

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int hp;
    [SerializeField] int range;
    [SerializeField] int attackRange;
    [SerializeField] int jumpPower;
    [SerializeField] bool isAttacking;
    [SerializeField] float angle;

    private float cosRange;


    [SerializeField] Transform player;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;

    [SerializeField] LayerMask layerMask;


    private State state = State.Idle;


    private void Awake()
    {
        cosRange = Mathf.Cos(angle * Mathf.Deg2Rad);   //탐색 범위  cosRange == 0.5
    }

    private void Update()
    {

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


        //몬스터와 플레이어까지의 거리
        if ((player.position - transform.position).sqrMagnitude < range * range)
        {
            ChangeState(State.Trace);
            //animator.Play("Trace");
        }
        if (hp <= 0)
        {
            ChangeState(State.Die);
            //animator.Play("Die");
        }
    }

    private void TraceState()
    {


        //Debug.Log("trace");


        Vector3 dir = (player.position - transform.position).normalized;
        if (dir.x > 0 && (player.position.x - transform.position.x) < attackRange)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            spriteRenderer.flipX = false;
        }
        else if (dir.x < 0 && (player.position.x - transform.position.x) < attackRange)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            spriteRenderer.flipX = true;
        }



        if ((player.position - transform.position).sqrMagnitude > range * range)
        {
            ChangeState(State.Idle);
            //animator.Play("Idle");
        }

        //바닥위일때만 공격하기
        if (Vector2.Dot(transform.up, (player.position - transform.position).normalized) > cosRange && (player.position - transform.position).sqrMagnitude < attackRange * attackRange)
        {
            ChangeState(State.Attack);
            //animator.Play("Attack");
            //공격딜레이
            StartCoroutine(Attacking());
        }

        if (hp <= 0)
        {
            ChangeState(State.Die);
            //animator.Play("Die");
        }
    }

    private void JumpState()
    {

    }


    private void AttackState()
    {
        //Debug.Log("attack");



        if (!isAttacking)
        {
            ChangeState(State.Trace);
            //animator.Play("Trace");
        }


        if (hp <= 0)
        {
            ChangeState(State.Die);
            //animator.Play("Die");
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
            if (Vector2.Dot(transform.right * -1, (player.position - transform.position).normalized) > cosRange && Vector2.Dot(transform.up, (player.position - transform.position).normalized) > cosRange && (player.position - transform.position).sqrMagnitude < attackRange * attackRange)
            {
                //IDamagable 인터페이스를 가진 오브젝트면 데미지 적용
                IDamagable damagable = player.GetComponent<IDamagable>();
                damagable?.TakeDamage(damage);
            }
        }
        else if (!spriteRenderer.flipX)
        {
            if (Vector2.Dot(transform.right, (player.position - transform.position).normalized) > cosRange && Vector2.Dot(transform.up, (player.position - transform.position).normalized) > cosRange && (player.position - transform.position).sqrMagnitude < attackRange * attackRange)
            {
                IDamagable damagable = player.GetComponent<IDamagable>();
                damagable?.TakeDamage(damage);
            }
        }
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
        yield return new WaitForSeconds(1.2f);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
