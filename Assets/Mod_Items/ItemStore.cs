// Ignore Spelling: Json

using System;
using System.Collections.Generic;

namespace ER.Items
{
    /// <summary>
    /// 物品系统（动态）(背包)（可读写）
    /// </summary>
    public class ItemStore
    {
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string storeName;

        /// <summary>
        /// 仓库物品
        /// </summary>
        private Dictionary<int, ItemVariable> items = new();

        
    }
}