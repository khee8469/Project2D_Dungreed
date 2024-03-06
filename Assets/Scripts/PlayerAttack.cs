using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] bool debug;

    [SerializeField] LayerMask targetLayer;
    [SerializeField] int range;
    [SerializeField] int angle;
    [SerializeField] int damage;
    [SerializeField] Transform cursor;
    float cosAngle;

    public void Awake()
    {
        cosAngle = Mathf.Cos(angle * Mathf.Deg2Rad);
    }

    //АјАн
    private void OnLeftMouse(InputValue value)
    {
        Attack();
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
