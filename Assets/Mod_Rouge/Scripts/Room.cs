using System;

namespace Mod_Rouge
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
        /// 出口数量
        /// </summary>
        public int exit;
        /// <summary>
        /// 出口索引起始位置
        /// </summary>
        public int exit_start;

        public static Room StartPoint { get => new Room { type = RoomType.start, useTemplate = 0 }; }
        public static Room EndPoint { get => new Room { type = RoomType.end, useTemplate = 0 }; }
        /// <summary>
        /// 创建Boss房间
        /// </summary>
        /// <param name="random">随机器</param>
        /// <param name="map">地图</param>
        /// <param name="deepID">所在深度</param>
        /// <returns></returns>
        public static Room CreatBossRoom(Random random,Map map,MapLayer layer)
        {
            Room room = new Room();
            room.type = RoomType.boss;
            room.exit = 1;
            /*选取模板*/
            return room;
        }
        /// <summary>
        /// 创建随机的一般房间
        /// </summary>
        /// <param name="random">随机器</param>
        /// <param name="map">地图</param>
        /// <param name="layer">所在地图层</param>
        /// <param name="romly">所在的房间层</param>
        /// <returns></returns>
        public static Room CreatRandomRoom(Random random, Map map, MapLayer layer, int romly)
        {
            Room room = new Room();
            double sr = random.NextDouble();
            if (sr < map.realEncounterRate)
            {
                room.type = RoomType.encounter;
            }
            else if (sr < map.realEncounterRate + map.realEventRate)
            {
                room.type = RoomType.eventRoom;
            }
            else if (sr < map.realEncounterRate + map.realEventRate + map.realRestRate)
            {
                room.type = RoomType.rest;
            }
            else
            {
                room.type = RoomType.reward;
            }

            if (romly < layer.romlys.Length - 1)//不是最后的房间层
            {
                room.exit = random.Next(map.exit_min, map.exit_max);
                room.exit_start = random.Next(-1, 1);
            }
            else
            {
                room.exit = 1;
                room.exit_start = 0;
            }
            /*选取模板*/
            return room;
        }
    }
}