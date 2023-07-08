// Ignore Spelling: spc

using ER.Parser;
using System.Linq;

namespace ER.Items
{
    public static class ItemExpand
    {
        /// <summary>
        /// 获取指定物品文本属性 按照 指定字符 分割后的字符串组
        /// </summary>
        /// <param name="item"></param>
        /// <param name="key"></param>
        /// <param name="spc"></param>
        /// <returns></returns>
        public static string[] SplitText(this Item item,string key,char spc)
        {
            if(item.Contains(key,DataType.Text))
            {
                string txt = item.GetText(key);
                return txt.Split(spc);
            }
            return null;
        }

        /// <summary>
        /// 获取指定物品的指定文本属性的分割组，判断该分割组是否包含目标值
        /// </summary>
        /// <param name="item"></param>
        /// <param name="key"></param>
        /// <param name="spc"></param>
        /// <param name="aimValue"></param>
        /// <returns></returns>
        public static bool ContainsSPT(this Item item,string key,char spc,string aimValue)
        {
            string[] spt = item.SplitText(key, spc);
            return spt.Contains(aimValue);
        }

    }
}