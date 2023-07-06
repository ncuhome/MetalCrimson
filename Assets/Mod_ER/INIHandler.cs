using Mod_Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ER.Parser
{
    /// <summary>
    /// INI�ļ�������
    /// </summary>
    public class INIHandler
    {
        private Dictionary<string, Dictionary<string, string>> sections;
        public INIHandler()
        {
            sections = new();
        }

        #region д�뷽��
        /// <summary>
        /// ����µĽڶ�
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
        /// ��Ӽ�ֵ��
        /// </summary>
        /// <param name="sectionName">���ڽڶ�����</param>
        /// <param name="key">����</param>
        /// <param name="value">ֵ��</param>
        /// <returns>���ָ���ڶβ������򷵻�false�������ʧ��</returns>
        public void AddPair(string sectionName, string key, string value)
        {
            //ConsolePanel.Instance.Print($"����д���ֵ�ԣ�[{sectionName}]<{key} = {value}>");
            if (!sections.ContainsKey(sectionName))
            {
                AddSection(sectionName);
            }
            sections[sectionName][key] = UnescapeValue(value);
            //ConsolePanel.Instance.Print($"�ɹ�д���ֵ�ԣ�[{sectionName}]<{key} = {sections[sectionName][key]}>");
        }

        /// <summary>
        /// �Ƴ�ָ���ڶε���������
        /// </summary>
        /// <param name="sectionName">�ڶ�����</param>
        /// <returns>��ָ���ڶβ����ڣ��򷵻�false</returns>
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
        /// �Ƴ�ָ���ڶ��µļ�ֵ��
        /// </summary>
        /// <param name="sectionName">�ڶ�����</param>
        /// <param name="key">����</param>
        /// <returns>���Ƴ�ʧ���򷵻�false</returns>
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
        /// �������ݵ�����INI�ļ�
        /// </summary>
        /// <param name="path">INI�ļ�·��</param>
        public void Save(string path)
        {
            File.WriteAllText(path, GetSaveString());
        }
        /// <summary>
        /// ��ȡ������Ϣ�ı�
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

        #region ��ȡ
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

        #region ���ݲ���
        public void Clear()
        {
            sections.Clear();
        }
        /// <summary>
        /// ����ı����棬ֻ����ָ���ڶε��ı�
        /// </summary>
        /// <param name="sections">��Ҫ�����Ľڶ�����</param>
        public void Clear(params string[] sections)
        {
            // ����һ���µ��ֵ����ڴ洢�����Ľڶ���Ϣ
            Dictionary<string, Dictionary<string, string>> newSections = new Dictionary<string, Dictionary<string, string>>();

            // ����ԭʼ�Ľڶ���Ϣ
            foreach (var section in this.sections)
            {
                string sectionName = section.Key;

                // ���ָ���Ľڶ������ڱ����Ľڶ������У��򽫸ýڶ���Ϣ��ӵ��µ��ֵ���
                if (sections.Contains(sectionName))
                {
                    newSections[sectionName] = section.Value;
                }
            }

            // ���½ڶ���ϢΪ�����Ľڶ���Ϣ
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