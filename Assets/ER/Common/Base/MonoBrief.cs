using System;
using UnityEngine;

namespace ER
{
    /// <summary>
    /// 短暂类, 用于初始化数据, 执行完初始化函数后, 自动销毁该组件
    /// </summary>
    public abstract class MonoBrief:MonoBehaviour
    {
        private void Awake()
        {
            Init(() => { this.enabled = false; });
        }
        /// <summary>
        /// 初始化函数, 在awake时会调用一次, 如果完成初始化请调用 callback 回调函数
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public abstract void Init(Action callback);

        private void OnDisable()
        {
            Destroy(this);
        }
    }
}