// Ignore Spelling: Unescape unescaped

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ER.Parser
{
    public class INIParser
    {
        private Dictionary<string, Dictionary<string, string>> sections;

        public INIParser()
        {
            sections = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
        }

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
                            AddKeyValuePair(currentSection, key, value);
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
        public string  GetValue(string section, string key)
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
        public Dictionary<string,string>  GetSection(string sectionName)
        {
            if(sections.TryGetValue(sectionName, out var sectionData))
            {
                return sectionData;
            }
            return null;
        }
        private void AddSection(string section)
        {
            if (!sections.ContainsKey(section))
            {
                sections.Add(section, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
            }
        }
        private void AddKeyValuePair(string section, string key, string value)
        {
            if (sections.TryGetValue(section, out var sectionData))
            {
                sectionData[key] = UnescapeValue(value);
            }
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
        #endregion

        public void Print()
        {
            Console.WriteLine($"sections.Count:{sections.Count}");
            foreach(var sec in sections)
            {
                Console.WriteLine($"[{sec.Key}]:{sec.Value.Count}");
                foreach(KeyValuePair<string,string> kv in sec.Value)
                {
                    kv.Print();
                }
            }
        }
    }
}