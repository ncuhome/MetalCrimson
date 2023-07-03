using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Items
{
    /// <summary>
    /// 物品系统（动态）
    /// </summary>
    [Serializable]
    public class ItemStore
    {
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string storeName;
        /// <summary>
        /// 仓库物品
        /// </summary>
        public List<string> stores = new List<string>();

    }
}
