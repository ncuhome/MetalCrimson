using ER.Resource;

namespace Mod_Item
{
    /// <summary>
    /// 部件模具
    /// </summary>
    public class RComponentMold:IResource
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
        /// 显示描述 资源路径
        /// </summary>
        public string descriptionPath;
        /// <summary>
        /// 模具类型注册名
        /// </summary>
        public string typeName;
        /// <summary>
        /// 目标部件注册名
        /// </summary>
        public string targetName;
        /// <summary>
        /// 重量系数:模具重量将会受此属性影响
        /// </summary>
        public float multi_weight;
    }
}