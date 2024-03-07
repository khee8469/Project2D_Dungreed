using System.Collections;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    [SerializeField] bool autoRelease;
    [SerializeField] float releaseTime;

    private ObjectPool pool;
    public ObjectPool Pool { get { return pool; } set { pool = value; } }

    private void OnEnable()
    {
        if (autoRelease)
        {
            StartCoroutine(ReleaseRoutine());
        }
    }

    IEnumerator ReleaseRoutine()
    {
        yield return new WaitForSeconds(releaseTime);
        Release();
    }

    //�������� �����ɶ����� ����
    public void Release()
    {
        //Pool�� �ִٸ�
        if (pool != null)
        {
            pool.ReturnPool(this);
        }
        //Pool�� ���ٸ� ����
        else
        {
            Destroy(gameObject);
        }
    }
}