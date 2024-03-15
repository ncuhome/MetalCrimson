using ER.Resource;
using UnityEngine;

namespace Mod_Resource
{
    //物品:材料
    public class RMaterial : IResource
    {
        private string registryName;
        /// <summary>
        /// 图片资源
        /// </summary>
        public SpriteResource sprite;
        public string RegistryName { get => registryName; }

        public RMaterial(ItemMaterialInfo info)
        {
            registryName = info.registryName;
            displayNamePath = info.displayNamePath;
            leffs = info.leffs;
        }

        /// <summary>
        /// 显示名称路径
        /// </summary>
        public string displayNamePath;
        /// <summary>
        /// 显示名称
        /// </summary>
        public string displayName;
        /// <summary>
        /// 羁绊
        /// </summary>
        public LinkageEffectStack[] leffs;

    }
    //信息:物品:材料
    public struct ItemMaterialInfo
    {
        /// <summary>
        /// 注册名
        /// </summary>
        public string registryName;

        /// <summary>
        /// 显示名称路径
        /// </summary>
        public string displayNamePath;

        /// <summary>
        /// 羁绊
        /// </summary>
        public LinkageEffectStack[] leffs;
    }
}