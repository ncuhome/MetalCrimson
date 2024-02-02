using System;

namespace ER.Entity2D
{
    /// <summary>
    /// 修正委托
    /// </summary>
    public struct CorrectValueDelegate
    {
        /// <summary>
        /// 优先级
        /// </summary>
        public int level;

        /// <summary>
        /// 标签
        /// </summary>
        public string tag;

        /// <summary>
        /// 数值修正委托
        /// </summary>
        public Func<float, float> correct;

        public CorrectValueDelegate(Func<float, float> correct, int level = 0, string tag = "None")
        { this.correct = correct; this.level = level; this.tag = tag; }

        /// <summary>
        /// 优先级比较器
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool Comparer(CorrectValueDelegate d1, CorrectValueDelegate d2)
        {
            return d1.level > d2.level;
        }

        public float Invoke(float num)
        { return correct(num); }

        public void Invoke(ref float num)
        { num = correct(num); }
    }

    /// <summary>
    /// 带有修正委托的值
    /// </summary>
    public class CorrectValue
    {
        public CorrectValue(float defaultValue = 0)
        {
            corrects = new PriorityQueue<CorrectValueDelegate>(CorrectValueDelegate.Comparer);
            this.defaultValue = defaultValue;
            value = defaultValue;
        }

        #region 属性

        private PriorityQueue<CorrectValueDelegate> corrects;
        private float value;//当前值
        private float defaultValue;//默认值

        /// <summary>
        /// 默认值
        /// </summary>
        public float DefaultValue
        {
            get => defaultValue;
            set
            {
                defaultValue = value;
                DefValueChangedEvent?.Invoke(defaultValue);
                UpdateValue();
            }
        }

        /// <summary>
        /// 当前值
        /// </summary>
        public float Value => value;

        /// <summary>
        /// 更新当前值时触发的事件
        /// </summary>
        public event Action<float> ValueChangedEvent;

        /// <summary>
        /// 默认值发生改变时触发的事件
        /// </summary>
        public event Action<float> DefValueChangedEvent;

        #endregion 属性

        #region 方法

        /// <summary>
        /// 添加新的修正委托
        /// </summary>
        /// <param name="delegate"></param>
        public void Add(CorrectValueDelegate @delegate)
        {
            corrects.Add(@delegate);
            UpdateValue();
        }

        /// <summary>
        /// 移除指定的修正委托
        /// </summary>
        /// <param name="delegate"></param>
        public void Remove(CorrectValueDelegate @delegate)
        {
            corrects.Remove(@delegate);
            UpdateValue();
        }
        /// <summary>
        /// 移除指定的修正委托
        /// </summary>
        /// <param name="tag"></param>
        public void Remove(string tag)
        {
            for (int i = 0; i < corrects.Count; i++)
            {
                if (corrects[i].tag == tag)
                {
                    corrects.RemoveAt(i);
                    UpdateValue();
                    return;
                }
            }
        }

        private float UpdateValue()
        {
            value = defaultValue;
            for (int i = 0; i < corrects.Count; i++)
            {
                corrects[i].Invoke(ref value);
            }
            ValueChangedEvent?.Invoke(value);
            return value;
        }

        /// <summary>
        /// 判断是否包含指定修正委托
        /// </summary>
        /// <param name="tag">修正标签</param>
        /// <returns></returns>
        public bool Contains(string tag)
        {
            for(int i=0;i<corrects.Count;i++)
            {
                if (corrects[i].tag == tag)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion 方法
    }
}