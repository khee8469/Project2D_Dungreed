using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<int, ObjectPool> poolDic = new Dictionary<int, ObjectPool>();


    //Pool , 프리팹 생성
    public void CreatePool(PooledObject prefab, int size, int capacity)
    {
        //오브젝트 생성 및 이름 지정
        GameObject gameObject = new GameObject();
        gameObject.name = $"Pool_{prefab.name}";
        //Pool 생성
        ObjectPool objectPool = gameObject.AddComponent<ObjectPool>();
        //Pool에 프리팹 생성
        objectPool.CreatePool(prefab, size, capacity);
        //Dictionary에 저장
        poolDic.Add(prefab.GetInstanceID(), objectPool);
    }

    //Pool 삭제
    public void DestroyPool(PooledObject prefab)
    {
        ObjectPool objectPool = poolDic[prefab.GetInstanceID()];
        Destroy(objectPool.gameObject);

        poolDic.Remove(prefab.GetInstanceID());
    }

    //딕셔너리 클리어
    public void ClearPool()
    {
        foreach (ObjectPool objectPool in poolDic.Values)
        {
            Destroy(objectPool.gameObject);
        }

        poolDic.Clear();
    }

    //Pool에서 프리팹 꺼내 사용
    public PooledObject GetPool(PooledObject prefab, Vector3 position, Quaternion rotation)
    {
        //딕셔너리에 저장된 주소로 ObjectPool에 접근
        return poolDic[prefab.GetInstanceID()].GetPool(position, rotation);
    }
}
