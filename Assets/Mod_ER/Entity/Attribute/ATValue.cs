using ER.UI;
using UnityEngine;

namespace ER.Entity
{
    /// <summary>
    /// 数值响应事件
    /// </summary>
    /// <param name="info">数值事件信息</param>
    public delegate void ValueActionEvent(ValueEventInfo info);
    /// <summary>
    /// 数值判定事件
    /// </summary>
    /// <param name="info">数值事件信息</param>
    /// <returns>判定结果</returns>
    public delegate bool ValueJudgeEvent(ValueEventInfo info);

    /// <summary>
    /// 数值属性结构体
    /// </summary>
    public struct Values
    {
        public float value;
        public float max;
    }
    /// <summary>
    /// 数值组件 发生事件时的事件信息
    /// </summary>
    public struct ValueEventInfo
    {
        /// <summary>
        /// 当前数值
        /// </summary>
        public float value;
        /// <summary>
        /// 当前数值最大值
        /// </summary>
        public float max;
        /// <summary>
        /// 本次事件中 数值的变化值
        /// </summary>
        public float deltaValue;
        /// <summary>
        /// 本次事件中 数值上限的变化值
        /// </summary>
        public float deltaMax;
        /// <summary>
        /// 触发此事件 的对象
        /// </summary>
        public object pruner;
    }

    /// <summary>
    /// 数值属性
    /// </summary>
    public class ATValue : DynamicAttribute
    {
        #region 字段
        [SerializeField]
        /// <summary>
        /// 数值
        /// </summary>
        protected float value = 100;
        [SerializeField]
        /// <summary>
        /// 数值上限
        /// </summary>
        protected float max = 100;
        [SerializeField]
        /// <summary>
        /// 自然增量
        /// </summary>
        protected float increase = 0;
        [SerializeField]
        /// <summary>
        /// 自然增量间隔（秒）
        /// </summary>
        protected float increaseCD = 5;
        [SerializeField]
        /// <summary>
        /// 数值是否可为负数
        /// </summary>
        protected bool negative = false;
        [SerializeField]
        /// <summary>
        /// 数值是否可溢出上限
        /// </summary>
        protected bool overflow = false;
        [SerializeField]
        /// <summary>
        /// 是否开启自然增量
        /// </summary>
        protected bool SelfIncrease = false;
        /// <summary>
        /// 可视化值UI条
        /// </summary>
        protected ValueBar bar;
        /// <summary>
        /// 治疗时钟
        /// </summary>
        private float timer = 0;
        #endregion

        #region 公开属性
        /// <summary>
        /// 当前数值
        /// </summary>
        public float Value { get => value; }
        /// <summary>
        /// 当前数值上限
        /// </summary>
        public float Max { get => max; }
        /// <summary>
        /// 数值是否可小于零
        /// </summary>
        public bool Negative { get => negative; set => negative = value; }
        /// <summary>
        /// 数值是否可超过上限
        /// </summary>
        public bool Overflow { get => overflow; set => overflow = value; }

        #endregion

        #region 事件
        /// <summary>
        /// 在触发 数值改变 事件，值改变前触发的事件（事件信息，当前操作是否生效）
        /// </summary>
        public event ValueJudgeEvent HealthChangeBefEvent;
        /// <summary>
        /// 在触发 数值上限改变 事件，值改变前触发的事件（事件信息，当前操作是否生效）
        /// </summary>
        public event ValueJudgeEvent HealthMaxChangeBefEvent;
        /// <summary>
        /// 数值变化后触发的事件(事件信息)
        /// </summary>
        public event ValueActionEvent HealthChangeEvent;
        /// <summary>
        /// 生命上限变化后触发的事件(事件信息)
        /// </summary>
        public event ValueActionEvent HealthMaxChangeEvent;
        /// <summary>
        /// 数值归零后触发的事件
        /// </summary>
        public event ValueActionEvent DeadEvent;
        #endregion

        #region 功能函数
        /// <summary>
        /// 设置当前数值
        /// </summary>
        /// <param name="value">修改后的数值</param>
        /// <param name="pruner">修改者对象（触发事件的对象）</param>
        /// <returns></returns>
        public bool SetValue(float value, object pruner)
        {
            bool next = true;
            float change = value - this.value;
            if (HealthChangeBefEvent != null)
            {
                next = HealthChangeBefEvent(new ValueEventInfo
                {
                    value = this.value,
                    max = max,
                    deltaValue = change,
                    pruner= pruner
                }) ;
            }
            if(next)
            {
                this.value = value;
                if (bar != null) { bar.Value = this.value / max; }
                #region 界线判定
                if (!negative)
                {
                    if (this.value < 0) { this.value = 0; }
                }
                if(!overflow)
                {
                    if (this.value > max) { this.value = max; }
                }
                #endregion

                if (HealthChangeEvent != null)
                {
                    HealthChangeEvent(new ValueEventInfo
                    {
                        value = this.value,
                        max = max,
                        deltaValue = change,
                        pruner = pruner
                    });
                }

                if(this.value < 0 && DeadEvent !=null)
                {
                    DeadEvent(new ValueEventInfo
                    {
                        value = this.value,
                        max = max,
                        deltaValue = change,
                        pruner = pruner
                    });
                }
            }
            return next;
        }
        /// <summary>
        /// 设置当前数值上限
        /// </summary>
        /// <param name="value">修改后的数值上限</param>
        /// <param name="pruner">修改者对象（触发事件的对象）</param>
        /// <returns></returns>
        public bool SetMax(float value, object pruner)
        {
            bool next = true;
            float change = value - max;
            if (HealthMaxChangeBefEvent != null)
            {
                next = HealthMaxChangeBefEvent(new ValueEventInfo
                {
                    value = this.value,
                    max = max,
                    deltaMax = change,
                    pruner = pruner
                });
            }
            if (next)
            {
                max = value;
                if (bar != null) { bar.Value = this.value / max; }

                #region 界线判定
                if (!negative)
                {
                    if (this.value < 0) { this.value = 0; }
                }
                if (!overflow)
                {
                    if (this.value > max) { this.value = max; }
                }
                #endregion

                if (HealthMaxChangeEvent != null)
                {
                    HealthMaxChangeEvent(new ValueEventInfo
                    {
                        value = this.value,
                        max = max,
                        deltaMax = change,
                        pruner = pruner
                    });
                }

            }
            return next;
        }
        /// <summary>
        /// 修改当前数值
        /// </summary>
        /// <param name="value">变化值</param>
        /// <param name="pruner">修改者对象</param>
        /// <returns></returns>
        public bool ModifyValue(float value, object pruner)
        {
            bool next = true;
            Debug.Log("生命发生改变！");
            if (HealthChangeBefEvent != null)
            {
                next = HealthChangeBefEvent(new ValueEventInfo
                {
                    value = this.value,
                    max = max,
                    deltaValue = value,
                    pruner = pruner
                });
            }
            if (next)
            {
                this.value += value;
                if (bar != null) { bar.Value = this.value / max; }

                #region 界线判定
                if (!negative)
                {
                    if (this.value < 0) { this.value = 0; }
                }
                if (!overflow)
                {
                    if (this.value > max) { this.value = max; }
                }
                #endregion

                if (HealthChangeEvent != null)
                {
                    HealthChangeEvent(new ValueEventInfo
                    {
                        value = this.value,
                        max = max,
                        deltaValue = value,
                        pruner = pruner
                    });
                }
                if (this.value < 0 && DeadEvent != null)
                {
                    DeadEvent(new ValueEventInfo
                    {
                        value = this.value,
                        max = max,
                        deltaValue = value,
                        pruner = pruner
                    });
                }
            }
            return next;
        }
        /// <summary>
        /// 修改当前数值上限
        /// </summary>
        /// <param name="value">变化值</param>
        /// <param name="pruner">修改者对象</param>
        /// <returns></returns>
        public bool ModifyMax(float value, object pruner)
        {
            bool next = true;
            if (HealthMaxChangeBefEvent != null)
            {
                next = HealthMaxChangeBefEvent(new ValueEventInfo
                {
                    value = this.value,
                    max = max,
                    deltaMax = value,
                    pruner = pruner
                });
            }
            if (next)
            {
                max += value;
                if (bar != null) { bar.Value = this.value / max; }

                #region 界线判定
                if (!negative)
                {
                    if (this.value < 0) { this.value = 0; }
                }
                if (!overflow)
                {
                    if (this.value > max) { this.value = max; }
                }
                #endregion

                if (HealthMaxChangeEvent != null)
                {
                    HealthMaxChangeEvent(new ValueEventInfo
                    {
                        value = this.value,
                        max = max,
                        deltaMax = value,
                        pruner = pruner
                    });
                }
            }
            return next;
        }
        #endregion

        #region 内部函数
        private void Start()
        {
            attributeName = "Health";
        }

        public override void Initialization()
        {
        }

        private void Update()
        {
            if(SelfIncrease)
            {
                if(timer >=0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    ModifyValue(increase,owner);
                    timer = increaseCD;
                }
            }
        }
        #endregion
    }
}
