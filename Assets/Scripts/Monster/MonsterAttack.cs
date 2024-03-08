using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float range;
    [SerializeField] float angle;
    float cosAngle;

    public void Awake()
    {
        cosAngle = Mathf.Cos(angle * Mathf.Deg2Rad);
    }

    Collider2D[] colliders = new Collider2D[10];
    private void FindTarget()
    {
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, range, colliders, targetLayer);
        for (int i = 0; i < size; i++)
        {
            Vector3 dir = (colliders[i].transform.position - transform.position).normalized;
            if (Vector3.Dot(dir, transform.position) < cosAngle)
            {

            }
        }
    }
}
