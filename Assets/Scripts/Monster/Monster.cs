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
        cosRange = Mathf.Cos(monsterData.monsters[monsterNumber].attackAngle * Mathf.Deg2Rad);   //Ž�� ����  cosRange == 0.5
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
        //���º�ȯ�� �ִϸ��̼� ���
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


        //�߰߰Ÿ����� ������
        if ((player.position - transform.position).sqrMagnitude < monsterData.monsters[monsterNumber].findRange * monsterData.monsters[monsterNumber].findRange)
        {
            ChangeState(State.Trace);
        }
        //���
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

        //���ݹ����������� ������Ÿ���϶�
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

        //�÷��̾ �����ְ�, Ž�������ϰ�, ������������
        if (!isJump && isGround && groundCheck.isJump && player.position.y > transform.position.y + 1 && (player.position - transform.position).sqrMagnitude < monsterData.monsters[monsterNumber].findRange * monsterData.monsters[monsterNumber].findRange)
        {
            ChangeState(State.Jump);
            StartCoroutine(JumpCoolTime());
        }
        //�÷��̾ �Ʒ��ְ�, Ž�������ϰ�, ������������
        else if (isGround && groundCheck.isJump && player.position.y < transform.position.y -1 &&  (player.position - transform.position).sqrMagnitude < monsterData.monsters[monsterNumber].findRange * monsterData.monsters[monsterNumber].findRange)
        {
            ChangeState(State.Jump);
            StartCoroutine(DownJump());
        }

        //�ٴ����϶��� �����ϱ�
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
        //Debug.Log("��������");


        

        if ((player.position - transform.position).sqrMagnitude < monsterData.monsters[monsterNumber].findRange * monsterData.monsters[monsterNumber].findRange)
        {
            ChangeState(State.Trace);
        }

        //�ٴ����϶��� �����ϱ�
        if (attackCoolTime < 0 && Vector2.Dot(transform.up, (player.position - transform.position).normalized) > cosRange && (player.position - transform.position).sqrMagnitude < monsterData.monsters[monsterNumber].attackRange * monsterData.monsters[monsterNumber].attackRange)
        {
            ChangeState(State.Attack);
            //���ݵ�����
            StartCoroutine(Attacking());
        }

        if (hp <= 0)
        {
            ChangeState(State.Die);
        }
    }





    private void AttackState()
    {
        //Debug.Log("������");



        //������ ������
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
        //Debug.Log("����");
    }


    public void TakeDamage(int damage)
    {
        //Debug.Log("������");
        hp -= damage;
    }

    //�ִϸ��̼� �̺�Ʈ�� ����
    private void AttackTimeing()
    {
        //Debug.Log("����");

        if (spriteRenderer.flipX)
        {
            //�����ʱ��� 180���� ���ʱ��� 180���� ��ġ�� 90���� ������
            if (Vector2.Dot(transform.right * -1, (player.position - transform.position).normalized) > cosRange && Vector2.Dot(transform.up, (player.position - transform.position).normalized) > cosRange && (player.position - transform.position).sqrMagnitude < monsterData.monsters[monsterNumber].attackRange * monsterData.monsters[monsterNumber].attackRange)
            {
                //IDamagable �������̽��� ���� ������Ʈ�� ������ ����
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

    //������Ÿ������ ���� �ʿ�
    IEnumerator Attacking()
    {
        isAttacking = true;
        attackCoolTime = monsterData.monsters[monsterNumber].coolTime; // ������Ÿ��
        yield return new WaitForSeconds(monsterData.monsters[monsterNumber].attackTime); // ���ݸ�ǽð�
        //���ݹ����ȿ��� �ڷΰ��� ������ȯ
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


    //�ٴ����� �Ǵ�
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
