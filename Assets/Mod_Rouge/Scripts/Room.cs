using ER;

namespace Mod_Rouge
{
    /// <summary>
    /// 肉鸽地图单元；
    ///
    /// </summary>
    public class Room
    {
        #region 属性

        /// <summary>
        /// 房间类型
        /// </summary>
        public RoomType type;

        /// <summary>
        /// 使用的房间模板ID
        /// </summary>
        public int useTemplate;

        /// <summary>
        /// 所在层数
        /// </summary>
        public int level;

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 根据种类和层数生成随机房间（从房间模板中抽取）（未完善）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        public Room(RoomType type, int level)
        {
            this.type = type;
            this.level = level;
        }

        #endregion 构造函数

        public void Print()
        {
            ConsolePanel.Print($"层数：{level}\t类型：{type}\t模板：{useTemplate}");
        }
    }
}