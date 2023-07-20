using UnityEngine;

namespace Common
{
    /// <summary>
    /// 单例模式基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class, new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
    }

    /// <summary>
    /// 组件单例模式基类，过场不销毁
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : class, new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                return instance;
            }
        }
        protected virtual void Awake()
        {
            if(instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
                return;
            }
            Destroy(gameObject);
        }
    }
}