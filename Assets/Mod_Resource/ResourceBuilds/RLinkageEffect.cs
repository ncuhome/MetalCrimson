using ER.Resource;

namespace Mod_Resource
{
    /// <summary>
    /// 羁绊
    /// </summary>
    public class RLinkageEffect:IResource
    {
        public enum DisplayType
        {
            //词条超过 0 后，直接展示给玩家
            Enable,
            //不予展示
            Disable,
            //词条数量达到激活阈值时（即触发后），展示给玩家
            Hide,
        }
        private string registryName;
        public string RegistryName { get => registryName; }

        public RLinkageEffect(LinkageEffectInfo info)
        {
            registryName = info.registryName;
            displayNamePath = info.displayNamePath;
            descriptionPath = info.descriptionPath;
            displayType = info.displayType;
        }

        /// <summary>
        /// 显示名称 资源路径
        /// </summary>
        public string displayNamePath;
        /// <summary>
        /// 显示描述 资源路径
        /// </summary>
        public string descriptionPath;
        /// <summary>
        /// 显示方式
        /// </summary>
        public DisplayType displayType;
    }

    public struct LinkageEffectInfo
    {
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
        /// 显示方式
        /// </summary>
        public RLinkageEffect.DisplayType displayType;
    }

    //羁绊堆
    public struct LinkageEffectStack
    {
        /// <summary>
        /// 羁绊注册名
        /// </summary>
        public string registryName;
        /// <summary>
        /// 羁绊等级
        /// </summary>
        public int level;
    }
}