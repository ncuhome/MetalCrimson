using ER.Save;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ER
{
    /// <summary>
    /// 全局uid对象管理器, 需要初始化设定 SavePath (仍需要优化)
    /// </summary>
    public class UIDManager:Singleton<UIDManager>
    {
        private Dictionary<UID, IUID> items = new Dictionary<UID, IUID>();
        /// <summary>
        /// 注册uuid对象
        /// </summary>
        /// <param name="item"></param>
        public void Registry(IUID item)
        {
            items.Add(item.UUID,item);
        }

        public void Unregistry(IUID item)
        {
            items.Remove(item.UUID);
        }
        public void Unregistry(UID uuid)
        {
            items.Remove(uuid);
        }
        public bool Contains(UID uuid)
        {
            return items.ContainsKey(uuid);
        }
        public bool Contains(IUID item)
        {
            return (items.ContainsKey(item.UUID));
        }
        /// <summary>
        /// 获取指定类型的所有对象
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public IUID[] GetWithClassName(string className)
        {
            List<IUID> uids = new List<IUID>();
            foreach(var pair in items)
            {
                if(pair.Key.ClassName == className)
                {
                    uids.Add(pair.Value);
                }
            }
            return uids.ToArray();
        }
        /// <summary>
        /// 根据uuid获取指定对象
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public IUID Get(UID uuid)
        {
            if(items.TryGetValue(uuid,out IUID item))
            {
                return item;
            }
            return null;
        }
        public T Get<T>(UID uuid) where T: class,IUID
        {
            IUID item = Get(uuid);
            if(item == null)
            {
                return null;
            }
            return item as T;
        }

        /// <summary>
        /// 从库中移除指定uuid对象
        /// </summary>
        /// <param name="className"></param>
        public void Remove(string className)
        {
            foreach (var pair in items)
            {
                if (pair.Key.ClassName == className)
                {
                    items.Remove(pair.Key);
                }
            }
        }
        /// <summary>
        /// 从库中移除指定uuid对象
        /// </summary>
        /// <param name="uuid"></param>
        public void Remove(UID uuid)
        {
            if(items.ContainsKey(uuid))
            {
                items.Remove(uuid);
            }
        }
        /// <summary>
        /// 将uid对象持久化存储
        /// </summary>
        public void Save(string savePath)
        {
            List<ObjectUIDInfo> datas = new List<ObjectUIDInfo>();
            foreach (var item in items)
            {
                ObjectUIDInfo data = item.Value.Serialize();
                if(!data.IsEmpty())
                {
                    datas.Add(data);
                }
            }
            string text = JsonConvert.SerializeObject(datas);
            File.WriteAllText(savePath, text);
        }
        /// <summary>
        /// 获取存储信息并复原这些uid对象(需要子类实现)
        /// </summary>
        public virtual void Load(string savePath)
        {
            //ObjectUIDInfo[] datas = LoadUIDInfo();
            //接下来通过 datas[i] 中的uuid信息判断该 信息属于哪一个 IUID 的派生类,
            //然后使用该类的 Deserialize() 重新复原出该对象
        }
        /// <summary>
        /// 读取储存在本地的uid信息
        /// </summary>
        /// <returns></returns>
        protected ObjectUIDInfo[] LoadUIDInfo(string savePath)
        {
            string text = File.ReadAllText(savePath);
            ObjectUIDInfo[] datas = JsonConvert.DeserializeObject<ObjectUIDInfo[]>(text);
            return datas;
        }


    }
}
