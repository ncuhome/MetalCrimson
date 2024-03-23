using System.Collections.Generic;

namespace ER.ItemStorage
{
    /// <summary>
    /// 物品堆接口: 继承于 IUID 注意查阅 IUID 接口注意事项
    /// </summary>
    public interface IItemStack:IUID
    {
        /// <summary>
        /// 资源母对象, 如果重设则新 物品的属性 覆盖物品堆的除了 amount 外的所有属性
        /// </summary>
        public IItemResource Resource{ get; set; }
        /// <summary>
        /// 物品堆数量
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 物品堆显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 显示信息组
        /// </summary>
        public DescriptionInfo[] Descriptions { get; set; }
        /// <summary>
        /// 是否可堆叠
        /// </summary>
        public bool Stackable { get; set; }
        /// <summary>
        /// 堆叠上限
        /// </summary>
        public int AmountMax { get; set; }
        /// <summary>
        /// 拓展属性
        /// </summary>
        public Dictionary<string, object> Infos { get; set; }
        /// <summary>
        /// 复制一个和自身属性相同的对象
        /// </summary>
        /// <returns></returns>
        public IItemStack Copy();

    }
}