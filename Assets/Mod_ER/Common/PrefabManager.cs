
using System.Collections.Generic;
using UnityEngine;

namespace ER
{
    public class PrefabManager : MonoSingleton<PrefabManager>
    {
        #region 预制体
        [SerializeField]
        public List<GameObject> prefabs = new List<GameObject>();

        #endregion 预制体

        /// <summary>
        /// 通过名称获取指定预制体
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject this[string name]
        {
            get
            {
                foreach (GameObject pb in prefabs)
                {
                    if (pb.name == name)
                    {
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
            for(int i =0;i<prefabs.Count;i++)
            {
                if (prefabs[i].name == name)
                {
                    prefabs.RemoveAt(i--);
                }
            }
        }
    }
}