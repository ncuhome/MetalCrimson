using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 对 特征 接口进一步封装，以适用于 将组件对象封装为一个 实体特征；
    /// 注意在构造函数中重新赋值特征名称；
    /// </summary>
    public abstract class MonoAttribute : MonoBehaviour, IAttribute
    {
        [SerializeField]
        [Tooltip("特征名称")]
        protected string AttributeName;

        /// <summary>
        /// 特征所属的实体对象
        /// </summary>
        protected Entity owner;

        public string Name { get => AttributeName; set => AttributeName = value; }
        public Entity Owner { get => owner; set => owner = value; }

        public abstract void Initialize();

        public virtual void Destroy()
        {
            Destroy(gameObject);
        }
    }
}