using UnityEngine;

public class Monster : MonoBehaviour
{
    public enum State { Idle, Trace, Attack, Die }
    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int hp;
    [SerializeField] int range;

    [SerializeField] Transform player;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;


    private State state = State.Idle;


    public void HitDamage(int damage)
    {
        Debug.Log("데미지");
        hp -= damage;
    }

    private void Die()
    {
        if (hp <= 0)
        {
            Debug.Log("사망");
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        Die();
        switch (state)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Trace:
                TraceState();
                break;
            case State.Attack:

                break;
            case State.Die:

                break;
        }
    }

    public void ChangeState(State state)
    {
        this.state = state;
    }

    private void IdleState()
    {
        //animator.SetBool("Trace", false);

        //몬스터와 플레이어까지의 거리
        if ((player.position - transform.position).sqrMagnitude < range * range)
        {
            ChangeState(State.Trace);
        }

    }

    private void TraceState()
    {
        //animator.SetBool("Trace", true);

        Vector3 dir = (player.position - transform.position).normalized;
        if (dir.x > 0)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            spriteRenderer.flipX = false;
        }
        else if (dir.x < 0)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            spriteRenderer.flipX = true;
        }


        if ((player.position - transform.position).sqrMagnitude > range * range)
        {
            ChangeState(State.Idle);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
