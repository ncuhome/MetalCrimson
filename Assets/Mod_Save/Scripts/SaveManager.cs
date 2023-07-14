using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        public List<FileInfo> saves;

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
                saves = directory.GetFiles().ToList();
                for(int i=0;i<saves.Count;i++)
                {
                    if (Path.GetExtension(saves[i].Name) != ".sav")
                    {
                        saves.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        /// <summary>
        /// 保存存档
        /// </summary>
        /// <param name="saveName">存档文件名称(不包含后缀)</param>
        public void Save(string saveName)
        {
            if (!Directory.Exists(savePackPath))
            {
                Directory.CreateDirectory(savePackPath);
            }
            string path = Path.Combine(savePackPath, saveName + ".sav");
            int index = 0;
            while (File.Exists(path))
            {
                path = Path.Combine(savePackPath, saveName + $"({index++}).sav");
            }
            File.Create(path).Close();
            File.WriteAllText(path, SaveWrapper.Instance.Serialize());
        }
    }
}