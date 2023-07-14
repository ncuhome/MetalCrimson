// Ignore Spelling: Json

using System.Collections.Generic;

namespace ER.Items
{
    /// <summary>
    /// 物品系统（动态）(背包)（可读写）
    /// </summary>
    public class ItemStore
    {
        private int size = 64;

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string storeName;

        /// <summary>
        /// 仓库大小
        /// </summary>
        public int Size
        {
            get => size;
            set
            {
                size = value;
                if (size < 0) size = 0;
                while (items.Count > size)
                {
                    items.RemoveAt(items.Count - 1);
                }
            }
        }

        /// <summary>
        /// 仓库物品
        /// </summary>
        private List<ItemVariable> items = new();

        /// <summary>
        /// 向仓库添加物品，如果仓库已满，则返回false
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddItem(ItemVariable item)
        {
            if (items.Count < size)
            {
                items.Add(item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据索引从此仓库移除物品，如果物品不存在则返回false
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool RemoveItem(int index)
        {
            if (index >= 0 || index < items.Count)
            {
                items.RemoveAt(index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 清空仓库
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>
        /// 根据物品索引获取指定物品
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ItemVariable this[int index]
        {
            get
            {
                return items[index];
            }
        }
    }
}