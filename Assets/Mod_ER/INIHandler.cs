using Mod_Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ER.Parser
{
    /// <summary>
    /// INI文件处理器
    /// </summary>
    public class INIHandler
    {
        private Dictionary<string, Dictionary<string, string>> sections;
        public INIHandler()
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
                sections[sectionName] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }
        }
        /// <summary>
        /// 添加键值对
        /// </summary>
        /// <param name="sectionName">所在节段名称</param>
        /// <param name="key">键名</param>
        /// <param name="value">值名</param>
        /// <returns>如果指定节段不存在则返回false并且添加失败</returns>
        public void AddPair(string sectionName, string key, string value)
        {
            //ConsolePanel.Instance.Print($"正在写入键值对：[{sectionName}]<{key} = {value}>");
            if (!sections.ContainsKey(sectionName))
            {
                AddSection(sectionName);
            }
            sections[sectionName][key] = UnescapeValue(value);
            //ConsolePanel.Instance.Print($"成功写入键值对：[{sectionName}]<{key} = {sections[sectionName][key]}>");
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
        #endregion

        #region 读取
        public void ParseINIFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("INI file not found.", filePath);
            }

            string[] lines = File.ReadAllLines(filePath);
            ParseINIText(lines);
        }
        public void ParseINIText(string INIText)
        {
            string[] lines = INIText.Split('\n');
            ParseINIText(lines);
        }
        public void ParseINIText(string[] lines)
        {
            string currentSection = string.Empty;

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith(";"))
                {
                    // Ignore empty lines and comments
                    continue;
                }
                else if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    // Parse section header
                    currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2);
                    AddSection(currentSection);
                }
                else
                {
                    // Parse key-value pair
                    int equalsIndex = trimmedLine.IndexOf('=');
                    if (equalsIndex > 0)
                    {
                        string key = trimmedLine.Substring(0, equalsIndex).Trim();
                        string value = trimmedLine.Substring(equalsIndex + 1).Trim();

                        if (!string.IsNullOrEmpty(currentSection))
                        {
                            AddPair(currentSection, key, value);
                        }
                    }
                }
            }
        }
        #endregion

        #region 内容操作
        public void Clear()
        {
            sections.Clear();
        }
        /// <summary>
        /// 清空文本缓存，只保留指定节段的文本
        /// </summary>
        /// <param name="sections">需要保留的节段名称</param>
        public void Clear(params string[] sections)
        {
            // 创建一个新的字典用于存储保留的节段信息
            Dictionary<string, Dictionary<string, string>> newSections = new Dictionary<string, Dictionary<string, string>>();

            // 遍历原始的节段信息
            foreach (var section in this.sections)
            {
                string sectionName = section.Key;

                // 如果指定的节段名称在保留的节段数组中，则将该节段信息添加到新的字典中
                if (sections.Contains(sectionName))
                {
                    newSections[sectionName] = section.Value;
                }
            }

            // 更新节段信息为保留的节段信息
            this.sections = newSections;
        }
        public string? GetValue(string section, string key)
        {
            if (sections.TryGetValue(section, out var sectionData))
            {
                if (sectionData.TryGetValue(key, out var value))
                {
                    return value;
                }
            }

            return null;
        }
        public Dictionary<string, string>? GetSection(string sectionName)
        {
            if (sections.TryGetValue(sectionName, out var sectionData))
            {
                return sectionData;
            }
            return null;
        }
        private string UnescapeValue(string value)
        {
            StringBuilder unescapedValue = new StringBuilder();

            bool escaped = false;

            foreach (char c in value)
            {
                if (escaped)
                {
                    if (c == 'n')
                    {
                        unescapedValue.Append('\n');
                    }
                    else if (c == 'r')
                    {
                        unescapedValue.Append('\r');
                    }
                    else if (c == 't')
                    {
                        unescapedValue.Append('\t');
                    }
                    else
                    {
                        unescapedValue.Append(c);
                    }

                    escaped = false;
                }
                else
                {
                    if (c == '\\')
                    {
                        escaped = true;
                    }
                    else
                    {
                        unescapedValue.Append(c);
                    }
                }
            }

            return unescapedValue.ToString();
        }
        private string EscapeValue(string value)
        {
            StringBuilder sb = new();
            foreach(char c in value)
            {
                switch(c)
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
        #endregion

        public void Print()
        {
            Console.WriteLine($"sections.Count:{sections.Count}");
            foreach (var sec in sections)
            {
                Console.WriteLine($"[{sec.Key}]:{sec.Value.Count}");
                foreach (KeyValuePair<string, string> kv in sec.Value)
                {
                    kv.Print();
                }
            }
        }
    }
}