

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ER.Parser
{
    /// <summary>
    /// 简单的指令解释器
    /// </summary>
    public class CommandParser
    {
        /// <summary>
        /// 消息输出接口
        /// </summary>
        public static event Action<string> Output = message => Console.WriteLine(message);

        /// <summary>
        /// 解析指令字符串
        /// </summary>
        /// <param name="txt">指令字符串</param>
        /// <param name="interpreter">指令器</param>
        /// <returns></returns>
        public static Data Parse(string txt, Interpreter interpreter)
        {
            if (interpreter == null)
            {
                Output("解释指令时，指令器不能为NULL！");
                return Data.Empty;
            }
            var parameters = Split(txt);
            if (parameters.Count > 0)//参数不为空
            {
#if TEST
                for(int i=0;i<parameters.Count;i++)
                {
                    Console.WriteLine(parameters[i].data + " " + parameters[i].dataType);
                }
#endif

                string command = string.Empty;//首参数为指令头
                if (parameters[0].Value != null)
                {
#pragma warning disable CS8602
                    command += parameters[0].Value.ToString();
#pragma warning restore CS8602
                }
                Data[] ps = new Data[parameters.Count - 1];//剩余数据元作为指令的参数
                for (int i = 0; i < ps.Length; i++)
                {
                    if (parameters[i + 1].Type == DataType.Function)//如果参数是一条指令，则运行该指令取其返回值作为参数
                    {
#pragma warning disable CS8602
                        ps[i] = Parse(parameters[i + 1].Value.ToString() + string.Empty, interpreter);
#pragma warning restore CS8602
                    }
                    else
                    {
                        ps[i] = parameters[i + 1];
                    }
                }
#if TEST
                for (int i = 0; i < ps.Length; i++)
                {
                    Console.WriteLine(ps[i].data + " " + ps[i].dataType);
                }
#endif
                return interpreter.Parse(command, ps);
            }
            Output("解析参数为空！");
            return Data.Empty;
        }

        /// <summary>
        /// 解析指令文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="interpreter">指令器</param>
        public static void Command(string fileName, Interpreter interpreter)
        {
            if (File.Exists(fileName))
            {
                StreamReader reader = new StreamReader(fileName);
                string line = reader.ReadLine();
                while (line != null)
                {
                    Parse(line, interpreter);
                    line = reader.ReadLine();
                }
            }
            else
            {
                Output("指令文件不存在！");
            }
        }

        /// <summary>
        /// 将字符串切割为若干数据元(以空格切割单元)
        /// </summary>
        /// <param name="txt">原始字符串</param>
        /// <returns></returns>
        private static List<Data> Split(string txt)
        {
            List<Data> list = new List<Data>();
            StringBuilder temp = new StringBuilder();
            int i = 0;

            bool trans = false;//转义状态
            int instruct = 0;//指令状态
            int quote = 0;//引用状态

            int mode = 0;//0正常封装 1指令封装 2字符串封装
#if Test

            Console.WriteLine($"正在解析:{txt}");
#endif
            while (i < txt.Length)
            {
                char c = txt[i++];
                if (trans)
                {
                    switch (c)
                    {
                        case '\\':
                            temp.Append(c);
                            break;

                        case 'r':
                            temp.Append('\r');
                            break;

                        case 'n':
                            temp.Append('\n');
                            break;

                        case 't':
                            temp.Append('\r');
                            break;

                        case '<':
                            temp.Append('<');
                            break;

                        case '>':
                            temp.Append('>');
                            break;
                    }
                    trans = false;
                }
                else
                {
                    switch (c)
                    {
                        case '\t'://忽略
                        case '\r'://忽略
                        case '\n'://忽略
                            break;

                        case ' '://分割符号

                            #region 分割

                            if (instruct > 0 || quote > 0)
                            {
                                temp.Append(c);
                            }
                            else if (temp.Length > 0)//缓存不为空
                            {
                                string s = temp.ToString();
                                switch (mode)
                                {
                                    case 2:
                                        list.Add(Data.ParseTo(s, DataType.Text));
                                        break;

                                    case 1:
                                        list.Add(Data.ParseTo(s, DataType.Function));
                                        break;

                                    case 0:
                                        list.Add(Data.ParseTo(s));
                                        break;
                                }
                                mode = 0;
                                temp.Clear();
                            }

                            #endregion 分割

                            break;

                        case '\\'://开启转义
                            if (instruct > 0)
                            {
                                temp.Append(c);
                            }
                            else
                            {
                                trans = true;
                            }
                            break;

                        case '['://指令头
                            if (quote > 0)
                            {
                                temp.Append(c);
                            }
                            else
                            {
                                if (instruct > 0)
                                {
                                    temp.Append(c);
                                }
                                else
                                {
                                    mode = 1;
                                }
                                instruct++;
                            }
                            break;

                        case ']'://指令尾
                            if (quote > 0)
                            {
                                temp.Append(c);
                            }
                            else
                            {
                                instruct--;
                                if (instruct > 0)
                                {
                                    temp.Append(c);
                                }
                            }
                            break;

                        case '<'://引用头
                            if (instruct > 0 || quote > 0)
                            {
                                temp.Append(c);
                            }
                            else
                            {
                                mode = 2;
                            }
                            quote++;
                            break;

                        case '>'://引用尾
                            quote--;
                            if (instruct > 0 || quote > 0)
                            {
                                temp.Append(c);
                            }
                            break;

                        default:
                            temp.Append(c);
                            break;
                    }
                }
            }

            #region 分割

            if (temp.Length > 0)//缓存不为空
            {
                string s = temp.ToString();
                switch (mode)
                {
                    case 2:
                        list.Add(Data.ParseTo(s, DataType.Text));
                        break;

                    case 1:
                        list.Add(Data.ParseTo(s, DataType.Function));
                        break;

                    case 0:
                        list.Add(Data.ParseTo(s));
                        break;
                }
                temp.Clear();
            }

            #endregion 分割

            return list;
        }
    }

    /// <summary>
    /// 指令器
    /// </summary>
    public abstract class Interpreter
    {
        public static event Action<string> Output = message => Console.WriteLine(message);

        public Data Parse(string commandName, Data[] parameters)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                Output("解释指令时，指令名称不能为空！");
                return Data.Empty;
            }

            return Effectuate(commandName, parameters);
        }

        /// <summary>
        /// 解释指令语句
        /// </summary>
        /// <param name="commandName">指令头（必定不为空）</param>
        /// <param name="parameters">参数表（可能为空）</param>
        /// <returns></returns>
        public abstract Data Effectuate(string commandName, Data[] parameters);
    }

    public class ParseException : Exception
    {
        public ParseException() : base("指令解析错误")
        {
            Console.WriteLine("指令解析错误");
        }

        public ParseException(string message) : base(message)
        {
            Console.WriteLine("指令解析错误");
        }
    }
}