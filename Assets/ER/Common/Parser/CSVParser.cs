using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace ER.Parser
{
    /// <summary>
    /// CSV解析器
    /// </summary>
    public static class CSVParser
    {
        /// <summary>
        /// 解析CSV文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static List<string[]> ParseCSV(string filePath)
        {
            List<string[]> data = new List<string[]>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] fields = ParseCSVLine(line);
                    data.Add(fields);
                }
            }

            return data;
        }

        /// <summary>
        /// 解析CSV文本
        /// </summary>
        /// <param name="text">CSV文本</param>
        /// <returns></returns>
        public static List<string[]> ParseCSVText(string text)
        {
            List<string[]> data = new List<string[]>();
            string[] lines = text.Split('\n');
            foreach (string line in lines)
            {
                string[] fields = ParseCSVLine(line);
                data.Add(fields);
            }
            return data;
        }
        /// <summary>
        /// 解析一行csv文本
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static string[] ParseCSVLine(string line)
        {
            List<string> fields = new List<string>();
            StringBuilder fieldBuilder = new StringBuilder();

            bool insideQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    // Check if the double quote is an escape character
                    if (insideQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        fieldBuilder.Append('"');
                        i++; // Skip the second double quote
                    }
                    else
                    {
                        insideQuotes = !insideQuotes;
                    }
                }
                else if (c == ',' && !insideQuotes)
                {
                    fields.Add(fieldBuilder.ToString());
                    fieldBuilder.Clear();
                }
                else
                {
                    fieldBuilder.Append(c);
                }
            }

            fields.Add(fieldBuilder.ToString());

            return fields.ToArray();
        }

        /// <summary>
        /// 尝试将一个字符串数组 解析为 一个整型数组；无法解析的数据将自动填入0
        /// </summary>
        /// <returns></returns>
        public static int[] TryParseToIntArray(this string[] strings)
        {
            int[] values = new int[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                if (int.TryParse(strings[i], out int v))
                {
                    values[i] = v;
                }
                else { values[i] = 0; }
            }
            return values;
        }

        /// <summary>
        /// 尝试将一个字符串数组 解析为 一个单精度浮点数组；无法解析的数据将自动填入0
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static float[] TryParseToFloatArray(this string[] strings)
        {
            float[] values = new float[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                if (float.TryParse(strings[i], out float v))
                {
                    values[i] = v;
                }
                else { values[i] = 0; }
            }
            return values;
        }
    }
}