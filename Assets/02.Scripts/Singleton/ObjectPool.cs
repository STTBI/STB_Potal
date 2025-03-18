using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool : MonoBehaviour
{
    /*
    오브젝트에 void OnDisable()
    { 
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke(); // Invoke함수 사용시
    } 함수 만들기
    */

    static ObjectPool instance;
    void Awake() => instance = this;

    [Serializable]
    public class Pool
    {
        public string tag; // Same name as prefab
        public GameObject prefab; // object
        public int size; // pool size
    }

    [SerializeField] Pool[] pools;
    List<GameObject> spawnObjects; //Put All GameObjects

    Dictionary<string, Queue<GameObject>> poolDictionary; // tag에 맞는 GameObject 생성

    void Start()
    {
        spawnObjects = new List<GameObject>();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // Create ObjectPool as size
        foreach (Pool pool in pools)
        {
            poolDictionary.Add(pool.tag, new Queue<GameObject>());
            for (int i = 0; i < pool.size; i++)
            {
                var obj = CreateNewObject(pool.tag, pool.prefab);
                ArrangePool(obj);
            }

            // OnDisable에 ReturnToPool 구현여부와 중복구현 검사
            if (poolDictionary[pool.tag].Count <= 0)
                Debug.LogError($"{pool.tag}");
            else if (poolDictionary[pool.tag].Count != pool.size)
                Debug.LogError($"{pool.tag} ReturnToPool is overlap");
        }
    }

    //instance._SpawnFromPool overroading Method
    public static GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) =>
        instance._SpawnFromPool(tag, position, rotation);

    public static T SpawnFromPool<T>(string tag, Vector3 position, Quaternion rotation) where T : Component
    {
        GameObject obj = instance._SpawnFromPool(tag, position, rotation);
        //check null & getcomponent
        if (obj.TryGetComponent(out T component))
            return component;
        else
        {
            obj.SetActive(false);
            throw new Exception($"Component not found");
        }
    }

    public static void ReturnToPool(GameObject obj)
    {
        //check by name
        if (!instance.poolDictionary.ContainsKey(obj.name))
            throw new Exception($"Pool with tag {obj.name} doesn't exist.");

        //Enqueue Pool Object
        instance.poolDictionary[obj.name].Enqueue(obj);
    }

    GameObject _SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag)) //Check tag Exception 
            throw new Exception($"Pool with tag {tag} doesn't exist.");

        Queue<GameObject> poolQueue = poolDictionary[tag];
        // if(Queue == null ) Add obj to pool
        if (poolQueue.Count <= 0)
        {
            //Find Same Tag
            Pool pool = Array.Find(pools, x => x.tag == tag);
            //CreateObject prefab As tag
            var obj = CreateNewObject(pool.tag, pool.prefab);
            ArrangePool(obj);
        }

        //pull out from queue
        GameObject objectToSpawn = poolQueue.Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    //Create Object to Pool
    GameObject CreateNewObject(string tag, GameObject prefab)
    {
        //Create Prefab
        var obj = Instantiate(prefab, transform);

        //Set name 
        obj.name = tag;
        obj.SetActive(false); // false == OnDisable() ReturnToPool -> Enqueue
                              // Create Immediately ReturnToPool
        return obj;
    }

    void ArrangePool(GameObject obj)
    {
        // 추가된 오브젝트 묶어서 정렬
        bool isFind = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == transform.childCount - 1)
            {
                obj.transform.SetSiblingIndex(i);
                spawnObjects.Insert(i, obj);
                break;
            }
            else if (transform.GetChild(i).name == obj.name)
                isFind = true;
            else if (isFind)
            {
                obj.transform.SetSiblingIndex(i);
                spawnObjects.Insert(i, obj);
                break;
            }
        }
    }
}

