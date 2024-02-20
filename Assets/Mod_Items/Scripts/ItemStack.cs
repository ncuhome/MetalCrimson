namespace Mod_Item
{
    /// <summary>
    /// 物品堆
    /// </summary>
    public class ItemStack:IRAnchor
    {
        public string registryName;
        private string uuid;
        /// <summary>
        /// 堆叠数量
        /// </summary>
        private int amount;
        /// <summary>
        /// 堆叠上限
        /// </summary>
        private int amountMax;

        private SpriteResource sprite;

        private string displayName;

        private string description;

        public string UUID => uuid;
    }
}