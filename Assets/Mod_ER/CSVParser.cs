using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Parser
{
    /// <summary>
    /// CSV解析器
    /// </summary>
    public class CSVParser
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
            string[] lines=text.Split('\n');
            foreach (string line in lines)
            {
                string[] fields = ParseCSVLine(line);
                data.Add(fields);
            }
            return data;
        }
        private static string[] ParseCSVLine(string line)
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
    }

}
