using System;
using System.Collections.Generic;

namespace ER.Entity2D
{
    /// <summary>
    /// 效果设置信息
    /// </summary>
    public struct BuffSetInfo
    {
        /// <summary>
        /// 效果名称
        /// </summary>
        public string buffName;

        /// <summary>
        /// 预期总共持续时间（小于0表示无限持续时间）
        /// </summary>
        public float defTime;

        /// <summary>
        /// 效果等级
        /// </summary>
        public int level;

        /// <summary>
        /// 其他初始化信息(拓展接口)
        /// </summary>
        public Dictionary<string, object> infos;
    }

    /// <summary>
    /// 效果
    /// </summary>
    [Serializable]
    public abstract class MDBuff
    {
        #region 属性

        /// <summary>
        /// 效果宿主
        /// </summary>
        public ATBuffManager owner;

        /// <summary>
        /// 效果名称
        /// </summary>
        public string buffName;

        /// <summary>
        /// 效果标签
        /// </summary>
        public List<string> buffTag = new();

        /// <summary>
        /// 名称文本
        /// </summary>
        public string nameText;

        /// <summary>
        /// 描述文本
        /// </summary>
        public string descriptionText;

        /// <summary>
        /// 预期总共持续时间（小于0表示无限持续时间）
        /// </summary>
        public float defTime;

        /// <summary>
        /// 当前剩余持续时间
        /// </summary>
        public float time;

        /// <summary>
        /// 效果等级
        /// </summary>
        public int level;

        /// <summary>
        /// 最大效果等级
        /// </summary>
        public int levelMax;

        /// <summary>
        /// 该效果对象对应的设置信息
        /// </summary>
        public BuffSetInfo SetInfo
        {
            get => new BuffSetInfo()
            {
                buffName = buffName,
                defTime = defTime,
                level = level,
                infos = null
            };
        }

        /// <summary>
        /// 重复叠加策略
        /// </summary>
        public enum RepeatType
        {
            /// <summary>
            /// 无回应
            /// </summary>
            None,

            /// <summary>
            /// 增加等级
            /// </summary>
            MoreLevel,

            /// <summary>
            /// 叠加时间
            /// </summary>
            MoreTime,

            /// <summary>
            /// 刷新时间
            /// </summary>
            RepeatTime,

            /// <summary>
            /// 无回应但是触发加载效果
            /// </summary>
            NoneAndEnter,

            /// <summary>
            /// 增加等级并且触发加载效果
            /// </summary>
            MoreLevelAndEnter,

            /// <summary>
            /// 叠加时间并且触发加载效果
            /// </summary>
            MoreTimeAndEnter,

            /// <summary>
            /// 刷新时间并且触发加载效果
            /// </summary>
            RepeatTimeAndEnter,

            /// <summary>
            /// 自定义应对策略
            /// </summary>
            Custom
        }

        /// <summary>
        /// 重复叠加策略
        /// </summary>
        public RepeatType repeatType;

        /// <summary>
        /// 优先级
        /// </summary>
        public int priority;

        #endregion 属性

        #region 事件

        /// <summary>
        /// 加入此效果触发的事件
        /// </summary>
        public event Action<ATBuffManager> EnterEvent;

        /// <summary>
        /// 持有此效果触发的事件
        /// </summary>
        public event Action<ATBuffManager> StayEvent;

        /// <summary>
        /// 移除此效果触发的事件
        /// </summary>
        public event Action<ATBuffManager> ExitEvent;

        #endregion 事件

        public MDBuff()
        {
        }

        public MDBuff(BuffSetInfo setInfo)
        {
            defTime = setInfo.defTime;
            level = setInfo.level;
        }

        #region 效果

        /// <summary>
        /// 重置时间
        /// </summary>
        public void ResetTime()
        {
            time = defTime;
        }

        /// <summary>
        /// 自定义叠加策略
        /// </summary>
        public virtual void Repeat()
        {
        }

        public void Enter()
        {
            if (EnterEvent != null) EnterEvent(owner);
            EffectOnEnter();
        }

        public void Stay()
        {
            if (StayEvent != null) StayEvent(owner);
            EffectOnStay();
        }

        public void Exit()
        {
            if (ExitEvent != null) ExitEvent(owner);
            EffectOnExit();
        }

        /// <summary>
        /// 当加入此效果时触发的函数
        /// </summary>
        /// <param name="owner"></param>
        public abstract void EffectOnEnter();

        /// <summary>
        /// 当持有此效果时触发的函数（每帧调用）
        /// </summary>
        /// <param name="owner"></param>
        public abstract void EffectOnStay();

        /// <summary>
        /// 当移除此效果时触发的函数
        /// </summary>
        /// <param name="owner"></param>
        public abstract void EffectOnExit();

        #endregion 效果
    }
}