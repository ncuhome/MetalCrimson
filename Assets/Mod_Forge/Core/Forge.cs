using ER;
using ER.ItemStorage;

namespace Mod_Forge
{
    public class Forge:Singleton<Forge>
    {
        
        private ItemContainer materials;
        private ItemContainer components;
        /// <summary>
        /// 材料库
        /// </summary>
        public ItemContainer Materials { get => materials; }
        /// <summary>
        /// 组件库
        /// </summary>
        public ItemContainer Components{ get=> components; }

    }
}