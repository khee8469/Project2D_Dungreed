using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

//���ݼӵ�����, �����������ε�

public class Monster : MonoBehaviour, IDamagable
{
    public enum State { Idle, Trace, Jump, Attack, Die }

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int hp;
    [SerializeField] int findRange;
    [SerializeField] int attackRange;
    [SerializeField] int jumpPower;
    [SerializeField] bool isAttacking;
    [SerializeField] bool isGround;

    [SerializeField] float angle;
    private float cosRange;


    [SerializeField] Transform player;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;

    [SerializeField] FindGround groundCheck;

    



    private State state = State.Idle;


    private void Awake()
    {
        cosRange = Mathf.Cos(angle * Mathf.Deg2Rad);   //Ž�� ����  cosRange == 0.5
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


        //���Ϳ� �÷��̾������ �Ÿ�
        if ((player.position - transform.position).sqrMagnitude < findRange * findRange)
        {
            ChangeState(State.Trace);
        }
        if (hp <= 0)
        {
            ChangeState(State.Die);
        }
    }




    private void TraceState()
    {
        Debug.Log("trace");
        Vector3 dir = (player.position - transform.position).normalized;
        if (dir.x > 0 && (player.position - transform.position).sqrMagnitude > attackRange * attackRange)
        {
            if (dir.x > 0 && rigid.velocity.x < speed)
            {
                rigid.velocity = new Vector2(speed, rigid.velocity.y);
            }
            spriteRenderer.flipX = false;
        }
        else if (dir.x < 0 && (player.position - transform.position).sqrMagnitude > attackRange * attackRange)
        {
            if (dir.x < 0 && rigid.velocity.x > -speed)
            {
                rigid.velocity = new Vector2(-speed, rigid.velocity.y);
            }
            spriteRenderer.flipX = true;
        }




        if ((player.position - transform.position).sqrMagnitude > findRange * findRange)
        {
            ChangeState(State.Idle);
        }

        //�÷��̾ �����ְ�, Ž�������ϰ�, ������������
        if (isGround && groundCheck.isJump && player.position.y > transform.position.y + 2 && (player.position - transform.position).sqrMagnitude < findRange * findRange)
        {
            ChangeState(State.Jump);
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
        //�÷��̾ �Ʒ��ְ�, Ž�������ϰ�, ������������
        else if (isGround && groundCheck.isJump && player.position.y < transform.position.y -2 &&  (player.position - transform.position).sqrMagnitude < findRange * findRange)
        {
            ChangeState(State.Jump);
            StartCoroutine(DownJump());
        }

        //�ٴ����϶��� �����ϱ�
        if (Vector2.Dot(transform.up, (player.position - transform.position).normalized) > cosRange && (player.position - transform.position).sqrMagnitude < attackRange * attackRange)
        {
            ChangeState(State.Attack);
            //�ӽ� ������Ÿ�ӵ����ͷ� ��ȯ�ʿ�
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


        

        if ((player.position - transform.position).sqrMagnitude < findRange * findRange)
        {
            ChangeState(State.Trace);
        }

        //�ٴ����϶��� �����ϱ�
        if (Vector2.Dot(transform.up, (player.position - transform.position).normalized) > cosRange && (player.position - transform.position).sqrMagnitude < attackRange * attackRange)
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
            if (Vector2.Dot(transform.right * -1, (player.position - transform.position).normalized) > cosRange && Vector2.Dot(transform.up, (player.position - transform.position).normalized) > cosRange && (player.position - transform.position).sqrMagnitude < attackRange * attackRange)
            {
                //IDamagable �������̽��� ���� ������Ʈ�� ������ ����
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

    //������Ÿ������ ���� �ʿ�
    IEnumerator Attacking()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1.2f);
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
        Gizmos.DrawWireSphere(transform.position, findRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
