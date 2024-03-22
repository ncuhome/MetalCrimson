using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mod_Forge
{
    /// <summary>
    /// 消息卡片
    /// </summary>
    public class MessageLabel:MonoBehaviour
    {
        [SerializeField]
        private TMP_Text txt;
        [SerializeField]
        private Image img;
        [SerializeField]
        private float animation_speed = 6;

        public float display_time=2;//停留时间

        private float timer = 0;

        /// <summary>
        /// 更新显示信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sprite"></param>
        public void UpdateInfo(string text,Sprite sprite)
        {

        }

        public void Display()
        {

        }

        private void Update()
        {
            
        }
    }
}