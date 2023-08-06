// Ignore Spelling: Creat

using Mod_Save;
using System.Collections.Generic;

namespace ER.Items
{
    /// <summary>
    /// 物品动态仓库管理器
    /// </summary>
    public class ItemStoreManager
    {
        #region 单例封装

        private static ItemStoreManager instance;

        public static ItemStoreManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemStoreManager();
                }
                return instance;
            }
        }

        private ItemStoreManager()
        { }

        #endregion 单例封装

        #region 属性

        private Dictionary<string, ItemStore> stores = new Dictionary<string, ItemStore>();

        /// <summary>
        /// 动态仓库列表
        /// </summary>
        public Dictionary<string, ItemStore> Stores { get => stores; }

        #endregion 属性

        #region 方法

        /// <summary>
        /// 创建动态仓库
        /// </summary>
        /// <param name="storeName">仓库名称</param>
        /// <param name="size">仓库大小</param>
        public void Creat(string storeName, int size = 64)
        {
            ItemStore store = new ItemStore(storeName, size);
            stores[storeName] = store;
        }

        /// <summary>
        /// 删除指定动态仓库
        /// </summary>
        /// <param name="storeName">仓库名称</param>
        public void Delete(string storeName)
        {
            if (stores.ContainsKey(storeName))
            {
                stores.Remove(storeName);
            }
        }

        /// <summary>
        /// 判断指定仓库是否存在
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns></returns>
        public bool Exist(string storeName)
        {
            if (stores.ContainsKey(storeName))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取指定动态仓库
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns></returns>
        public ItemStore this[string storeName]
        {
            get => stores[storeName];
        }

        #endregion 方法
    }
}