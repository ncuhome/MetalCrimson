namespace ER.Entity
{

    /// <summary>
    /// 空间状态事件
    /// </summary>
    /// <param name="name">状态名称</param>
    /// <param name="index">状态的动画索引</param>
    public delegate void DelArea(ATSpaceManager.SpaceInfo info);
    public class ATSpaceManager : StaticAttribute
    {
        public struct SpaceInfo
        {
            public string name;
            public int index;
            public SpaceInfo(string _name,int _index) { name = _name; index = _index; }
        }

        #region 组件
        /// <summary>
        /// 接触器
        /// </summary>
        public TouchLand touchLand;
        #endregion

        #region 事件
        /// <summary>
        /// 空间状态发生切换时触发的事件
        /// </summary>
        public event DelArea SpaceChangeEvent;
        #endregion

        #region 字段
        private readonly SpaceInfo[] spaces = new SpaceInfo[]
        {
            new SpaceInfo("地面",0),
            new SpaceInfo("空中",1),
        };
        private SpaceInfo nowSpace;
        #endregion

        public ATSpaceManager(ExciteEntity _owner, TouchLand _touchLand) :base(_owner)
        {
            attributeName = "空间管理器";
            touchLand = _touchLand;
            touchLand.TouchEvent += Touch;
            touchLand.UntouchLandEvent += Leave;
        }

        #region 内部函数
        private void Touch()
        {
            ChangeSpace(0);
        }
        private void Leave()
        {
            ChangeSpace(1);
        }
        private void ChangeSpace(int index)
        {
            if (SpaceChangeEvent != null) { SpaceChangeEvent(spaces[index]); }
            nowSpace = spaces[index];
        }
        #endregion

        public SpaceInfo NowSpaceInfo()
        {
            return nowSpace;
        }
        public override object GetStatus()
        {
            return nowSpace;
        }

        public override void Initialization()
        {
        }

        #region Unity
        #endregion
    }
}
