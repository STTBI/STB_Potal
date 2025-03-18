using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{

    // key: string, value: Prefab
    private Dictionary<string, Object> objResources;

    private void OnValidate()
    {
        objResources = new Dictionary<string, Object>();

        // 플레이어 가져오기
        LoadResource<GameObject>("Prefabs", "Player");
        //LoadAllResources<GameObject>("Prefabs\\enemy", enemy); enemy폴더안에 들어있는 프리펩들을 enemy1, enemy2, enemy3 불러와서 키값을 가진다. 
    }

    /// <summary>
    /// Object를 상속받은 리소스들을 스크립트에서 가져올 수 있습니다.
    /// </summary>
    /// <typeparam name="T">ObjectType</typeparam>
    /// <param name="key">Resource name</param>
    /// <returns></returns>
    public T GetResource<T>(string key) where T : Object
    {
        if (objResources.TryGetValue(key, out var obj))
        {
            if (obj is T t) // 타입변경이 가능하면 반환
                return t as T;
        }

        // 타입 변경할 수 없다는 에러메시지 출력
        Debug.LogError($"[ResourceManager] Resource with key '{key}' is not of type {typeof(T)}.");
        return null;
    }

    /// <summary>
    /// Object를 상속받은 리소스들을 불러올 수 있습니다.
    /// </summary>
    /// <typeparam name="T">ObjectType</typeparam>
    /// <param name="path">경로</param>
    /// <param name="key"></param>
    public void LoadResource<T>(string path, string key) where T : Object
    {
        if(!objResources.ContainsKey(key))
        {
            T resource = Resources.Load<T>($"{path}\\{key}");
            if(resource != null)
            {
                objResources[key] = resource;
            }
            else
            {
                Debug.LogError($"[ResourceManager] Failed to load resource at path: {path}\\{key}");
            }
        }
    }

    public void LoadAllResources<T>(string path, string key) where T : Object
    {
        T[] resources = Resources.LoadAll<T>(path);
        int numbering = 1;
        foreach (T resource in resources)
        {
            if (resource != null)
            {
                // ※이름 순으로 차례대로 불러와서 넣습니다.
                objResources[$"{key}{numbering++}"] = resource;
            }
            else
            {
                Debug.LogError($"[ResourceManager] Failed to load resource at path: {path}");
            }
        }
    }
}
