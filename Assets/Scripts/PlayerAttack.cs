using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] bool debug;

    [SerializeField] LayerMask targetLayer;
    [SerializeField] float range;
    [SerializeField] int angle;
    [SerializeField] int damage;
    [SerializeField] float effectRange;
    [SerializeField] Transform cursor;
    [SerializeField] Transform leftRotate;
    [SerializeField] PooledObject attactEffectPrefab;

    

    float cosAngle;
    [SerializeField] bool attack;

    public void Awake()
    {
        cosAngle = Mathf.Cos(angle * Mathf.Deg2Rad);
        Manager.Pool.CreatePool(attactEffectPrefab, 4, 8);
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
        float a = Vector2.Angle(transform.right, cursor.position);
        Debug.Log(a);

        // 검 이펙트 풀링 사용
        Manager.Pool.GetPool(attactEffectPrefab, transform.position + dir * 2, Quaternion.AxisAngle(Vector3.forward, a)); 
    }

    Collider2D[] colliders = new Collider2D[10];
    private void Attack()
    {
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, range, colliders, targetLayer);
        for (int i = 0; i < size; i++)
        {
            Vector2 dir = (colliders[i].transform.position - transform.position).normalized;
            Debug.Log(dir);
            Monster monster = colliders[i].GetComponent<Monster>();
            if (Vector2.Dot(dir, cursor.position) > cosAngle)
            {
                monster.HitDamage(damage);
            }
        }
    }

    //풀링전 임시
    IEnumerator EffectDestroy(GameObject effect)
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(effect);
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
