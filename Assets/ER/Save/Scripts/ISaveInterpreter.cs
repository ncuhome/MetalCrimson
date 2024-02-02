// Ignore Spelling: Deserialize

namespace ER.Save
{
    /// <summary>
    /// 存档解释器
    /// </summary>
    public interface ISaveInterpreter
    {
        /// <summary>
        /// 将存档对象序列化成 存档文本
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Serialize(SaveData data);

        /// <summary>
        /// 尝试将存档文本反序列化为 存档对象，转化失败返回false
        /// </summary>
        /// <param name="text">存档文本</param>
        /// <param name="saveData">存档对象</param>
        /// <returns></returns>
        public bool Deserialize(string text, out SaveData saveData);
    }
}