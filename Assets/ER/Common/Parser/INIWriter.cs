using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ER.Parser
{
    /// <summary>
    /// INI写入器
    /// </summary>
    public class INIWriter
    {
        private Dictionary<string, Dictionary<string, string>> sections;

        public INIWriter()
        {
            sections = new();
        }

        #region 写入方法

        /// <summary>
        /// 添加新的节段
        /// </summary>
        /// <param name="sectionName"></param>
        public void AddSection(string sectionName)
        {
            if (!sections.ContainsKey(sectionName))
            {
                sections[sectionName] = new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// 添加键值对
        /// </summary>
        /// <param name="sectionName">所在节段名称</param>
        /// <param name="key">键名</param>
        /// <param name="value">值名</param>
        /// <returns>如果指定节段不存在则返回false并且添加失败</returns>
        public bool AddPair(string sectionName, string key, string value)
        {
            if (sections.ContainsKey(sectionName))
            {
                sections[sectionName][key] = value;
                return true;
            }
            AddSection(sectionName);
            sections[sectionName][key] = value;
            return false;
        }

        /// <summary>
        /// 移除指定节段的所有数据
        /// </summary>
        /// <param name="sectionName">节段名称</param>
        /// <returns>若指定节段不存在，则返回false</returns>
        public bool DeleteSection(string sectionName)
        {
            if (sections.ContainsKey(sectionName))
            {
                sections.Remove(sectionName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 移除指定节段下的键值对
        /// </summary>
        /// <param name="sectionName">节段名称</param>
        /// <param name="key">键名</param>
        /// <returns>若移除失败则返回false</returns>
        public bool DeletePair(string sectionName, string key)
        {
            if (sections.ContainsKey(sectionName) && sections[sectionName].ContainsKey(key))
            {
                sections[sectionName].Remove(key);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 保存内容到本地INI文件
        /// </summary>
        /// <param name="path">INI文件路径</param>
        public void Save(string path)
        {
            File.WriteAllText(path, GetSaveString());
        }

        /// <summary>
        /// 获取保存信息文本
        /// </summary>
        /// <returns></returns>
        public string GetSaveString()
        {
            StringBuilder txt = new StringBuilder();
            foreach (string sectionName in sections.Keys)
            {
                txt.Append('[');
                txt.Append(sectionName);
                txt.Append(']');
                txt.Append('\n');
                Dictionary<string, string> section = sections[sectionName];
                foreach (string key in section.Keys)
                {
                    txt.Append(key);
                    txt.Append(' ');
                    txt.Append('=');
                    txt.Append(' ');
                    txt.Append(EscapeValue(section[key]));
                    txt.Append('\n');
                }
            }
            return txt.ToString();
        }

        /// <summary>
        /// 将指定字符串处理成值字符串（目前功能尚未完善）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EscapeValue(string value)
        {
            StringBuilder sb = new();
            foreach (char c in value)
            {
                switch (c)
                {
                    case '\n':
                        sb.Append('\\');
                        sb.Append('n');
                        break;

                    case '\r':
                        sb.Append('\\');
                        sb.Append('r');
                        break;

                    case '\t':
                        sb.Append('\\');
                        sb.Append('t');
                        break;

                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }

        #endregion 写入方法
    }
}