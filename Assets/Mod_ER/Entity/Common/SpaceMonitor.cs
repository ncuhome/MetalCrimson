namespace ER.Entity
{
    /// <summary>
    /// 控件检测事件
    /// </summary>
    public delegate void DelSpace();
    /// <summary>
    /// 空间检测机基类（不负责动画）
    /// </summary>
    public abstract class SpaceMonitor:DynamicAttribute
    {
        #region 字段
        /// <summary>
        /// 进入此状态时调用的事件
        /// </summary>
        public event DelSpace EnterEvent;
        /// <summary>
        /// 离开此状态时调用的事件
        /// </summary>
        public event DelSpace ExitEvent;
        /// <summary>
        /// 此状态持续时调用的事件（持续调用）
        /// </summary>
        public event DelSpace SustainEvent;
        protected bool spaceConform;
        /// <summary>
        /// 空间状态(当前空间是否是此状态)
        /// </summary>
        public virtual bool SpaceConform
        {
            get => spaceConform;
            protected set
            {
                spaceConform = value;
                if(spaceConform)
                {
                    Enter();
                }
                else
                {
                    Exit();
                }
            }
        }
        #endregion

        #region 
        protected SpaceMonitor() 
        {
            attributeName = "空间状态";
        }
        #endregion

        #region 属性
        public override string Name { get => attributeName;}
        public override Entity Owner { get => owner; set => owner = value as ExciteEntity; }
        #endregion

        /// <summary>
        /// 安全调用委托（进入状态）
        /// </summary>
        protected void Enter() { if (EnterEvent != null) { EnterEvent(); } }
        /// <summary>
        /// 安全调用委托（离开状态）
        /// </summary>
        protected void Exit() { if (ExitEvent != null) { ExitEvent(); } }

        /// <summary>
        /// 环境监测（这里负责对此状态的控制，根据环境检测调整spaceConform状态）
        /// </summary>
        protected abstract void EnvironCheck();
        #region Unity
        private void Update()
        {
            if (SustainEvent != null) { SustainEvent(); }
            EnvironCheck();
        }
        #endregion


    }
}
