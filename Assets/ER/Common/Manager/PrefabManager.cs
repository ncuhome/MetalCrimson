using System.Collections.Generic;
using UnityEngine;
using ER.Template;

namespace ER
{
    /// <summary>
    /// 预制体管理器
    /// </summary>
    public class PrefabManager : MonoSingleton<PrefabManager>, MonoInit
    {
        #region 预制体

        [SerializeField]
        public List<GameObject> prefabs = new List<GameObject>();

        #endregion 预制体

        /// <summary>
        /// 通过名称获取指定预制体
        /// </summary>
        /// <param name="name"></param>
        /// <param name="copy">是否获取它的拷贝</param>
        /// <returns></returns>
        public GameObject this[string name,bool copy = true]
        {
            get
            {
                foreach (GameObject pb in prefabs)
                {
                    if (pb.name == name)
                    {
                        if(copy)
                        {
                            return Instantiate(pb);
                        }
                        return pb;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 添加新的物体作为预制体（一般不使用）
        /// </summary>
        /// <param name="obj"></param>
        public void AddPrefab(GameObject obj)
        {
            prefabs.Add(obj);
        }

        public void Init()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            if (!enabled)
                enabled = true;
            MonoLoader.InitCallback();
        }

        /// <summary>
        /// 移除指定预制体
        /// </summary>
        /// <param name="obj"></param>
        public void RemovePrefab(GameObject obj)
        {
            prefabs.Remove(obj);
        }

        /// <summary>
        /// 移除指定预制体
        /// </summary>
        /// <param name="obj"></param>
        public void RemovePrefab(string name)
        {
            for (int i = 0; i < prefabs.Count; i++)
            {
                if (prefabs[i].name == name)
                {
                    prefabs.RemoveAt(i--);
                }
            }
        }
    }
}