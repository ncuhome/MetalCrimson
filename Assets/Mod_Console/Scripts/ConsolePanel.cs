using ER.Parser;
using Mod_Console;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace Mod_Console
{
    public class ConsolePanel : MonoBehaviour
    {
        #region 单例封装
        public static ConsolePanel Instance { get; private set; } = null;
        public void Awake()
        {
            if (Instance is null) { Instance = this; }
            else { Destroy(gameObject); }
            DontDestroyOnLoad(this);
        }
        #endregion

        #region 组件 | 属性
        /// <summary>
        /// 显示器对象
        /// </summary>
        public TMP_InputField monitor;//显示器
        /// <summary>
        /// 输入框对象
        /// </summary>
        public TMP_InputField input;//输入框
        /// <summary>
        /// 使用的指令解释器
        /// </summary>
        public DefaultInterpreter interpreter = new DefaultInterpreter();//指令器
        /// <summary>
        /// 最大消息数
        /// </summary>
        public int maxLines = 50;//消息最大数量
        /// <summary>
        /// 历史输入记录的最大限制
        /// </summary>
        public int maxHistory = 20;//历史输入最大记录数量
        /// <summary>
        /// 历史输入信息
        /// </summary>
        public List<string> history = new List<string>(20);//历史输入
        private int histortIndex = -1;//历史索引
        private bool inputing = false;//是否正处于输入状态
        #endregion

        #region 委托
        public void ActiveInput()
        {
            inputing = true;
        }
        public void InactiveInput()
        {
            inputing = false;
        }
        #endregion

        #region 功能函数
        /// <summary>
        /// 切换显示状态
        /// </summary>
        public void SwitchUsing()
        {
            if(gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
        /// <summary>
        /// 提交输入框中的信息
        /// </summary>
        public void SendMessage()
        {
            if (input.text != string.Empty)
            {
                if (input.text.StartsWith('/'))//输入的是指令
                {
                    string command = input.text.Substring(1);
                    if (!Command(command))//指令错误
                    {
                        PrintError($"This is not a valid instruction statement : {command}");
                    }
                }
                else
                {
                    Print(input.text);
                }
                RecordInput(input.text);
            }
            input.text = string.Empty;
        }
        /// <summary>
        /// 向控制台打印消息
        /// </summary>
        /// <param name="txt">消息</param>
        public void Print(string txt, bool newLine = true)
        {
            if (newLine) { monitor.text += '\n'; }
            monitor.text += txt;
            LimitLines();
        }
        /// <summary>
        /// 向控制台打印异常消息
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="newLine"></param>
        public void PrintError(string txt, bool newLine = true)
        {
            if (newLine) { monitor.text += '\n'; }
            monitor.text += new StringBuilder("<color=red>").Append(txt).Append("</color>");
            LimitLines();
        }
        /// <summary>
        /// 清空控制台消息
        /// </summary>
        public void Clear()
        {
            monitor.text = string.Empty;
        }
        /// <summary>
        /// 解释指定指令
        /// </summary>
        /// <param name="commandText">指令内容</param>
        /// <returns>是否是一个有效指令</returns>
        public bool Command(string commandText)
        {
            if (CommandParser.Parse(commandText, interpreter).isError())
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 内部函数
        /// <summary>
        /// 限制消息行数
        /// </summary>
        private void LimitLines()
        {

            string[] ts = monitor.text.Split('\n');
            if (ts.Length > maxLines)
            {
                StringBuilder newText = new StringBuilder();
                for (int i = ts.Length - maxLines; i < ts.Length; i++)
                {
                    newText.Append(ts[i]);
                    if (i < ts.Length - 1) { newText.Append("\n"); }
                }
                monitor.text = newText.ToString();
            }
        }
        /// <summary>
        /// 记录输入历史
        /// </summary>
        private void RecordInput(string input)
        {
            history.Add(input);
            if (history.Count > maxHistory)
            {
                history.RemoveAt(0);
            }
            histortIndex = history.Count;
        }
        private void MoveCursorToEnd()
        {
            if (input != null)
            {
                input.caretPosition = input.text.Length;
                input.selectionAnchorPosition = input.text.Length;
                input.selectionFocusPosition = input.text.Length;
            }
        }
        private void Update()
        {
            if (inputing)
            {
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow))//上一条历史输入
                    {
                        histortIndex--;
                        if (histortIndex < -1) { histortIndex = -1; }

                        if (histortIndex > -1 && histortIndex < history.Count)
                        {
                            input.text = history[histortIndex];
                            MoveCursorToEnd();
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow))//下一条历史输入
                    {
                        histortIndex++;
                        if (histortIndex > history.Count) { histortIndex = history.Count - 1; }

                        if (histortIndex > -1 && histortIndex < history.Count)
                        {
                            input.text = history[histortIndex];
                            MoveCursorToEnd();
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                    {
                        SendMessage();
                        input.ActivateInputField();
                    }
                }
            }
        }
        #endregion
    }

}