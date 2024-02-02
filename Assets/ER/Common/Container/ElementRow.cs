namespace ER
{
    /// <summary>
    /// 元素行
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ElementRow<T>
    {
        private T[] array;//元素组
        private int length;//行长度
        /// <summary>
        /// 索引指针
        /// </summary>
        public int index_hand;

        /// <summary>
        /// 行长度
        /// </summary>
        public int Length => length;

        public ElementRow(int length)
        {
            this.length = length;
            array = new T[length];
            index_hand = -1;
        }

        /// <summary>
        /// 访问指定元素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get => array[index];
            set => array[index] = value;
        }
        /// <summary>
        /// 获取上一个元素(索引指针前移)
        /// </summary>
        /// <returns></returns>
        public T GetPreviousValue()
        {
            index_hand--;
            return array[index_hand];
        }
        /// <summary>
        /// 获取当前索引指针指向的元素
        /// </summary>
        /// <returns></returns>
        public T GetThisValue()
        {
            return array[index_hand];
        }

        /// <summary>
        /// 获取下一个元素(索引指针后移)
        /// </summary>
        /// <returns></returns>
        public T GetNextValue()
        {
            index_hand++;
            return array[index_hand];
        }
        /// <summary>
        /// 尝试获取上一个元素
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetPreviousValue(out T value)
        {
            index_hand--;
            if (IsIndexInRange())
            {
                value = array[index_hand];
                return true;
            }
            value = default(T);
            return false;
        }
        /// <summary>
        /// 尝试获取当前元素
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetThisValue(out T value)
        {
            if (IsIndexInRange())
            {
                value = array[index_hand];
                return true;
            }
            value = default(T);
            return false;
        }
        /// <summary>
        /// 尝试获取下一个元素
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetNextValue(out T value)
        {
            index_hand++;
            if (IsIndexInRange())
            {
                value = array[index_hand];
                return true;
            }
            value = default(T);
            return false;
        }

        /// <summary>
        /// 判断当前索引指针是否在有效范围内
        /// </summary>
        /// <returns></returns>
        public bool IsIndexInRange()
        {
            return index_hand > -1 && index_hand < length;
        }
        /// <summary>
        /// 将索引指针设为0(开头元素)
        /// </summary>
        public void SetIndexFirst()
        {
            index_hand = 0;
        }
        /// <summary>
        /// 将索引指针设为length-1(末尾元素)
        /// </summary>
        public void SetIndexLast()
        {
            index_hand = length - 1;
        }
        /// <summary>
        /// 获取一个空行对象
        /// </summary>
        public static ElementRow<T> Empty
        {
            get
            {
                ElementRow<T> row = new ElementRow<T>(0);
                return row;
            }
        }
        /// <summary>
        /// 判断自身是否为空行
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return length <= 0;
        }
    }
}