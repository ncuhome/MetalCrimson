using Mod_Console;
using System;
using System.Collections.Generic;

namespace Mod_Save
{
    /// <summary>
    /// 一个存档的全部内容
    /// </summary>
    [Serializable]
    public class SaveData
    {
        #region 属性

        /// <summary>
        /// 存档中其他系统的信息
        /// </summary>
        public Dictionary<string, SaveEntry> SaveEntries = new();

        /// <summary>
        /// 存档自身信息
        /// </summary>
        public Dictionary<string, string> saveInfos = new();

        #endregion 属性

        /// <summary>
        /// 添加存档片段，如果已经存在同名片段，则覆盖旧的
        /// </summary>
        /// <param name="saveEntry"></param>
        public void Add(SaveEntry saveEntry)
        { SaveEntries[saveEntry.identifier] = saveEntry; }

        /// <summary>
        /// 尝试获取存档片段（若不存在则返回null）
        /// </summary>
        /// <param name="identifier">片段标识符</param>
        /// <returns></returns>
        public SaveEntry TryGet(string identifier)
        {
            if (SaveEntries.TryGetValue(identifier, out SaveEntry saveEntry))
            {
                return saveEntry;
            }
            return null;
        }

        /// <summary>
        /// 向控制台输出存档信息
        /// </summary>
        public void PrintInfo()
        {
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
    /// 存档封装器
    /// </summary>
    public class SaveWrapper
    {
        #region 单例封装

        private static SaveWrapper instance;

        public static SaveWrapper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SaveWrapper();
                }
                return instance;
            }
        }

        private SaveWrapper()
        { }

        #endregion 单例封装

        #region 属性

        /// <summary>
        /// 所使用的存档解释器
        /// </summary>
        public ISaveInterpreter interpreter;

        /// <summary>
        /// 存档文件夹路径
        /// </summary>
        public string savePath;

        private List<ISavable> savableObjects = new List<ISavable>();

        /// <summary>
        /// 当前存档的数据
        /// </summary>
        public SaveData Data { get; private set; }

        #endregion 属性

        #region 方法

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
        /// 存档装箱，并更新当前存档数据
        /// </summary>
        public SaveData Pack()
        {
            SaveData saveData = new SaveData();
            // 遍历存储对象列表，将标记为需要存储的对象进行存储操作
            foreach (ISavable savableObject in savableObjects)
            {
                saveData.Add(savableObject.GetSaveEntry());
            }
            return saveData;
        }

        /// <summary>
        /// 将存档序列化
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            return interpreter.Serialize(Data);
        }

        /// <summary>
        /// 存档拆箱，并更新当前存档数据
        /// </summary>
        /// <param name="text">存档文本</param>
        public SaveData Unpack(string text)
        {
            if (interpreter.Deserialize(text, out SaveData data))
            {
                return data;
            }
            else
            {
                ConsolePanel.Instance.Print("解析存档出错，存档已损坏或者不存在");
                return null;
            }
        }

        #endregion 方法
    }
}