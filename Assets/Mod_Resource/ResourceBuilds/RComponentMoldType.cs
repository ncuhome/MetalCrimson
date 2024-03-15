// Ignore Spelling: Mold

using ER.Resource;

namespace Mod_Resource
{
    /// <summary>
    /// 部件模具类型
    /// </summary>
    public class RComponentMoldType:IResource
    {
        private string registryName;

        /// <summary>
        /// 图片资源
        /// </summary>
        public SpriteResource sprite;
        public string RegistryName { get => registryName; }

        public RComponentMoldType(ItemComponentMoldTypeInfo info)
        {
            registryName = info.registryName;
            displayNamePath = info.displayNamePath;
            descriptionPath = info.descriptionPath;
            interfaceTags = info.interfaceTags;
        }
        /// <summary>
        /// 显示名称 资源路径
        /// </summary>
        public string displayNamePath;

        /// <summary>
        /// 显示名称
        /// </summary>
        public string displayName;
        /// <summary>
        /// 显示描述 资源路径
        /// </summary>
        public string descriptionPath;
        /// <summary>
        /// 显示描述
        /// </summary>
        public string description;
        /// <summary>
        /// 接口标签
        /// </summary>
        public string[] interfaceTags;
    }

    public struct ItemComponentMoldTypeInfo
    {
        /// <summary>
        /// 羁绊注册名
        /// </summary>
        public string registryName;
        /// <summary>
        /// 显示名称 资源路径
        /// </summary>
        public string displayNamePath;
        /// <summary>
        /// 显示描述 资源路径
        /// </summary>
        public string descriptionPath;
        /// <summary>
        /// 接口标签
        /// </summary>
        public string[] interfaceTags;
    }
}