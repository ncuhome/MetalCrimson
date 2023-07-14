using System.IO;

namespace Mod_Save
{
    /// <summary>
    /// 存档系统
    /// </summary>
    public class SaveManager
    {
        #region 单例封装

        private static SaveManager instance;

        public static SaveManager Instance
        {
            get
            {
                if (instance == null) { instance = new SaveManager(); }
                return instance;
            }
        }

        private SaveManager()
        { }

        #endregion 单例封装

        /// <summary>
        /// 存档目录
        /// </summary>
        public string savePackPath;

        /// <summary>
        /// 存档列表
        /// </summary>
        public FileInfo[] saves;

        /// <summary>
        /// 从自定义路径中读取存档
        /// </summary>
        /// <param name="path"></param>
        public void Load(string path)
        {
            if (File.Exists(path))
            {
                SaveWrapper.Instance.Unpack(File.ReadAllText(path));
            }
        }

        /// <summary>
        /// 更新列表存档
        /// </summary>
        public void UpdateList()
        {
            if (Directory.Exists(savePackPath))
            {
                DirectoryInfo directory = new DirectoryInfo(savePackPath);
                saves = directory.GetFiles();
            }
        }

        /// <summary>
        /// 保存存档
        /// </summary>
        /// <param name="saveName">存档文件名称(不包含后缀)</param>
        public void Save(string saveName)
        {
            string path = Path.Combine(savePackPath, saveName, ".sav");
            int index = 0;
            while (File.Exists(path))
            {
                path = Path.Combine(savePackPath, saveName, $"({index++})", ".sav");
            }
            File.WriteAllText(path, SaveWrapper.Instance.Serialize());
        }
    }
}