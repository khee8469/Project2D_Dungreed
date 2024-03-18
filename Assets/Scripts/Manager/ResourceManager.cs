using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>  // 기본 소스데이터들 로드
{
    public Dictionary<int, ItemData> itemDic = new Dictionary<int, ItemData>();
    public Dictionary<int, WeaponData> weaponDic = new Dictionary<int, WeaponData>();
    public Dictionary<int, MonsterData> monsterDic = new Dictionary<int, MonsterData>();


    /*public T Load<T>(string path) where T : Object
    {
        string key = $"{path}_{typeof(T)}";

        if (resources.TryGetValue(key, out Object obj))
        {
            return obj as T;
        }
        else
        {
            T resource = Resources.Load<T>(path);
            resources.Add(key, resource);
            return resource;
        }
    }*/

    private void Awake()
    {
        ItemLoad();
    }


    public void ItemLoad() // 데이터 로드
    {
        ItemData[] items = Resources.LoadAll<ItemData>("Data/ItemData");
        //Debug.Log(items.Length);
        foreach (ItemData data in items)
        {
            itemDic.Add(data.itemInfo.itemNumber, data);
        }
        
        /*WeaponData[] weapons = Resources.LoadAll<WeaponData>("Data/WeaponData");
        foreach (WeaponData weapon in weapons)
        {
            weaponDic.Add(weapon.weaponInfo.weaponNumber, weapon);
        }*/

        MonsterData[] monsters = Resources.LoadAll<MonsterData>("Data/MonsterData");
        //Debug.Log(monsters.Length);
        foreach (MonsterData monster in monsters)
        {
            monsterDic.Add(monster.monsterInfo.monsterNumber, monster);
        }
    }


}

/*public class Monster
{
    public void Die()
    {
        Manager.Resource.itemDic["롱소드"];
    }
}*/