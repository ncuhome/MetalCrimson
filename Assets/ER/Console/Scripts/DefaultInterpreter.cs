// Ignore Spelling: openfile

using ER.Parser;
using System.IO;
using UnityEngine;

namespace ER
{
    public class DefaultInterpreter : Interpreter
    {
        public static void Print(string txt, bool newline = true)
        {
            ConsolePanel.Print(txt, newline);
        }

        public static void PrintError(string txt, bool newline = true)
        {
            ConsolePanel.PrintError(txt, newline);
        }

        #region 指令函数

        protected Data CMD_clear()
        {
            ConsolePanel.Instance.Clear();
            return Data.Empty;
        }

        protected Data CMD_Erinbone()
        {
            Print("This is the author Erinbone. This version is currently in the testing phase. Welcome to participate in the testing and report errors to us");
            return Data.Empty;
        }

        protected Data CMD_exit()
        {
            Application.Quit();
            return Data.Empty;
        }

        protected Data CMD_help(Data[] parameters)
        {
            Print("----------help----------");
            Print("This is the help page, you can use help [page] to switch the current page of the help page");
            int page = 1;
            if (parameters.Length == 0)
            {
            }
            else if (parameters[0].Type == DataType.Integer)
            {
                page = (int)parameters[0].Value;
                if (page < 1 || page > 2)
                {
                    PrintError("The specified help page does not exist: Page[1-1]");
                    page = 1;
                }
            }
            else
            {
                return Data.Error;
            }
            switch (page)
            {
                case 1:
                    Print("clear      *clear console message");
                    Print("help [page]     *get help");
                    break;

                case 2:
                    Print("Oh! Construction is underway here");
                    break;
            }
            Print($"-----------[{page}/1]-----------");
            Print("----------help----------");
            return Data.Empty;
        }

        protected Data CMD_print(Data[] parameters)
        {
            if (!parameters.IsEmpty())
            {
                Data txt = parameters[0];
                if (txt.isError())
                {
                    return Data.Error;
                }
                string text = txt.Value.ToString();
                Print(text);
                return new Data(text, DataType.Text);
            }
            return Data.Empty;
        }

        protected Data CMD_openfile(Data[] parameters)
        {
            if (parameters.Length == 0)
            {
                PrintError("Path parameter is empty!");
                return Data.Error;
            }
            Data path = parameters[0];
            if (path.isError())//数据错误或者文件不存在时返回错误
            {
                PrintError("Path parameter error");
                return Data.Error;
            }
            string pt = path.Value.ToString();
            if (File.Exists(pt))
            {
                string txt = File.ReadAllText(pt);
                return new Data(txt, DataType.Text);
            }
            PrintError($"The file path does not exist:{pt}");
            return Data.Error;
        }

        protected Data CMD_path(Data[] parameters)
        {
            if (parameters.Length == 0)
            {
                Print($"dataPath:{Application.dataPath}");
            }
            else if (parameters.IsMate(DataType.Integer))
            {
                switch ((int)parameters[0].Value)
                {
                    case 0:
                        Print($"dataPath:{Application.dataPath}");
                        break;

                    case 1:
                        Print($"consoleLogPath:{Application.consoleLogPath}");
                        break;

                    case 2:
                        Print($"persistentDataPath:{Application.persistentDataPath}");
                        break;

                    case 3:
                        Print($"streamingAssetsPath:{Application.streamingAssetsPath}");
                        break;

                    case 4:
                        Print($"temporaryCachePath:{Application.temporaryCachePath}");
                        break;

                    default:
                        Print($"this is not a path index:{parameters[0].Value}");
                        break;
                }
            }
            return Data.Empty;
        }

        #endregion 指令函数

        /// <summary>
        /// 解释指令语句，优先执行EffectuateSuper函数（由子类实现），如果子类没有注册该指令或者执行失败则执行本函数的解释语句
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override Data Effectuate(string commandName, Data[] parameters)
        {
            Data data = EffectuateSuper(commandName, parameters);
            if (data.isError())
            {
                switch (commandName)
                {
                    case "clear":
                        return CMD_clear();

                    case "Erinbone":
                        return CMD_Erinbone();

                    case "exit":
                        return CMD_exit();

                    case "help":
                        return CMD_help(parameters);

                    case "print":
                        return CMD_print(parameters);

                    case "openfile":
                        return CMD_openfile(parameters);

                    case "path":
                        return CMD_path(parameters);

                    default:
                        return Data.Error;
                }
            }
            return data;
        }

        /// <summary>
        /// 子类需要具体实现的函数
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual Data EffectuateSuper(string commandName, Data[] parameters)
        {
            return Data.Error;
        }
    }
}