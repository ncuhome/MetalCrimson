using System.Collections.Generic;

namespace Rouge
{
    /// <summary>
    /// ����ģ����
    /// </summary>
    public class RoomTemplate
    {
        /// <summary>
        /// ģ��ID
        /// </summary>
        public int ID;
    }
    /// <summary>
    /// ����ģ��ֿ�
    /// </summary>
    public class RoomTLStore
    {
        #region ������װ
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

        #region ����
        
        #endregion

        /// <summary>
        /// ��ʼ�����ӱ��ض�ȡ���ݣ�
        /// </summary>
        public void Init()
        {

        }
    }
}