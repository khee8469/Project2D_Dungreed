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

    //프리팹이 생성될때마다 실행
    public void Release()
    {
        //Pool이 있다면
        if (pool != null)
        {
            pool.ReturnPool(this);
        }
        //Pool이 없다면 삭제
        else
        {
            Destroy(gameObject);
        }
    }
}
