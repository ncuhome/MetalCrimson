// Ignore Spelling: mold leffs

using ER.Resource;

namespace Mod_Item
{
    public class RComponent:IResource
    {
        private string registryName;
        /// <summary>
        /// 图片资源
        /// </summary>
        public SpriteResource sprite;
        public string RegistryName { get => registryName; }

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
        /// 模具注册名
        /// </summary>
        public string moldName;
        /// <summary>
        /// 羁绊
        /// </summary>
        public LinkageEffectStack[] leffs;
        /// <summary>
        /// 部件重量
        /// </summary>
        public float weight;
        /// <summary>
        /// 输入直径
        /// </summary>
        public int in_diameter;
        /// <summary>
        /// 输出直径
        /// </summary>
        public int out_diameter;

    }
}
