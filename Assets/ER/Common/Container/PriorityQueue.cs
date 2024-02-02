using System;
using System.Collections.Generic;

namespace ER
{
    /// <summary>
    /// 优先队列
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    public class PriorityQueue<T>
    {
        #region 属性

        private List<T> values;//元素表
        private Func<T, T, bool> Comparer;//元素比较器

        #endregion 属性

        /// <summary>
        ///
        /// </summary>
        /// <param name="_Compare">排序方法, 返回 true 则将参数目标1放在索引更小的位置</param>
        public PriorityQueue(Func<T, T, bool> _Compare)
        {
            values = new();
            Comparer = _Compare;
        }

        public void Add(params T[] items)
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (Comparer(item, values[i]))
                {
                    values.Insert(i, item);
                    return;
                }
            }
            values.Add(item);
        }

        /// <summary>
        /// 移除指定元素
        /// </summary>
        /// <param name="item"></param>
        public void Remove(T item)
        {
            values.Remove(item);
        }

        public void RemoveAt(int index)
        {
            values.RemoveAt(index);
        }

        /// <summary>
        /// 顶部元素
        /// </summary>
        public T Top
        {
            get
            {
                if (values.Count > 0) return values[0];
                return default(T);
            }
        }

        /// <summary>
        /// 获取顶部元素, 并从表中移除它
        /// </summary>
        /// <returns></returns>
        public T GetTop()
        {
            if (values.Count > 0)
            {
                T top = values[0];
                values.RemoveAt(0);
                return top;
            }
            return default(T);
        }

        /// <summary>
        /// 容器中的元素数量
        /// </summary>
        public int Count { get => values.Count; }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= values.Count) return default(T);
                return values[index];
            }
        }

    }
}