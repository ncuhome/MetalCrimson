using System.Collections.Generic;
using UnityEngine;
namespace ER.Entity
{
    /// <summary>
    /// 伤害体触发事件委托
    /// </summary>
    /// <param name="">伤害事件信息</param>
    public delegate void DelDamage(DamageEventInfo info);
    /// <summary>
    /// 击退事件委托
    /// </summary>
    /// <param name="info">击退事件信息</param>
    public delegate void DelRepel(RepelEventInfo info);
    /// <summary>
    /// 加载完毕属性时触发事件
    /// </summary>
    public delegate void DelLoadAttributes();

    /// <summary>
    /// 实体管理类：
    /// 用于对实体所拥有的属性脚本进行管理，该实体下的属性脚本可通过此对象间接 获取 其他属性脚本；
    /// 在Start时，该脚本会自动将 所挂载的游戏物体 身上的所有 IAttribute 对象收纳管理列表当中
    /// </summary>
    public class Entity : MonoBehaviour
    {
        #region 组件
        /// <summary>
        /// 自身刚体组件
        /// </summary>
        public Rigidbody2D SelfRigidbody;
        #endregion

        #region 字段

        protected string damageTag = "Test";
        protected List<IAttribute> attributes= new List<IAttribute>();
        /// <summary>
        /// 此实体受到伤害判定时调用的事件
        /// </summary>
        public event DelDamage GetDamangeEvent;
        /// <summary>
        /// 此实体衍生的伤害体造成伤害判定时调用的事件
        /// </summary>
        public event DelDamage CauseDamageEvent;
        /// <summary>
        /// 此实体受到击退调用的事件
        /// </summary>
        public event DelRepel GetRepelEvent;
        /// <summary>
        /// 此实体对其他实体造成击退时调用的事件
        /// </summary>
        public event DelRepel CauseRepelEvent;

        /// <summary>
        /// 加载完毕属性时触发事件
        /// </summary>
        public event DelLoadAttributes LoadAttributesEvent;
        /// <summary>
        /// 是否接受控制
        /// </summary>
        public bool controlabel = true;
        /// <summary>
        /// 击退箱
        /// </summary>
        public ATRepelBox repelBox;
        #endregion

        #region 属性
        /// <summary>
        /// 伤害标签
        /// </summary>
        public string DamageTag
        {
            get => damageTag; set => damageTag = value;
        }
        #endregion

        #region 公开函数
        /// <summary>
        /// 添加新的属性对象
        /// </summary>
        public virtual void AddAttribute(IAttribute attribute)
        {
            if (attribute == null) return;
            Debug.Log("添加属性成功");
            attributes.Add(attribute);
            attribute.Owner = this;
        }
        /// <summary>
        /// 获取单个属性组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns></returns>
        public virtual T GetAttribute<T>() where T :class,IAttribute
        {
            foreach(IAttribute attribute in attributes)
            {
                if (attribute is T) { return attribute as T; }
            }
            return null;
        }
        /// <summary>
        /// 获取属性组件组
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns></returns>
        public virtual T[] GetAttributes<T>() where T : class, IAttribute
        {
            List<T> list = new List<T>();
            foreach (IAttribute attribute in attributes)
            {
                if (attribute is T) { list.Add(attribute as T); }
            }
            return list.ToArray();
        }
        /// <summary>
        /// 根据名称获取指定属性组件
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <returns></returns>
        public virtual IAttribute GetAttribute(string name)
        {
            foreach(IAttribute attribute in attributes)
            {
                if (attribute.Name == name) { return attribute; }
            }
            return null;
        }    
        /// <summary>
        /// 从这个实体中移除指定属性（同时销毁组件）
        /// </summary>
        /// <param name="attribute"></param>
        public virtual void RemoveAttribute(IAttribute attribute)
        {
            if(attributes.Contains(attribute))
            {
                attributes.Remove(attribute);
                attribute.Destroy();
            }
        }
        /// <summary>
        /// 受到伤害体判定时自动调用
        /// </summary>
        /// <param name="eventInfo">伤害事件信息</param>
        public virtual void GetDamage(DamageEventInfo eventInfo)
        {
            if (GetDamangeEvent != null) { GetDamangeEvent(eventInfo); }
        }
        /// <summary>
        /// 由此实体衍生的伤害体造成伤害判定时自动调用
        /// </summary>
        /// <param name="eventInfo">伤害事件信息</param>
        public virtual void CauseDamage(DamageEventInfo eventInfo)
        {
            if (CauseDamageEvent != null) { CauseDamageEvent(eventInfo); }
        }
        /// <summary>
        /// 受到击退时自动调用
        /// </summary>
        /// <param name="eventInfo"></param>
        public virtual void GetRepel(RepelEventInfo eventInfo)
        {
            if (GetRepelEvent != null) { GetRepelEvent(eventInfo); }
        }
        /// <summary>
        /// 此实体衍生的击退体造成击退时自动调用
        /// </summary>
        /// <param name="eventInfo"></param>
        public virtual void CauseRepel(RepelEventInfo eventInfo)
        {
            if (CauseRepelEvent != null) { CauseRepelEvent(eventInfo); }
        }
        #endregion

        #region Unity
        protected virtual void Awake()
        {
            repelBox = new ATRepelBox(this);
            AddAttribute(repelBox);
        }
        protected virtual void Start()
        {
            IAttribute[] ats = GetComponents<IAttribute>();

            foreach (IAttribute attribute in ats)
            {
                AddAttribute(attribute);
            }
            if (LoadAttributesEvent != null) { LoadAttributesEvent(); }
        }
        void Update()
        {

        }

        #endregion
    }
}