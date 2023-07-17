using ER.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER
{
    public static class ERTool
    {
        /// <summary>
        /// 获取此字符串的解析数据
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Data Parse(this string text)
        {
            return Data.ParseTo(text);
        }
        /// <summary>
        /// 尝试将此字符串解析为整型
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool TryParseInt(this string text,out int Value)
        {
            int num = 0;
            try
            {
                num = Convert.ToInt32(text);
            }
            catch (FormatException)
            {
                Value = 0;
                return false;
            }
            catch (OverflowException)
            {
                Value = 0;
                return false;
            }
            Value = num;
            return true;
        }
        /// <summary>
        /// 尝试将此字符串解析为整型
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool TryParseDouble(this string text, out double Value)
        {
            double num = 0;
            try
            {
                num = Convert.ToDouble(text);
            }
            catch (FormatException)
            {
                Value = 0;
                return false;
            }
            catch (OverflowException)
            {
                Value = 0;
                return false;
            }
            Value = num;
            return true;
        }
        /// <summary>
        /// 尝试将此字符串解析为布尔值
        /// </summary>
        /// <param name="text"></param>
        /// <param name="Vaule"></param>
        /// <returns></returns>
        public static bool TryParseBoolean(this string text,out bool Value)
        {
            if (text.ToUpper() == "TRUE")
            {
                Value = true;
                return true;
            }
            else if (text.ToUpper() == "FALSE")
            {
                Value = false;
                return true;
            }
            Value = false;
            return false;
        }

        public static void Print<TKey, TValue>(this KeyValuePair<TKey, TValue> pair,
            Action<string> printDelegate = null)
        {
            string txt = $"<{pair.Key?.ToString()}>:{pair.Value?.ToString()}";
            printDelegate?.Invoke(txt);
            Console.WriteLine(txt);
        }
    }
}
