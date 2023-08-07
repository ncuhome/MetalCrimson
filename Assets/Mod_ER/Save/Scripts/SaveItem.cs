using UnityEngine;

namespace ER.Save
{
    /// <summary>
    /// 可以写入存档的对象
    /// </summary>
    public interface ISavable
    {
        /// <summary>
        /// 片段身份标识（唯一身份标识符）
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// 获取存档片段
        /// </summary>
        /// <returns></returns>
        public SaveEntry GetSaveEntry();

        /// <summary>
        /// 根据存档片段还原对象状态
        /// </summary>
        /// <param name="saveEntry"></param>
        public void Restore(SaveEntry saveEntry);
    }

    public abstract class SaveItem : MonoBehaviour, ISavable
    {
        public string identifier = "Test";
        public string Identifier { get => identifier; }

        /// <summary>
        /// 获取存档片段
        /// </summary>
        /// <returns></returns>
        public abstract SaveEntry GetSaveEntry();

        /// <summary>
        /// 根据存档片段还原对象状态
        /// </summary>
        /// <param name="saveEntry"></param>
        public abstract void Restore(SaveEntry saveEntry);
    }
}