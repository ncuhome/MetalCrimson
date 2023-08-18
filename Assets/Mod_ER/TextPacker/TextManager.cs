using ER;
using ER.Parser;
using UnityEngine;

namespace ER.TextPacker
{
    /// <summary>
    /// 文本管理器
    /// </summary>
    public static class TextManager
    {
        #region 属性
        /// <summary>
        /// 当前语言包对象
        /// </summary>
        private static LanguagePack pack;
        /// <summary>
        /// 当前语言包对象
        /// </summary>
        public static LanguagePack Pack
        {
            get
            {
                if(pack == null)
                {
                    pack = new LanguagePack(PackagePanel.Instance.DefaultPackPath);
                }
                return pack;
            }
            set
            {
                Debug.Log("更新语言包!");
                pack = value;
            }
        }
        #endregion
    }
}