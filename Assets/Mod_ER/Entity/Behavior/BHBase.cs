using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace ER.Entity
{
    /// <summary>
    /// 状态机状态
    /// </summary>
    public enum State 
    { 
        /// <summary>
        /// 未启动
        /// </summary>
        wait,
        /// <summary>
        /// 开始阶段（前段）
        /// </summary>
        start,
        /// <summary>
        /// 运行阶段（中段）
        /// </summary>
        running,
        /// <summary>
        /// 结束阶段（后段）
        /// </summary>
        end
    }

    /// <summary>
    /// 行为状态信息结构体
    /// </summary>
    public struct StateBH
    {
        /// <summary>
        /// 行为名称
        /// </summary>
        public string name;
        /// <summary>
        /// 可被一下行为取消
        /// </summary>
        public string[] breakers;
        /// <summary>
        /// 当前行为的状态
        /// </summary>
        public State status;
    }
    /// <summary>
    /// 行为事件
    /// </summary>
    public delegate void DelBehaviour();

    /// <summary>
    /// 行为模块抽象基类
    /// </summary>
    public abstract class BHBase : StaticAttribute
    {
        #region 字段|属性
        protected new ExciteEntity owner;
        /// <summary>
        /// 启用此行为时触发的事件
        /// </summary>
        public event DelBehaviour StartEvent;
        /// <summary>
        /// 正常结束此行为时触发的事件
        /// </summary>
        public event DelBehaviour EndEvent;
        /// <summary>
        /// 行为发生中断时触发的事件
        /// </summary>
        public event DelBehaviour BreakEvent;
        protected bool started = false;//是否已开始此行为
        protected int animationIndex;
        #endregion

        #region 属性
        /// <summary>
        /// 动画状态索引
        /// </summary>
        public virtual int AnimationIndex { get => animationIndex; protected set => animationIndex = value; }
        public override Entity Owner { get => owner; set => owner = value as ExciteEntity; }
        /// <summary>
        /// 是否已开始此行为
        /// </summary>
        public bool Started { get => started; }

        /// <summary>
        /// 获取当前行为信息
        /// </summary>
        public StateBH Info
        {
            get
            {
                return new StateBH
                {
                    name = attributeName 
                };
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 配置 属性名称，动画机索引
        /// </summary>
        /// <param name="_owner"></param>
        public BHBase(ExciteEntity _owner):base(_owner)
        {
            owner = _owner;
            animationIndex = 0;
            attributeName = "基态行为";
        }
        #endregion

        #region 与实体关联
        public void Start()
        {
            if (StartEvent != null) { StartEvent(); }
        }
        public void End()
        {
            if (EndEvent != null) { EndEvent(); }
        }
        public void Break()
        {
            if (BreakEvent != null) { BreakEvent(); }
            BreakBehaviour();
        }
        #endregion

        #region 功能函数
        public override object GetStatus()
        {
            return Info;
        }

        /// <summary>
        /// 中断此行为
        /// </summary>
        protected abstract void BreakBehaviour();
        /// <summary>
        /// 运行此行为
        /// </summary>
        /// <param name="index">行为状态索引</param>
        public abstract void Behaviour(int index);
        #endregion
    }
}
