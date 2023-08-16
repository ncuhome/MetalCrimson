
using ER;
using ER.Parser;
using Mod_Console;
using System.Collections.Generic;
using UnityEngine;

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
        end,

        /// <summary>
        /// 出口
        /// </summary>
        exit
    }

    /// <summary>
    /// 肉鸽地图
    /// </summary>
    public class RougeMap : Singleton<RougeMap>
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

        /// <summary>
        /// 房间出口最小数量
        /// </summary>
        public int exitMin_settings = 0;

        /// <summary>
        /// 房间出口最大数量
        /// </summary>
        public int exitMax_settings = 0;

        #endregion 预设属性

        #region 当前属性

        /// <summary>
        /// 玩家所在的层数
        /// </summary>
        public int level;

        /// <summary>
        /// 玩家所在房间类型
        /// </summary>
        public RoomType roomType = RoomType.exit;

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

        /// <summary>
        /// 检查线，同一层中剩余房间小于这个值，开始检查是否强制生成某些房间
        /// </summary>
        private int checkLine = 0;

        /// <summary>
        /// 本层地图长度
        /// </summary>
        public int length = 0;

        /// <summary>
        /// 当前房间的出口
        /// </summary>
        public Room[] exits;

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

            if (int.TryParse(section["exitMin"], out int vvvv))
            {
                exitMin_settings = vvvv;
            }
            else
            {
                ConsolePanel.PrintError("地图配置文件错误：房间出口最小数量");
            }

            if (int.TryParse(section["exitMax"], out int vvvvv))
            {
                exitMax_settings = vvvvv;
            }
            else
            {
                ConsolePanel.PrintError("地图配置文件错误：房间出口最大数量");
            }
            probabilities_settings = new float[4][];
            minCount_settings = new int[4][];
            maxCount_settings = new int[4][];

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
        /// 从头开始（地图初始化）
        /// </summary>
        public Room Start()
        {
            roomType = RoomType.start;
            level = 1;
            roomPassed = 0;
            roomPassedNowLayer = 0;
            counter = new int[7];
            counterNowLayer = new int[7];
            length = Random.Range(minLength_settings, maxLength_settings);

            Next();

            return new Room(RoomType.start, level);
        }

        /// <summary>
        /// 更新当前房间出口
        /// </summary>
        private void Next()
        {
            switch (roomPassedNowLayer - length)
            {
                case 0://通过boss前的最后房间
                    exits = new Room[] { BossRoom() };
                    break;

                case 1://通过boss房
                    exits = new Room[] { EndRoom() };
                    break;

                case 2://进入下一层
                    exits = NextLevel() ;
                    break;

                default:
                    UpdateCheck();
                    int count = Random.Range(exitMin_settings, exitMax_settings+1);//确定出口数量
                    exits = new Room[count];
                    for (int i = 0; i < count; i++)
                    {
                        exits[i] = NormalRoom();
                    }
                    break;
            }
        }

        /// <summary>
        /// 选择房间出口，获得出口的房间对象，并生成它的出口房间
        /// </summary>
        /// <param name="index"></param>
        public Room SelectRoom(int index)
        {
            if(roomType == RoomType.exit)
            {
                ConsolePanel.PrintError($"错误操作：请对地图初始化后进行选择出口");
                return null;
            }
            Record();
            if (index >= 0 && index < exits.Length)
            {
                switch (exits[index].type)
                {
                    case RoomType.start:
                        level++;
                        roomPassedNowLayer = 0;
                        counterNowLayer = new int[7];
                        length = Random.Range(minLength_settings, maxLength_settings);
                        roomType = RoomType.start;
                        Room next = exits[index];
                        Next();
                        return next;

                    case RoomType.exit:
                        roomType = RoomType.exit;
                        return exits[index];

                    default:
                        roomType = exits[index].type;
                        Room next2 = exits[index];
                        Next();
                        return next2;
                }
            }
            ConsolePanel.PrintError("出口索引错误");
            return null;
        }

        private Room NormalRoom()
        {
            if (length - counterNowLayer.Sum() <= checkLine)//检查是否存在必须生成的房间
            {
                bool[] ok = new bool[4];
                for (int i = 0; i < 4; i++)
                {
                    ok[i] = counterNowLayer[i] <= minCount_settings[i].TryValue(level - 1, int.MaxValue);
                }

                int limit = 66;
                RoomType type = RandomType(true);
                if (!ok.OrAll())
                {
                    ConsolePanel.PrintError("无符合条件的房间最小值");
                }
                else
                {
                    while (!ok[(int)type])//如果结果类型不满足条件则再次随机：已有房间数小于最小要求数
                    {
                        type = RandomType(true);
                        limit--;
                        if (limit == 0)
                        {
                            ConsolePanel.PrintError("随机房间生成超出次数限制");
                            break;
                        }
                    }
                }
                return new Room(type, level);
            }
            return new Room(RandomType(true), level);
        }

        private Room BossRoom()
        {
            return new Room (RoomType.boss, level); 
        }

        private Room EndRoom()
        {
            return  new Room(RoomType.end, level) ;
        }

        /// <summary>
        /// 下一层（地图初始化）
        /// </summary>
        private Room[] NextLevel()
        {

            if (level < level_settings)
            {
                Room[] rooms = new Room[2];
                rooms[0] = new Room(RoomType.start, level+1);
                rooms[1] = new Room(RoomType.exit, level+1);
                return rooms;
            }
            return new Room[] { new Room(RoomType.exit, level + 1) };
        }

        public void PrintExits()
        {
            if (exits != null)
            {
                for (int i = 0; i < exits.Length; i++)
                {
                    exits[i].Print();
                }
            }
        }

        public void PrintInfo()
        {
            ConsolePanel.Print($"层数:{level}");
            ConsolePanel.Print($"本层地图长度:{length}");
            ConsolePanel.Print($"当前房间类型:{roomType}");
            ConsolePanel.Print($"已经经过的房间数量:{roomPassed}");
            ConsolePanel.Print($"当前层已经过的数量:{roomPassedNowLayer}");
            ConsolePanel.Print($"各房间经过的数量:{RoomType.encounter}={counter[(int)RoomType.encounter]}," +
                $"{RoomType.eliteRoom}={counter[(int)RoomType.eliteRoom]}," +
                $"{RoomType.eventRoom}={counter[(int)RoomType.eventRoom]}," +
                $"{RoomType.rewardRoom}={counter[(int)RoomType.rewardRoom]}");
            ConsolePanel.Print($"当前层各房间经过的数量:{RoomType.encounter}={counterNowLayer[(int)RoomType.encounter]}," +
                $"{RoomType.eliteRoom}={counterNowLayer[(int)RoomType.eliteRoom]}," +
                $"{RoomType.eventRoom}={counterNowLayer[(int)RoomType.eventRoom]}," +
                $"{RoomType.rewardRoom}={counterNowLayer[(int)RoomType.rewardRoom]}");
        }

        /// <summary>
        /// 更新检查线
        /// </summary>
        private void UpdateCheck()
        {
            checkLine = minCount_settings[0][level - 1] + minCount_settings[1][level - 1] + minCount_settings[2][level - 1] + minCount_settings[3][level - 1];
        }

        /// <summary>
        /// 根据设置随机生成一个房间类型
        /// </summary>
        /// <param name="limitMax">是否受当前房间最大值设定影响</param>
        /// <returns></returns>
        private RoomType RandomType(bool limitMax)
        {
            if (!limitMax) return RandomType();
            //首先检测是否存在可随机的余地
            bool[] filled = new bool[4];
            for (int i = 0; i < 4; i++)
            {
                filled[i] = (counterNowLayer[i] >= maxCount_settings[i].TryValue(level, int.MaxValue));
            }
            if (filled.AndAll())
            {
                return RandomType();
            }

            //存在余地
            RoomType type = RandomType();
            while (filled[(int)type])//如果目标房间已经填满，则再次随机
            {
                type = RandomType();
            }
            return type;
        }

        /// <summary>
        /// 根据设置随机生成一个房间类型
        /// </summary>
        /// <returns></returns>
        private RoomType RandomType()
        {
            return (RoomType)Probabilizer.Parse(0, probabilities_settings[0].TryValue(level - 1, 0f), probabilities_settings[1].TryValue(level - 1, 0f),
                probabilities_settings[2].TryValue(level - 1, 0f), probabilities_settings[3].TryValue(level - 1, 0f));
        }

        /// <summary>
        /// 记录通过信息
        /// </summary>
        private void Record()
        {
            counter[(int)roomType] += 1;
            counterNowLayer[(int)roomType] += 1;
            roomPassed += 1;
            roomPassedNowLayer += 1;
        }
    }
}