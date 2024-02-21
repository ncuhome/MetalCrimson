using System.Collections.Generic;

namespace Mod_Item
{
    /// <summary>
    /// 物品箱子
    /// </summary>
    public class ItemChest : IRAnchor
    {
        private string uuid;
        public string UUID => uuid;

        private List<ItemStack> stacks;
    }
}