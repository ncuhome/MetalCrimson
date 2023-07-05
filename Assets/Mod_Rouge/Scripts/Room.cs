using System;

namespace Rouge
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


        /// <summary>
        /// �����ʼ��
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