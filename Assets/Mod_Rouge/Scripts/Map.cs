#define Debug
using JetBrains.Annotations;
using Mod_Console;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Mod_Rouge
{
    /// <summary>
    /// 地图层
    /// </summary>
    public class MapLayer
    {
        /// <summary>
        /// 所属地图对象
        /// </summary>
        public Map owner;
        /// <summary>
        /// 层ID
        /// </summary>
        public int deepID;
        /// <summary>
        /// 所有房间对象
        /// </summary>
        public List<Room> rooms;
        /// <summary>
        /// 房间层房间个数
        /// </summary>
        public int[] romlys;
        /// <summary>
        /// 房间排列
        /// </summary>
        public int[][] roomArrage;

        /// <summary>
        /// 长度
        /// </summary>
        public int length;
        /// <summary>
        /// 房间数量
        /// </summary>
        public int count;


        /// <summary>
        /// 随机器
        /// </summary>
        public System.Random random;


        /// <summary>
        /// 配置一个地图层
        /// </summary>
        /// <param name="_owner"></param>
        /// <param name="layerSeed"></param>
        public MapLayer(Map _owner,int layerSeed,int _deepID)
        {
            owner = _owner;
            deepID = _deepID;
            random = new System.Random(owner.seed + layerSeed);
            length = owner.length  + random.Next(-owner.roundLength, owner.roundLength);
            count = owner.count + random.Next(-owner.roundCount, owner.roundCount);
            //生成个房间层包含房间的数量
            romlys = new int[length];
            int width = count / length;//平均
            int amore = count % length;//剩余数
            for(int i=0;i<owner.roundWidth*length/2; i++)
            {
                int index = random.Next(0, length);
                while (romlys[index] <= -owner.roundWidth)
                {
                    index = random.Next(0, length);
                }
                romlys[index]--;
            }
            for(int i=0;i<owner.roundWidth*length/2+amore;i++)
            {
                int index = random.Next(0, length);
                while(romlys[index] >= owner.roundWidth)
                {
                    index = random.Next(0, length);
                }
                romlys[index]++;
            }
            for(int i=0;i<length;i++)
            {
                romlys[i] += width;
            }
#if Debug
            ConsolePanel.Print($"MapLayer Info:\n" +
                $"layerSeed:{layerSeed}\n" +
                $"deepID:{deepID}\n" +
                $"count:{count}\n" +
                $"length:{length}");
            string txt = "";
            for(int i=0;i<length;i++)
            {
                txt += romlys[i] + " ";
            }
            ConsolePanel.Print(txt);
#endif
        }

        /// <summary>
        /// 初始化地图层，生成具体的房间对象
        /// </summary>
        public void Init()
        {
            roomArrage = new int[length][];
            int index = 0;
            for(int i=0;i<length;i++)
            {
                roomArrage[i] = new int[romlys[i]];
                for(int k = 0; k < romlys[i];k++)
                {
                    roomArrage[i][k] = 0;
                }
            }
            

            for(int i=0;i<count;i++)
            {
                rooms.Add(Room.CreatRandomRoom(random,owner,this,FindLayer(i)));
            }
            


        }
        /// <summary>
        /// 查询制定房间所在的房间层号
        /// </summary>
        /// <param name="roomID"></param>
        /// <returns></returns>
        public int FindLayer(int roomID)
        {
            for(int i=0;i<romlys.Length;i++)
            {
                if (roomID < roomArrage[i][0])
                {
                    return i - 1;
                }
            }
            return -1;
        }
    }

    /// <summary>
    /// 肉鸽地图，从起点到终点经过的房间数量是一定的，且起点和终点的数量只有一个;
    /// 一个房间允许有多个出口，且可以拥有多个入口;
    /// 且所有房间情况在地图生成时确定
    /// </summary>
    public class Map
    {
        #region 固定属性（生成配置）
        /// <summary>
        /// 地图层数
        /// </summary>
        public int deepth = 3;
        /// <summary>
        /// 单层房间数量
        /// </summary>
        public int count = 40;
        /// <summary>
        /// 地图长度
        /// </summary>
        public int length = 10;
        /// <summary>
        /// 地图宽度
        /// </summary>
        public int width = 4;
        /// <summary>
        /// 房间出口数量最小值
        /// </summary>
        public int exit_min = 1;
        /// <summary>
        /// 房间出口数量最大值
        /// </summary>
        public int exit_max = 3;

        /// <summary>
        /// 遭遇战房间比率
        /// </summary>
        public float encounterRate = 0.6f;
        /// <summary>
        /// 事件房间比率
        /// </summary>
        public float eventRoomRate = 0.2f;
        /// <summary>
        /// 奖励房间比率
        /// </summary>
        public float rewardRate = 0.1f;
        /// <summary>
        /// 休息室比率
        /// </summary>
        public float restRate = 0.1f;
        #endregion

        #region 随机量限制
        /// <summary>
        /// 遭遇战房间比率浮动范围
        /// </summary>
        public float encounterRoundRate = 0.1f;
        /// <summary>
        /// 事件房间比率浮动范围
        /// </summary>
        public float eventRoundRate = 0.05f;
        /// <summary>
        /// 奖励房间比率浮动范围
        /// </summary>
        public float rewardRoundRate = 0.05f;
        /// <summary>
        /// 休息室比率浮动范围
        /// </summary>
        public float restRoundRate = 0.05f;
        /// <summary>
        /// 单层房间数量浮动值
        /// </summary>
        public int roundCount = 16;
        /// <summary>
        /// 地图长度浮动范围
        /// </summary>
        public int roundLength = 2;
        /// <summary>
        /// 地图宽度浮动范围
        /// </summary>
        public int roundWidth = 2;
        #endregion

        #region 实际属性
        /// <summary>
        /// 地图种子
        /// </summary>
        public int seed = 0;
        /// <summary>
        /// 实际地图层数（一般来说和deepth等价）
        /// </summary>
        public int realDeepth = 3;
        /// <summary>
        /// 实际遭遇战房间比率
        /// </summary>
        public double realEncounterRate;
        /// <summary>
        /// 实际事件房间比率
        /// </summary>
        public double realEventRate;
        /// <summary>
        /// 实际奖励房间比率
        /// </summary>
        public double realRewardRate = 0.1f;
        /// <summary>
        /// 实际休息室比率
        /// </summary>
        public double realRestRate = 0.1f;
        /// <summary>
        /// 地图层对象
        /// </summary>
        public MapLayer[] layers;
        #endregion

        #region 属性
        System.Random random;
        #endregion

        #region 函数
        public static Map Creat(int seed = 0)
        {
            Map map = new Map();
            map.Init(seed);
            return map;
        }
        public void Init(int seed = 0)
        {
            this.seed = seed;
            random = new System.Random(seed);
            realDeepth = deepth;

            //生成各房间的实际占比
            double encounterRateTmp = encounterRate + (random.NextDouble() - 0.5) * 2 * encounterRoundRate;
            double eventRateTmp = eventRoomRate + (random.NextDouble() - 0.5) * 2 * eventRoundRate;
            double rewardRateTmp = rewardRate + (random.NextDouble() - 0.5) * 2 * rewardRoundRate;
            double restRateTmp = restRate + (random.NextDouble() - 0.5) * 2 * restRoundRate;
            double sum = encounterRateTmp + eventRateTmp + rewardRateTmp + restRateTmp;
            realEncounterRate = encounterRateTmp / sum;
            realEventRate = eventRateTmp / sum;
            realRestRate = restRateTmp / sum;
            realRewardRate = rewardRateTmp / sum;

            //初始化地图层
            layers = new MapLayer[realDeepth];
            for(int i=0;i<realDeepth;i++)
            {
                layers[i] = new MapLayer(this,random.Next() + seed,i+1);
            }
#if Debug
            ConsolePanel.Print($"map info:\n" +
                $"seed:{seed}\n" +
                $"deepth:{realDeepth}\n" +
                $"encounterRate:{realEncounterRate}\n" +
                $"eventRate:{realEventRate}\n" +
                $"rewardRate:{realRewardRate}\n" +
                $"restRate:{realRestRate}");
#endif
        }
        #endregion

    }
}