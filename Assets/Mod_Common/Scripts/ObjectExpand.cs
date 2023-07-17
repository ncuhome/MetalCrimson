using Mod_Console;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// 通用拓展方法类
    /// </summary>
    public static class ObjectExpand
    {
        /// <summary>
        /// 为此对象创建一个虚拟访问锚点
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="anchorName">锚点名称</param>
        public static void RegisterAnchor(this object obj, string anchorName)
        {
            VirtAnchor anchor = new VirtAnchor(anchorName);
            anchor.SetOwner(obj);
            AnchorManager.Instance.AddAnchor(anchor);
        }
        /// <summary>
        /// 获取字典的深拷贝
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static Dictionary<string,TValue> Copy<TValue>(this Dictionary<string, TValue> dic) 
        {
            Dictionary<string, TValue> d = new();
            foreach(string key in dic.Keys)
            {
                d[key] = dic[key];
            }
            return d;
        }
    }
}