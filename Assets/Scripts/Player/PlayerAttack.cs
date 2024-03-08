using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour, IDamagable
{
    [SerializeField] bool debug;

    [SerializeField] LayerMask targetLayer;
    [SerializeField] float range;
    [SerializeField] int attacAngle;
    [SerializeField] int damage;
    [SerializeField] int hp;
    [SerializeField] float effectRange;
    [SerializeField] Transform cursor;
    [SerializeField] Transform leftRotate;
    [SerializeField] PooledObject attactEffectPrefab;

    

    float cosAngle;
    [SerializeField] bool attack;//공격모션 변환용

    Vector2 attactEffectPos;
    float angle;

    public void Awake()
    {
        cosAngle = Mathf.Cos(attacAngle * Mathf.Deg2Rad);
        Manager.Pool.CreatePool(attactEffectPrefab, 2, 4);
    }

    //공격
    private void OnLeftMouse(InputValue value)
    {
        Attack();
        AttactEffect();
    }

    private void AttactEffect()
    {
        if (!attack)
        {
            leftRotate.localScale = new Vector3(1, -1, 1);
            attack = true;
        }
        else if (attack)
        {
            leftRotate.localScale = new Vector3(1, 1, 1);
            attack = false;
        }
        // 이펙트 방향, 거리
        Vector3 dir = (cursor.position - transform.position).normalized;

        // 검 이펙트 풀링 사용
        PooledObject pooledObject = Manager.Pool.GetPool(attactEffectPrefab, transform.position, transform.rotation);

        // 이펙트 마우스방향으로 회전
        angle = Mathf.Atan2(cursor.position.y - transform.position.y, cursor.position.x - transform.position.x) * Mathf.Rad2Deg;
        pooledObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);//forward(z축 기준)으로 회전

    }

    Collider2D[] colliders = new Collider2D[10];
    private void Attack()
    {
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, range, colliders, targetLayer);
        for (int i = 0; i < size; i++)
        {
            Vector2 dir = (colliders[i].transform.position - transform.position).normalized;

            IDamagable monster = colliders[i].GetComponent<IDamagable>();
            if (Vector2.Dot(dir, cursor.position) > cosAngle)
            {
                Debug.Log("데미지를 주었다.");
                monster.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("적한테 맞음");
        hp -= damage;

        if(hp <= 0)
        {
            Debug.Log("죽음");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (debug == false)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
