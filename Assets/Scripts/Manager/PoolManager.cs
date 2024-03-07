using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<int, ObjectPool> poolDic = new Dictionary<int, ObjectPool>();


    //Pool , ������ ����
    public void CreatePool(PooledObject prefab, int size, int capacity)
    {
        //������Ʈ ���� �� �̸� ����
        GameObject gameObject = new GameObject();
        gameObject.name = $"Pool_{prefab.name}";
        //Pool ����
        ObjectPool objectPool = gameObject.AddComponent<ObjectPool>();
        //Pool�� ������ ����
        objectPool.CreatePool(prefab, size, capacity);
        //Dictionary�� ����
        poolDic.Add(prefab.GetInstanceID(), objectPool);
    }

    //Pool ����
    public void DestroyPool(PooledObject prefab)
    {
        ObjectPool objectPool = poolDic[prefab.GetInstanceID()];
        Destroy(objectPool.gameObject);

        poolDic.Remove(prefab.GetInstanceID());
    }

    //��ųʸ� Ŭ����
    public void ClearPool()
    {
        foreach (ObjectPool objectPool in poolDic.Values)
        {
            Destroy(objectPool.gameObject);
        }

        poolDic.Clear();
    }

    //Pool���� ������ ���� ���
    public PooledObject GetPool(PooledObject prefab, Vector3 position, Quaternion rotation)
    {
        //��ųʸ��� ����� �ּҷ� ObjectPool�� ����
        return poolDic[prefab.GetInstanceID()].GetPool(position, rotation);
    }
}
