using JetBrains.Annotations;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ER.Parser
{
    /// <summary>
    /// INI�ļ��༭��
    /// </summary>
    public class INIWriter
    {
        private Dictionary<string, Dictionary<string, string>> sections;

        public INIWriter()
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
            if(!sections.ContainsKey(sectionName))
            {
                sections[sectionName] = new Dictionary<string, string>();
            }
        }
        /// <summary>
        /// ��Ӽ�ֵ��
        /// </summary>
        /// <param name="sectionName">���ڽڶ�����</param>
        /// <param name="key">����</param>
        /// <param name="value">ֵ��</param>
        /// <returns>���ָ���ڶβ������򷵻�false�������ʧ��</returns>
        public bool AddPair(string sectionName,string key,string value)
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
        /// �Ƴ�ָ���ڶε���������
        /// </summary>
        /// <param name="sectionName">�ڶ�����</param>
        /// <returns>��ָ���ڶβ����ڣ��򷵻�false</returns>
        public bool DeleteSection(string sectionName)
        {
            if(sections.ContainsKey(sectionName))
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
        public bool DeletePair(string sectionName,string key)
        {
            if(sections.ContainsKey(sectionName) && sections[sectionName].ContainsKey(key))
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
        /// <summary>
        /// ��ָ���ַ��������ֵ�ַ�����Ŀǰ������δ���ƣ�
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
        #endregion
    }
}