using ER.Resource;

namespace ER.ItemStorage
{
    /// <summary>
    /// 物品资源接口
    /// </summary>
    public interface IItemResource:IResource
    {
        /// <summary>
        /// 详细描述
        /// </summary>
        public DescriptionInfo[] Descriptions { get; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; }
        /// <summary>
        /// 是否可堆叠
        /// </summary>
        public bool Stackable { get; }
        /// <summary>
        /// 堆叠上限
        /// </summary>
        public int AmountMax { get;}
    }

    /// <summary>
    /// 显示信息
    /// </summary>
    public struct DescriptionInfo
    {
        /// <summary>
        /// 文本名称(用于显示这个文本是描述什么的, 也可以没有名称, 看需求)
        /// </summary>
        public string key;
        /// <summary>
        /// 显示内容
        /// </summary>
        public string text;
        /// <summary>
        /// 标签标记, 用于分组显示内容(需要使用时指定相应规则)
        /// </summary>
        public string tag; 
    }
}