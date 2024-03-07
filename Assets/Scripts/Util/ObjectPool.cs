using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] PooledObject prefab;
    [SerializeField] int size;
    [SerializeField] int capacity;

    private Stack<PooledObject> objectPool;

    public void CreatePool(PooledObject prefab, int size, int capacity)
    {
        this.prefab = prefab; //프리팹
        this.size = size;  // 프리팹 생성 수
        this.capacity = capacity; // Stack크기

        objectPool = new Stack<PooledObject>(capacity);
        for (int i = 0; i < size; i++)
        {
            //프리팹 생성
            PooledObject instance = Instantiate(prefab);
            instance.gameObject.SetActive(false);
            //프리팹의 Pool을 지정
            instance.Pool = this;
            //Pool을 부모로
            instance.transform.parent = transform;
            
            objectPool.Push(instance);
        }
    }

    public PooledObject GetPool(Vector3 position, Quaternion rotation)
    {
        //스택에 프리팹이 있다면 꺼내서 사용
        if (objectPool.Count > 0)
        {
            PooledObject instance = objectPool.Pop();
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.gameObject.SetActive(true);
            return instance;
        }
        //없다면 생성해서 사용
        else
        {
            PooledObject instance = Instantiate(prefab);
            instance.Pool = this;
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            return instance;
        }
    }

    public void ReturnPool(PooledObject instance)
    {
        //스택에 공간이 있다면
        if (objectPool.Count < capacity)
        {
            instance.gameObject.SetActive(false);
            instance.transform.parent = transform;
            objectPool.Push(instance);
        }
        //공간이없다면 삭제
        else
        {
            Destroy(instance.gameObject);
        }
    }
}
