﻿using System.Collections.Generic;
using UnityEngine;

namespace Mod_Common
{
    /// <summary>
    /// 对象池事件
    /// </summary>
    /// <param name="obj">参与事件的对象</param>
    public delegate void DelObjectPool(GameObject obj);
    /// <summary>
    /// 对象池
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        #region 事件
        /// <summary>
        /// 从此池中获取对象时触发的事件
        /// </summary>
        public event DelObjectPool GetObjectEvent;
        /// <summary>
        /// 有对象返回到此池时触发的对象
        /// </summary>
        public event DelObjectPool ReturnObjectEvent;
        #endregion

        #region 公开属性
        /// <summary>
        /// 对象物体
        /// </summary>
        public GameObject Prefab;
        /// <summary>
        /// 对象池内剩余对象的数量
        /// </summary>
        public int ObjectCount { get => pool.Count; }
        /// <summary>
        /// 池的默认大小
        /// </summary>
        public int PoolSize = 20;
        /// <summary>
        /// 对象池名称
        /// </summary>
        public string PoolName = "默认池";
        #endregion

        #region 内部属性
        /// <summary>
        /// 对象池
        /// </summary>
        private Queue<GameObject> pool;
        #endregion

        #region 功能函数
        /// <summary>
        /// 从对象池中获取一个新的对象
        /// </summary>
        /// <returns></returns>
        public GameObject GetObject()
        {
            if(pool.Count > 0)
            {
                GameObject obj = pool.Dequeue();
                GetObjectEvent.Invoke(obj);
                obj.transform.SetParent(null);
                return obj;
            }
            else
            {
                Debug.LogWarning("对象池为空，无法获取新对象！");
                return null;
            }
        }

        /// <summary>
        /// 将对象返回对象池
        /// </summary>
        /// <param name="obj"></param>
        public void ReturnObject(GameObject obj)
        {
            ReturnObjectEvent.Invoke(obj);
            pool.Enqueue(obj);
            obj.transform.SetParent(transform);
        }
        /// <summary>
        /// 将池内的对象数量恢复至指定数量
        /// </summary>
        /// <param name="count"></param>
        public void SetSize(int count)
        {
            for (int i = 0; i < count; i++)
            {
                ReturnObject(Instantiate(Prefab, transform));
            }
        }
        #endregion

        public ObjectPool()
        {
            pool = new Queue<GameObject>();
            SetSize(PoolSize);
            gameObject.SetActive(false);
        }
    }
}
