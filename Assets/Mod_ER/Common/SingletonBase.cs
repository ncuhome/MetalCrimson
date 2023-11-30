using UnityEngine;

namespace ER
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
                if (instance == null)
                {
                    Debug.LogError($"单例对象不存在:{typeof(T)}");
                }
                return instance;
            }
        }

        /// <summary>
        /// 替换单例对象为自身，如果已存在则销毁自身
        /// </summary>
        protected void PasteInstance()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
                return;
            }
            Destroy(gameObject);
        }

        protected virtual void Awake()
        {
            PasteInstance();
        }
    }
}