using System;

namespace Rouge
{
    /// <summary>
    /// 房间类型枚举
    /// </summary>
    public enum RoomType
    {
        /// <summary>
        /// 遭遇战
        /// </summary>
        encounter,
        /// <summary>
        /// 事件房
        /// </summary>
        eventRoom,
        /// <summary>
        /// 奖励房
        /// </summary>
        reward,
        /// <summary>
        /// 休息室
        /// </summary>
        rest,

        /// <summary>
        /// boss关卡
        /// </summary>
        boss,
        /// <summary>
        /// 起点
        /// </summary>
        start,
        /// <summary>
        /// 终点
        /// </summary>
        end
    }

    /// <summary>
    /// 肉鸽地图单元；
    /// 
    /// </summary>
    public struct Room
    {
        /// <summary>
        /// 房间类型
        /// </summary>
        public RoomType type;
        /// <summary>
        /// 使用的房间模板ID
        /// </summary>
        public int useTemplate;


        /// <summary>
        /// 随机初始化
        /// </summary>
        public void RandomInit(Random random,Map map,int deepID)
        {
            double sr = random.NextDouble();
            if (sr<map.realEncounterRate)
            {
                type = RoomType.encounter;
            }
            else if(sr<map.realEncounterRate + map.realEventRate)
            {
                type = RoomType.eventRoom;
            }
            else if(sr < map.realEncounterRate+map.realEventRate+map.realRestRate)
            {
                type = RoomType.rest;
            }
            else
            {
                type = RoomType.reward;
            }
        }
    }
}