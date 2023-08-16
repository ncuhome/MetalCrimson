using ER;
using ER.Parser;
using UnityEngine;

namespace ER.TextPacker
{
    /// <summary>
    /// 文本管理器
    /// </summary>
    public class TextManager:Singleton<TextManager>
    {
        #region 属性
        /// <summary>
        /// 语言包路径
        /// </summary>
        public string packsPath = Application.streamingAssetsPath + "/Language";
        /// <summary>
        /// 当前语言包对象
        /// </summary>
        private LanguagePack pack;
        /// <summary>
        /// 当前语言包对象
        /// </summary>
        public LanguagePack Pack { get { return pack; } }
        #endregion

        #region 资源|组件
        [SerializeField]
        [Tooltip("设置面板")]
        private PackagePanel Panel;
        #endregion

        /// <summary>
        /// 显示设置面板
        /// </summary>
        public void DisplayPanel()
        {
            Panel.gameObject.SetActive(true);
        }
        /// <summary>
        /// 更新当前使用的语言包
        /// </summary>
        /// <param name="pack"></param>
        public void SetPack(LanguagePack pack)
        {
            this.pack = pack;
        }
    }
}