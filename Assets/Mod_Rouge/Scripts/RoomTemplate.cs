using System.Collections.Generic;

namespace Rouge
{
    /// <summary>
    /// 房间模板类
    /// </summary>
    public class RoomTemplate
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        public int ID;
    }
    /// <summary>
    /// 房间模板仓库
    /// </summary>
    public class RoomTLStore
    {
        #region 单例封装
        private static RoomTLStore instance;
        public static RoomTLStore Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new RoomTLStore();
                    instance.Init();
                }
                return instance;
            }
        }
        private RoomTLStore() { }
        #endregion

        #region 属性
        
        #endregion

        /// <summary>
        /// 初始化（从本地读取数据）
        /// </summary>
        public void Init()
        {

        }
    }
}