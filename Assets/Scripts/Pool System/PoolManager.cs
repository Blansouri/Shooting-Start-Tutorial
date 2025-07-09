using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] EnemyPools;

    [SerializeField] Pool[] playerProjectilePools;

    [SerializeField] Pool[] EnemyProjectilePools;

    [SerializeField] Pool[] VFXPools;

    [SerializeField] Pool[] LootItemPools;

    static Dictionary<GameObject, Pool> dictionary;

    void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();
        Initialize(EnemyPools);
        Initialize(playerProjectilePools);
        Initialize(EnemyProjectilePools);
        Initialize(VFXPools);
        Initialize(LootItemPools);
    }

#if UNITY_EDITOR
    void OnDestroy()//尺寸检查
    {
        CheckPoolSize(EnemyPools);
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(EnemyProjectilePools);
        CheckPoolSize(VFXPools);
        CheckPoolSize(LootItemPools);
    }
#endif
    void CheckPoolSize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
                Debug.LogWarning(string.Format("Pool: {0} has a runtime size {1} bigger than its initial size {2}!", 
                    pool.Prefab.name,
                    pool.RuntimeSize,
                    pool.Size));
        }
    }

    void Initialize(Pool[] pools)//初始化池，在添加子弹时记得在PoolManager中添加对应的对象池
    {
        foreach (var pool in pools)
        {
#if UNITY_EDITOR//条件编译代码
            if (dictionary.ContainsKey(pool.Prefab))//如果字典的键中有相同的预制体，则跳过这次循环，ContainsKey判断字典中是否包含指定的键
            {
                Debug.LogError("Same prefab in multiple pools! Prefab:" + pool.Prefab.name);
                continue;
            }
#endif
            dictionary.Add(pool.Prefab,pool);//每初始化一个对象池，字典中就会添加一个预制体和池的键值对
            Transform poolParent = new GameObject("Pool:" + pool.Prefab.name).transform;
            poolParent.parent = transform;
            pool.Initialize(poolParent);
        }
    }

    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("PoolManager could not find prefab:"+ prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject();
    }

    public static GameObject Release(GameObject prefab, Vector3 position)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("PoolManager could not find prefab:" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position);
    }

    public static GameObject Release(GameObject prefab, Vector3 position,Quaternion rotation)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("PoolManager could not find prefab:" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position,rotation);
    }
    
    public static GameObject Release(GameObject prefab, Vector3 position,Quaternion rotation,Vector3 localScale)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("PoolManager could not find prefab:" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position,rotation,localScale);
    }

}
