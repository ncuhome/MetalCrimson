using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 带有区域判定的对象，简称为 实体(Entity)；
    /// Entity 类表示的是一个对象整体，可以保证各 特征组件 有顺序的初始化
    /// 通常来说 一个2D对象可能由很多 GameObject 和 Component 组成，需要有一个 代表对象 来表示这个游戏对象；
    /// 也就是说 Entity 必须是一个 通用的 描述广泛的 对象代表类；
    /// 需要保证可以 以 Entity 为媒介 便捷地传递消息；
    /// </summary>
    public class Entity : MonoBehaviour
    {
        /// <summary>
        /// 条件委托；
        /// 在指定 特征添加进后 触发的委托
        /// </summary>
        private struct ConditionalDelegation
        {
            /// <summary>
            /// 条件特征名称
            /// </summary>
            public string name;

            /// <summary>
            /// 触发行为
            /// </summary>
            public Action<IAttribute> action;

            public ConditionalDelegation(string name, Action<IAttribute> action)
            {
                this.name = name;
                this.action = action;
            }
        }

        #region 属性

        [Tooltip("预加载特征列表")]
        public List<MonoAttribute> Attributes;

        /// <summary>
        /// 实体的特征对象(使用特征名称作为索引)
        /// </summary>
        private Dictionary<string, IAttribute> attributes = new();

        /// <summary>
        /// 条件委托列表
        /// </summary>

        private List<ConditionalDelegation> delegations = new();

        #endregion 属性

        #region 条件委托管理

        /// <summary>
        /// 创建一个条件委托
        /// </summary>
        public void CreateDelegation(string attributeName, Action<IAttribute> action)
        {
            delegations.Add(new ConditionalDelegation(attributeName, action));
        }

        /// <summary>
        /// 尝试触发条件委托
        /// </summary>
        private void JudgeDelegation(IAttribute attribute)
        {
            foreach (var del in delegations)
            {
                if (del.name == attribute.Name) del.action(attribute);
            }
        }

        #endregion 条件委托管理

        #region 特征管理

        /// <summary>
        /// 向该实体添加新的特征对象，并初始化特征；
        /// 如果新填特征的名称为null，添加失败并返回 true；
        /// 如果发生名称冲突，返回 true；
        /// </summary>
        /// <param name="attribute">特征对象</param>
        /// <param name="cover">发生名称冲突时是否强制覆盖</param>
        /// <returns>添加时是否存在冲突，</returns>
        public bool Add(IAttribute attribute, bool cover = false)
        {
            if (attribute.Name == null) return false;

            if (attributes.ContainsKey(attribute.Name))
            {
                if (cover) attributes[attribute.Name] = attribute;
                attribute.Owner = this;
                attribute.Initialize();
                JudgeDelegation(attribute);
                return true;
            }

            attributes[attribute.Name] = attribute;
            attribute.Owner = this;
            attribute.Initialize();
            JudgeDelegation(attribute);
            return false;
        }

        /// <summary>
        /// 移除指定特征
        /// </summary>
        /// <param name="attributeName">特征名称</param>
        /// <returns>是否移除成功</returns>
        public bool Remove(string attributeName)
        {
            if (Exist(attributeName))
            {
                attributes.Remove(attributeName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断该实体是否拥有指定特征
        /// </summary>
        /// <param name="attributeName">特征名称</param>
        /// <returns></returns>
        public bool Exist(string attributeName)
        {
            return attributes.ContainsKey(attributeName);
        }

        /// <summary>
        /// 访问该实体指定特征对象；
        /// 如果特征不存在则返回null；
        /// </summary>
        /// <param name="attributeName">特征名称</param>
        /// <returns></returns>
        public IAttribute this[string attributeName]
        {
            get
            {
                if (Exist(attributeName)) return attributes[attributeName];
                return null;
            }
        }

        /// <summary>
        /// 获取指定特征，返回找到的第一个特征对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetAttribute<T>() where T : IAttribute
        {
            foreach (IAttribute attribute in attributes.Values)
            {
                if (attribute is T) return (T)attribute;
            }
            return default(T);
        }

        /// <summary>
        /// 尝试获取特征并赋值, 如果获取特征失败, 则将回调函数加入委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public bool TryGetAttribute<T>(string attributeName, ref T aim, Action<IAttribute> callBack) where T : IAttribute
        {
            IAttribute t = this[attributeName];
            if (t != null)
            {
                aim = (T)t;
                return true;
            }
            CreateDelegation(attributeName, callBack);
            return false;
        }

        #endregion 特征管理

        #region Unity

        private void Awake()
        {
            if (Attributes != null && Attributes.Count > 0)
            {
                foreach (var attribute in Attributes)
                {
                    if (Add(attribute, false))
                    {
                        Debug.LogWarning($"预加载特征出现命名冲突:{attribute.Name}");
                    }
                }
            }
            Attributes = null;
            Initialized();
        }

        protected virtual void Initialized()
        {
        }

        #endregion Unity
    }
}