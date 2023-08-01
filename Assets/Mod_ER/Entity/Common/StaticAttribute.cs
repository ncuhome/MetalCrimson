namespace ER.Entity
{
    /// <summary>
    /// 静态属性
    /// </summary>
    public abstract class StaticAttribute : IAttribute
    {
        #region 字段
        protected string attributeName = "静态属性";
        protected Entity owner;
        #endregion

        #region 构造函数
        public StaticAttribute(Entity _owner)
        {
            owner = _owner;
        }
        #endregion

        #region 属性
        public virtual string Name { get => attributeName; set => attributeName = value; }
        public virtual Entity Owner { get => owner; set => owner = value; }
        #endregion

        #region 功能函数
        public virtual void Destroy() { }
        public virtual object GetStatus()
        {
            return null;
        }

        public abstract void Initialization();
        #endregion
    }
}
