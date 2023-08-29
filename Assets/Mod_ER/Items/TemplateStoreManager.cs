using ER.Save;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Items
{
    /// <summary>
    /// 模板仓库管理器
    /// </summary>
    public class TemplateStoreManager : Singleton<TemplateStoreManager>
    {
        #region 属性

        /// <summary>
        /// 仓库组
        /// </summary>
        private Dictionary<string, ItemTemplateStore> stores = new Dictionary<string, ItemTemplateStore>();

        #endregion 属性

        #region 功能

        /// <summary>
        /// 重新加载预设目录
        /// </summary>
        public void Load()
        {
            stores = new Dictionary<string, ItemTemplateStore>();

            string settings = SettingsManager.Instance["templateLoadPath"];
            string[] sts = settings.Split(';');
            foreach (string s in sts)
            {
                int sp = s.IndexOf(':');
                string name = s.Substring(0, sp);
                string[] paths = s.Substring(sp + 1).Split(',');
                ItemTemplateStore store = new ItemTemplateStore(name);
                store.LoadItemsList(paths);
                stores[name] = store;
            }
        }

        /// <summary>
        /// 取仓库
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ItemTemplateStore this[string name]
        {
            get
            {
                Debug.Log(stores + " " + name);
                if (stores.TryGetValue(name, out ItemTemplateStore store))
                {
                    return store;
                }
                return null;
            }
        }

        /// <summary>
        /// 清楚指定静态仓库
        /// </summary>
        /// <param name="name"></param>
        public void Clear(string name)
        {
            if (stores.TryGetValue(name, out var store))
            {
                store.Clear();
                return;
            }
            Debug.Log("指定静态仓库不存在");
        }

        #endregion 功能
    }
}