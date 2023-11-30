using UnityEngine;

namespace ER
{
    /// <summary>
    /// 对象池内容物
    /// </summary>
    public abstract class Water : MonoBehaviour
    {
        /// <summary>
        /// 所属池子对象
        /// </summary>
        protected ObjectPool pool;

        /// <summary>
        /// 所属池子对象
        /// </summary>
        public ObjectPool Pool { get; }

        /// <summary>
        /// 重置状态
        /// </summary>
        public abstract void ResetState();

        /// <summary>
        /// 自身销毁（返回对象池）
        /// </summary>
        public void Destroy()
        {
            if (pool != null)
            {
                pool.ReturnObject(this);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
            OnHide();
        }

        /// <summary>
        /// 自身返回对象池时触发的函数
        /// </summary>
        protected abstract void OnHide();
    }
}