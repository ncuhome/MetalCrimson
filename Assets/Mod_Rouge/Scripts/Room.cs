using System;

namespace Mod_Rouge
{
    /// <summary>
    /// ��������ö��
    /// </summary>
    public enum RoomType
    {
        /// <summary>
        /// ����ս
        /// </summary>
        encounter,
        /// <summary>
        /// �¼���
        /// </summary>
        eventRoom,
        /// <summary>
        /// ������
        /// </summary>
        reward,
        /// <summary>
        /// ��Ϣ��
        /// </summary>
        rest,

        /// <summary>
        /// boss�ؿ�
        /// </summary>
        boss,
        /// <summary>
        /// ���
        /// </summary>
        start,
        /// <summary>
        /// �յ�
        /// </summary>
        end
    }

    /// <summary>
    /// ����ͼ��Ԫ��
    /// 
    /// </summary>
    public struct Room
    {
        /// <summary>
        /// ��������
        /// </summary>
        public RoomType type;
        /// <summary>
        /// ʹ�õķ���ģ��ID
        /// </summary>
        public int useTemplate;

        public static Room StartPoint { get => new Room { type = RoomType.start, useTemplate = 0 }; }
        public static Room EndPoint { get => new Room { type = RoomType.end, useTemplate = 0 }; }
        
        /// <summary>
        /// ����Boss����
        /// </summary>
        /// <param name="random">�����</param>
        /// <param name="map">��ͼ</param>
        /// <param name="deepID">�������</param>
        /// <returns></returns>
        public static Room CreatBossRoom(Random random,Map map,int deepID)
        {
            Room room = new Room();
            room.type = RoomType.boss;
            /*ѡȡģ��*/
            return room;
        }
        /// <summary>
        /// ���������һ�㷿��
        /// </summary>
        /// <param name="random">�����</param>
        /// <param name="map">��ͼ</param>
        /// <param name="deepID">�������</param>
        /// <returns></returns>
        public static Room CreatRandomRoom(Random random, Map map, int deepID)
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
            /*ѡȡģ��*/
            return room;
        }
    }
}