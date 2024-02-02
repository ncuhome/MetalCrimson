using System.Collections.Generic;
using UnityEngine;

namespace ER
{
    /// <summary>
    /// 对象池事件
    /// </summary>
    /// <param name="obj">参与事件的对象</param>
    public delegate void DelObjectPool(Water obj);

    /// <summary>
    /// 对象池
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        #region 公开属性
        /// <summary>
        /// 当存量不足时, 自动扩充的容量, 为0或者负数表示不自动扩充
        /// </summary>
        public int AutoExpandSize = 5;

        /// <summary>
        /// 对象物体
        /// </summary>
        public GameObject Prefab;

        /// <summary>
        /// 对象池内剩余对象的数量
        /// </summary>
        public int ObjectCount { get => pool.Count; }

        [Tooltip("池的默认大小")]
        [SerializeField]
        private int poolSize = 20;
        /// <summary>
        /// 池的大小
        /// </summary>
        public int PoolSize
        {
            get { return poolSize; }
            set { SetSize(value); }
        }

        /// <summary>
        /// 对象池名称
        /// </summary>
        public string PoolName = "默认池";

        #endregion 公开属性

        #region 内部属性

        /// <summary>
        /// 对象池
        /// </summary>
        private Queue<Water> pool = new Queue<Water>();

        #endregion 内部属性

        #region 功能函数

        /// <summary>
        /// 从对象池中获取一个新的对象
        /// </summary>
        /// <param name="reset">是否自动重置为默认状态</param>
        /// <returns></returns>
        public Water GetObject(bool reset = false)
        {
            if (Prefab == null) return null;
            if (pool.Count > 0)
            {
                Water obj = pool.Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                if(reset)
                {
                    obj.ResetState();
                }
                return obj;
            }
            else
            {
                if(AutoExpandSize > 0)
                {
                    PoolSize += AutoExpandSize;
                    for(int i=0;i<AutoExpandSize;i++)
                    {
                        Water water = Instantiate(Prefab, transform).GetComponent<Water>();
                        water.Pool = this;
                        ReturnObject(water);
                    }
                    return GetObject(reset);
                }
                Debug.LogWarning("对象池为空，无法获取新对象！");
                return null;
            }
        }

        /// <summary>
        /// 将对象返回对象池
        /// </summary>
        /// <param name="obj"></param>
        public void ReturnObject(Water obj)
        {
            if (Prefab == null) return;
            //Debug.Log("获取返回对象!");
            pool.Enqueue(obj);
            obj.transform.SetParent(transform);
            obj.gameObject.SetActive(false);
        }

        /// <summary>
        /// 将池内的对象数量恢复至指定数量
        /// </summary>
        /// <param name="count"></param>
        public void SetSize(int count)
        {
            if (Prefab == null) return;
            poolSize = count;
            while(pool.Count < count)
            {
                Water water = Instantiate(Prefab, transform).GetComponent<Water>();
                water.Pool = this;
                ReturnObject(water);
            }
        }

        #endregion 功能函数

        public void Init(string poolName,GameObject prefab,int count = 20)
        {
            PoolName = poolName;
            Prefab = prefab;
            poolSize = count;
            Init();
        }
        /// <summary>
        /// 初始化对象池
        /// </summary>
        public void Init()
        {
            if (Prefab == null) return;
            ObjectPoolManager.Instance.RegisterPool(this);
            if (Prefab == null || Prefab.GetComponent<Water>() == null)
            {
                Debug.LogError($"对象池输入预制体出错:{PoolName}");
            }
            else
            {
                SetSize(poolSize);
            }
        }

        private void Awake()
        {
            Init();
        }

        private void OnDestroy()
        {
            ObjectPoolManager.Instance.UnregisterPool(PoolName);
        }
    }
}