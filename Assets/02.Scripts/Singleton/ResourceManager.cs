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

        // �÷��̾� ��������
        LoadResource<GameObject>("Prefabs\\Player", "player");
        //LoadAllResources<GameObject>("Prefabs\\enemy", enemy); enemy�����ȿ� ����ִ� ��������� enemy1, enemy2, enemy3 �ҷ��ͼ� Ű���� ������. 
    }

    /// <summary>
    /// Object�� ��ӹ��� ���ҽ����� ��ũ��Ʈ���� ������ �� �ֽ��ϴ�.
    /// </summary>
    /// <typeparam name="T">ObjectType</typeparam>
    /// <param name="key">Resource name</param>
    /// <returns></returns>
    public T GetResource<T>(string key) where T : Object
    {
        if (objResources.TryGetValue(key, out var obj))
        {
            if (obj is T t) // Ÿ�Ժ����� �����ϸ� ��ȯ
                return t as T;
        }

        // Ÿ�� ������ �� ���ٴ� �����޽��� ���
        Debug.LogError($"[ResourceManager] Resource with key '{key}' is not of type {typeof(T)}.");
        return null;
    }

    /// <summary>
    /// Object�� ��ӹ��� ���ҽ����� �ҷ��� �� �ֽ��ϴ�.
    /// </summary>
    /// <typeparam name="T">ObjectType</typeparam>
    /// <param name="path">���</param>
    /// <param name="key"></param>
    private void LoadResource<T>(string path, string key) where T : Object
    {
        if(!objResources.ContainsKey(key))
        {
            T resource = Resources.Load<T>(path);
            if(resource != null)
            {
                objResources[key] = resource;
            }
            else
            {
                Debug.LogError($"[ResourceManager] Failed to load resource at path: {path}");
            }
        }
    }

    private void LoadAllResources<T>(string path, string key) where T : Object
    {
        T[] resources = Resources.LoadAll<T>(path);
        int numbering = 0;
        foreach (T resource in resources)
        {
            if (resource != null)
            {
                // ���̸� ������ ���ʴ�� �ҷ��ͼ� �ֽ��ϴ�.
                objResources[$"{key}{numbering++}"] = resource;
            }
            else
            {
                Debug.LogError($"[ResourceManager] Failed to load resource at path: {path}");
            }
        }
    }
}
