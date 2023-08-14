using ER;
using ER.Control;
using ER.Parser;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ER.TextPacker
{
    /// <summary>
    /// 语言包选项卡
    /// </summary>
    public class PackItem : MonoBehaviour, IHandler
    {
        #region 属性
        private LanguagePackInfo info;

        [Header("组件绑定")]
        [SerializeField]
        [Tooltip("语言包图片")]
        private Image image;
        [SerializeField]
        [Tooltip("语言包标题")]
        private TMP_Text titleText;
        [SerializeField]
        [Tooltip("语言包版本")]
        private TMP_Text versionText;
        [SerializeField]
        [Tooltip("语言包作者")]
        private TMP_Text auhtorText;

        [Header("默认资源")]
        [SerializeField]
        [Tooltip("默认图片")]
        private Sprite defSprite;
        [SerializeField]
        [Tooltip("默认标题")]
        private string defTitle = "未知语言包";

        [Header("其他设置")]
        [Range(10,99)]
        [SerializeField]
        [Tooltip("最大显示长度")]
        private int maxLength = 16;
        [SerializeField]
        [Tooltip("正常状态颜色")]
        private Color color_ok = Color.green;
        [SerializeField]
        [Tooltip("异常状态颜色")]
        private Color color_warming = Color.red;
        #endregion

        #region 功能
        private string LimitLength(StringBuilder sb)
        {
            if (sb.Length > maxLength)
            {
                sb.Remove(maxLength, sb.Length - maxLength);
                sb.Append("...");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 同步显示信息
        /// </summary>
        /// <param name="info"></param>
        public void UpdateInfo(LanguagePackInfo info)
        {
            this.info = info;
            if(File.Exists(info.ImagePath))
            {
                image.sprite = ERTool.LoadTextureByIO(info.ImagePath).TextureToSprite();
            }
            else
            {
                image.sprite = defSprite;
            }
            StringBuilder sb = new StringBuilder(info.LanguagePackName);
            titleText.text = LimitLength(sb);

            sb.Clear();
            sb.Append("ver:");
            sb.Append(info.LanguagePackVersion);
            versionText.text = LimitLength(sb);
            if(IsMateVersion())
            {
                versionText.color = color_ok;
            }
            else
            {
                versionText.color = color_warming;
            }

            sb.Clear();
            sb.Append("author:");
            sb.Append(info.LanguagePackAuthor);
            auhtorText.text = LimitLength(sb);

        }
        /// <summary>
        /// 检测是否匹配当前版本(未完善)
        /// </summary>
        /// <returns></returns>
        private bool IsMateVersion()
        {
            return true;
        }
        #endregion

        #region 鼠标控制
        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
           
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }

        public void OnPointerExit(PointerEventData eventData)
        { 
          
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            
        }
        #endregion
    }
}