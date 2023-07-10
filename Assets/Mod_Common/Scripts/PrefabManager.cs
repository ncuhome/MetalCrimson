using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    #region 单例封装
    private PrefabManager instance;
    public PrefabManager Instance { get{ return instance; } }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    #region 预制体
    public List<GameObject> prefabs = new List<GameObject>();
    #endregion

    /// <summary>
    /// 通过名称获取指定预制体
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetPrefab(string name)
    {
        foreach(GameObject pb in prefabs)
        {
            if(pb.name == name)
            {
                return pb;
            }
        }
        return null;
    }
    /// <summary>
    /// 添加新的物体作为预制体（一般不使用）
    /// </summary>
    /// <param name="obj"></param>
    public void AddPrefab(GameObject obj)
    {
        prefabs.Add(obj);
    }
    /// <summary>
    /// 移除指定预制体
    /// </summary>
    /// <param name="obj"></param>
    public void RemovePrefab(GameObject obj)
    {
        prefabs.Remove(obj);
    }
}
