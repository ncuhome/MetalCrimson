using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mod_Save
{
    /// <summary>
    /// 一个存档的全部内容
    /// </summary>
    [Serializable]
    public class SaveData
    {
        /// <summary>
        /// 存档信息
        /// </summary>
        public Dictionary<string,SaveEntry> entries = new Dictionary<string, SaveEntry>();
        /// <summary>
        /// 添加存档片段
        /// </summary>
        /// <param name="saveEntry"></param>
        public void Add(SaveEntry saveEntry) { entries.Add(saveEntry.identifier,saveEntry); }
        /// <summary>
        /// 尝试获取存档片段（若不存在则返回null）
        /// </summary>
        /// <param name="identifier">片段标识符</param>
        /// <returns></returns>
        public SaveEntry TryGet(string identifier)
        {
            if(entries.TryGetValue(identifier,out SaveEntry saveEntry))
            {
                return saveEntry;
            }
            return null;
        }
    }

    /// <summary>
    /// 一个存档片段
    /// </summary>
    [Serializable]
    public class SaveEntry
    {
        /// <summary>
        /// 存储信息的身份标记
        /// </summary>
        public string identifier;
        /// <summary>
        /// 存档信息
        /// </summary>
        public Dictionary<string, object> data;
    }

    /// <summary>
    /// 存档系统
    /// </summary>
    public class SaveManager
    {
        private List<ISavable> savableObjects = new List<ISavable>();
        /// <summary>
        /// 存档文件夹路径
        /// </summary>
        public string savePath;


        /// <summary>
        /// 注册存档片段
        /// </summary>
        /// <param name="savableObject"></param>
        public void RegisterObject(ISavable savableObject)
        {
            savableObjects.Add(savableObject);
        }
        /// <summary>
        /// 注销存档片段
        /// </summary>
        /// <param name="savableObject"></param>
        public void UnregisterObject(ISavable savableObject)
        {
            savableObjects.Remove(savableObject);
        }

        /// <summary>
        /// 存储对象
        /// </summary>
        /// <param name="fileName">存档文件名</param>
        public void SaveObjects(string fileName)
        {
            SaveData saveData = new SaveData();
            // 遍历存储对象列表，将标记为需要存储的对象进行存储操作
            foreach (ISavable savableObject in savableObjects)
            {
                saveData.Add(savableObject.GetSaveEntry());
            }
            //File.WriteAllText(Path.Combine(savePath, fileName), JsonConvert.SerializeObject(saveData));
        }

        /// <summary>
        /// 还原对象
        /// </summary>
        /// <param name="fileName">存档文件名</param>
        public void RestoreObjects(string fileName)
        {

            //SaveData saveData = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(Path.Combine(savePath, fileName)));
            //// 遍历存储对象列表，将标记为需要还原的对象进行还原操作
            //foreach (ISavable savableObject in savableObjects)
            //{
            //    SaveEntry saveEntry = saveData.TryGet(savableObject.Identifier);
            //    if(saveEntry!=null)
            //    {
            //        savableObject.Restore(saveEntry);
            //    }
            //}
        }
    }
}
