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
        /// 当前语言包对象
        /// </summary>
        private LanguagePack pack;
        /// <summary>
        /// 当前语言包对象
        /// </summary>
        public LanguagePack Pack { get { return pack; } }
        #endregion

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