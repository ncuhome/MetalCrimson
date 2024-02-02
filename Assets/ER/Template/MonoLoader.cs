using System.Collections.Generic;
using UnityEngine;

namespace ER.Template
{
    /// <summary>
    /// 初始化加载器, 非显性单例, 不要重复创建实例, 完成初始化加载后自动销毁物体
    /// </summary>
    public class MonoLoader:MonoBehaviour
    {
        private static MonoLoader instance;
        [Tooltip("初始化组件顺序 - Awake 顺序 - 必须实现于 MonoInit")]
        public List<MonoBehaviour> components = new List<MonoBehaviour>();
        private void Awake()
        {
            instance = this;
            NextInit();
        }
        /// <summary>
        /// 初始化下一个对象(作为回调函数调用)
        /// 初始化组件时, 会使 目标组件 awake , 接下来目标组件 需要在完成初始化后 调用本函数完成逻辑回调
        /// </summary>
        public void NextInit()
        {
            if (components.Count > 0)
            {
                MonoBehaviour cp = components[0];
                components.RemoveAt(0);
                MonoInit mi = cp as MonoInit;
                if(mi != null)
                {
                    mi.Init();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
        /// <summary>
        /// 初始化回调函数
        /// </summary>
        public static void InitCallback()
        {
            instance?.NextInit();
        }
    }
    /// <summary>
    /// 初始化接口, 需要配合 MonoLoader 使用, 实现该接口需要满足以下要求:
    /// #1 必须是 MonoBehaviour 的派生类 # 
    /// #2 必须在 Awake 或者 Start 两者至少一个中 调用 Init()
    /// #3 完成初始化后必须 对 MonoLoader.NextInit() 进行回调
    /// <code> MonoLoader.InitCallback(); </code>
    /// </summary>
    public interface MonoInit
    {
        public void Init();
    }
}