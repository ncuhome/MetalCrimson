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
    /// ��ͼ��
    /// </summary>
    public class MapLayer
    {
        /// <summary>
        /// ������ͼ����
        /// </summary>
        public Map owner;
        /// <summary>
        /// ��ID
        /// </summary>
        public int deepID;
        /// <summary>
        /// ���з������
        /// </summary>
        public List<Room> rooms;
        /// <summary>
        /// ����㷿�����
        /// </summary>
        public int[] romlys;

        /// <summary>
        /// ����
        /// </summary>
        public int length;
        /// <summary>
        /// ��������
        /// </summary>
        public int count;


        /// <summary>
        /// �����
        /// </summary>
        public System.Random random;


        /// <summary>
        /// ����һ����ͼ��
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
            //���ɸ������������������
            romlys = new int[length];
            int width = count / length;//ƽ��
            int amore = count % length;//ʣ����
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
            ConsolePanel.Instance.Print($"MapLayer Info:\n" +
                $"layerSeed:{layerSeed}\n" +
                $"deepID:{deepID}\n" +
                $"count:{count}\n" +
                $"length:{length}");
            string txt = "";
            for(int i=0;i<length;i++)
            {
                txt += romlys[i] + " ";
            }
            ConsolePanel.Instance.Print(txt);
#endif
        }

        /// <summary>
        /// ��ʼ����ͼ�㣬���ɾ���ķ������
        /// </summary>
        public void Init()
        {
            for(int i=0;i<count;i++)
            {
                rooms.Add(Room.CreatRandomRoom(random,owner,deepID));
            }
            


        }
    }

    /// <summary>
    /// ����ͼ������㵽�յ㾭���ķ���������һ���ģ��������յ������ֻ��һ��;
    /// һ�����������ж�����ڣ��ҿ���ӵ�ж�����;
    /// �����з�������ڵ�ͼ����ʱȷ��
    /// </summary>
    public class Map
    {
        #region �̶����ԣ��������ã�
        /// <summary>
        /// ��ͼ����
        /// </summary>
        public int deepth = 3;
        /// <summary>
        /// ���㷿������
        /// </summary>
        public int count = 40;
        /// <summary>
        /// ��ͼ����
        /// </summary>
        public int length = 10;
        /// <summary>
        /// ��ͼ���
        /// </summary>
        public int width = 4;
        /// <summary>
        /// �������������Сֵ
        /// </summary>
        public int exit_min = 1;
        /// <summary>
        /// ��������������ֵ
        /// </summary>
        public int exit_max = 3;

        /// <summary>
        /// ����ս�������
        /// </summary>
        public float encounterRate = 0.6f;
        /// <summary>
        /// �¼��������
        /// </summary>
        public float eventRoomRate = 0.2f;
        /// <summary>
        /// �����������
        /// </summary>
        public float rewardRate = 0.1f;
        /// <summary>
        /// ��Ϣ�ұ���
        /// </summary>
        public float restRate = 0.1f;
        #endregion

        #region ���������
        /// <summary>
        /// ����ս������ʸ�����Χ
        /// </summary>
        public float encounterRoundRate = 0.1f;
        /// <summary>
        /// �¼�������ʸ�����Χ
        /// </summary>
        public float eventRoundRate = 0.05f;
        /// <summary>
        /// ����������ʸ�����Χ
        /// </summary>
        public float rewardRoundRate = 0.05f;
        /// <summary>
        /// ��Ϣ�ұ��ʸ�����Χ
        /// </summary>
        public float restRoundRate = 0.05f;
        /// <summary>
        /// ���㷿����������ֵ
        /// </summary>
        public int roundCount = 16;
        /// <summary>
        /// ��ͼ���ȸ�����Χ
        /// </summary>
        public int roundLength = 2;
        /// <summary>
        /// ��ͼ��ȸ�����Χ
        /// </summary>
        public int roundWidth = 2;
        #endregion

        #region ʵ������
        /// <summary>
        /// ��ͼ����
        /// </summary>
        public int seed = 0;
        /// <summary>
        /// ʵ�ʵ�ͼ������һ����˵��deepth�ȼۣ�
        /// </summary>
        public int realDeepth = 3;
        /// <summary>
        /// ʵ������ս�������
        /// </summary>
        public double realEncounterRate;
        /// <summary>
        /// ʵ���¼��������
        /// </summary>
        public double realEventRate;
        /// <summary>
        /// ʵ�ʽ����������
        /// </summary>
        public double realRewardRate = 0.1f;
        /// <summary>
        /// ʵ����Ϣ�ұ���
        /// </summary>
        public double realRestRate = 0.1f;
        /// <summary>
        /// ��ͼ�����
        /// </summary>
        public MapLayer[] layers;
        #endregion

        #region ����
        System.Random random;
        #endregion

        #region ����
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

            //���ɸ������ʵ��ռ��
            double encounterRateTmp = encounterRate + (random.NextDouble() - 0.5) * 2 * encounterRoundRate;
            double eventRateTmp = eventRoomRate + (random.NextDouble() - 0.5) * 2 * eventRoundRate;
            double rewardRateTmp = rewardRate + (random.NextDouble() - 0.5) * 2 * rewardRoundRate;
            double restRateTmp = restRate + (random.NextDouble() - 0.5) * 2 * restRoundRate;
            double sum = encounterRateTmp + eventRateTmp + rewardRateTmp + restRateTmp;
            realEncounterRate = encounterRateTmp / sum;
            realEventRate = eventRateTmp / sum;
            realRestRate = restRateTmp / sum;
            realRewardRate = rewardRateTmp / sum;

            //��ʼ����ͼ��
            layers = new MapLayer[realDeepth];
            for(int i=0;i<realDeepth;i++)
            {
                layers[i] = new MapLayer(this,random.Next() + seed,i+1);
            }
#if Debug
            ConsolePanel.Instance.Print($"map info:\n" +
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