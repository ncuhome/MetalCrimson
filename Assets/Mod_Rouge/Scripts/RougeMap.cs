using ER.Parser;
using Mod_Console;
using System.Collections.Generic;

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
        /// 精英房
        /// </summary>
        eliteRoom,

        /// <summary>
        /// 奖励房
        /// </summary>
        rewardRoom,

        /// <summary>
        /// 事件房
        /// </summary>
        eventRoom,

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
    /// 肉鸽地图
    /// </summary>
    public class RougeMap
    {
        #region 预设属性

        /// <summary>
        /// 各层房间几率(设置)
        /// </summary>
        public float[][] probabilities_settings;

        /// <summary>
        /// 各层中房间出现最大值(设置)
        /// </summary>
        public int[][] maxCount_settings;

        /// <summary>
        /// 各层中房间出现最小值(设置)
        /// </summary>
        public int[][] minCount_settings;

        /// <summary>
        /// 地图最大长度(设置)
        /// </summary>
        public int maxLength_settings;

        /// <summary>
        /// 地图最小长度(设置)
        /// </summary>
        public int minLength_settings;

        /// <summary>
        /// 地图层数(设置)
        /// </summary>
        public int level_settings;

        #endregion 预设属性

        #region 当前属性

        /// <summary>
        /// 玩家所在的层数
        /// </summary>
        public int level;

        /// <summary>
        /// 玩家所在房间类型
        /// </summary>
        public RoomType roomType;

        /// <summary>
        /// 已经经过的房间数量
        /// </summary>
        public int roomPassed;

        /// <summary>
        /// 当前层玩家已经经过房间的数量
        /// </summary>
        public int roomPassedNowLayer;

        /// <summary>
        /// 各种类型的房间经过的数量
        /// </summary>
        public int[] counter;

        /// <summary>
        /// 当前层各类型房间经过的数量
        /// </summary>
        public int[] counterNowLayer;

        #endregion 当前属性

        /// <summary>
        /// 加载预设
        /// </summary>
        /// <param name="filePath"></param>
        public void LoadConfig(string filePath)
        {
            INIParser parser = new INIParser();
            parser.ParseINIFile(filePath);

            #region 设置基本配置

            Dictionary<string, string> section = parser.GetSection("Common");

            if (int.TryParse(section["maxLength"], out int v))
            {
                maxLength_settings = v;
            }
            else
            {
                maxLength_settings = 10;
                ConsolePanel.PrintError("地图配置文件错误：地图最大长度");
            }
            if (int.TryParse(section["minLength"], out int vvv))
            {
                minLength_settings = vvv;
            }
            else
            {
                minLength_settings = 10;
                ConsolePanel.PrintError("地图配置文件错误：地图最小长度");
            }
            if (int.TryParse(section["level"], out int vv))
            {
                level_settings = vv;
            }
            else
            {
                ConsolePanel.PrintError("地图配置文件错误：地图深度");
            }

            probabilities_settings = new float[4][];

            #endregion 设置基本配置

            #region 设置概率

            section = parser.GetSection("Probabilities");
            if (section.TryGetValue("encounter", out string v1))
            {
                probabilities_settings[(int)RoomType.encounter] = CSVParser.ParseCSVLine(v1).TryParseToFloatArray();
            }
            if (section.TryGetValue("eliteRoom", out string v2))
            {
                probabilities_settings[(int)RoomType.eliteRoom] = CSVParser.ParseCSVLine(v2).TryParseToFloatArray();
            }
            if (section.TryGetValue("rewardRoom", out string v3))
            {
                probabilities_settings[(int)RoomType.rewardRoom] = CSVParser.ParseCSVLine(v3).TryParseToFloatArray();
            }
            if (section.TryGetValue("eventRoom", out string v4))
            {
                probabilities_settings[(int)RoomType.eventRoom] = CSVParser.ParseCSVLine(v4).TryParseToFloatArray();
            }

            #endregion 设置概率

            #region 设置房间最大值

            section = parser.GetSection("MaxCount");
            if (section.TryGetValue("encounter", out string v21))
            {
                maxCount_settings[(int)RoomType.encounter] = CSVParser.ParseCSVLine(v21).TryParseToIntArray();
            }
            if (section.TryGetValue("eliteRoom", out string v22))
            {
                maxCount_settings[(int)RoomType.eliteRoom] = CSVParser.ParseCSVLine(v22).TryParseToIntArray();
            }
            if (section.TryGetValue("rewardRoom", out string v23))
            {
                maxCount_settings[(int)RoomType.rewardRoom] = CSVParser.ParseCSVLine(v23).TryParseToIntArray();
            }
            if (section.TryGetValue("eventRoom", out string v24))
            {
                maxCount_settings[(int)RoomType.eventRoom] = CSVParser.ParseCSVLine(v24).TryParseToIntArray();
            }

            #endregion 设置房间最大值

            #region 设置房间最小值

            section = parser.GetSection("MinCount");
            if (section.TryGetValue("encounter", out string v31))
            {
                minCount_settings[(int)RoomType.encounter] = CSVParser.ParseCSVLine(v31).TryParseToIntArray();
            }
            if (section.TryGetValue("eliteRoom", out string v32))
            {
                minCount_settings[(int)RoomType.eliteRoom] = CSVParser.ParseCSVLine(v32).TryParseToIntArray();
            }
            if (section.TryGetValue("rewardRoom", out string v33))
            {
                minCount_settings[(int)RoomType.rewardRoom] = CSVParser.ParseCSVLine(v33).TryParseToIntArray();
            }
            if (section.TryGetValue("eventRoom", out string v34))
            {
                minCount_settings[(int)RoomType.eventRoom] = CSVParser.ParseCSVLine(v34).TryParseToIntArray();
            }

            #endregion 设置房间最小值
        }
        /// <summary>
        /// 从头开始
        /// </summary>
        public void Start()
        {
            roomType = RoomType.start;
            level = 1;
            roomPassed = 0;
            roomPassedNowLayer = 0;
            counter = new int[7];
            counterNowLayer = new int[7];

        }
    }
}