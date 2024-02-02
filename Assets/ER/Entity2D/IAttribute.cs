namespace ER.Entity2D
{
    /// <summary>
    /// 对象 特征 接口；
    /// 有了一个描述游戏对象整体的 Entity 类，就需要有一个 类表示 它的部分，也就是它的 特征；
    /// 为此提供一个 接口 作为 游戏对象的特征/部分 的封装
    /// </summary>
    public interface IAttribute
    {
        /// <summary>
        /// 特征的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属实体对象
        /// </summary>
        public Entity Owner { get; set; }

        /// <summary>
        /// 该特征被添加进实体时的初始化
        /// </summary>
        public void Initialize();

        /// <summary>
        /// 销毁该特征，释放资源
        /// </summary>
        public void Destroy();
    }

    /// <summary>
    /// 对 特征 接口进一步封装，适用于非组件类型的特征封装；
    /// 注意在构造函数中重新复制特征的名称；
    /// </summary>
    public abstract class NormalAttribute : IAttribute
    {
        /// <summary>
        /// 特征名称
        /// </summary>
        protected string AttributeName;

        /// <summary>
        /// 特征所属的实体对象
        /// </summary>
        protected Entity owner;

        public string Name { get => AttributeName; set => AttributeName = value; }
        public Entity Owner { get => owner; set => owner = value; }

        public abstract void Initialize();

        public void Destroy()
        { }
    }
}