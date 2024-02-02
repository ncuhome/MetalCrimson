using ER.Control;
using ER.Parser;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace ER
{
    public class ConsolePanel : MonoControlPanel
    {
        #region 单例封装

        private ConsolePanel()
        { handleName = "ConsolePanel"; _panelType = IControlPanel.PanelType.Single; }

        private static ConsolePanel instance;

        public static ConsolePanel Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.LogError($"单例对象不存在:{typeof(ConsolePanel)}");
                }
                return instance;
            }
        }

        /// <summary>
        /// 替换单例对象为自身，如果已存在则销毁自身
        /// </summary>
        protected bool PasteInstance()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                return true;
            }
            Destroy(gameObject);
            return false;
        }

        protected override void Awake()
        {
            if (PasteInstance())
            {
                base.Awake();

                if (font != null)
                {
                    monitor.fontAsset = font;
                    input.fontAsset = font;
                }
                //开关控制台
                //InputManager.InputActions.UI.ConsolePanel.performed += (InputAction.CallbackContext ctx)=> SwitchUsing();
            }
        }

        #endregion 单例封装

        #region 组件 | 属性

        [Tooltip("显示字体")]
        [SerializeField]
        private TMP_FontAsset font;

        public TMP_FontAsset FontAsset
        {
            get => font;
            set
            {
                font = value;
                monitor.fontAsset = font;
                input.fontAsset = font;
            }
        }

        /// <summary>
        /// 使用的指令解释器
        /// </summary>
        public static DefaultInterpreter interpreter = new DefaultInterpreter();//指令器

        [Tooltip("最大消息数")]
        public int maxLines = 50;//消息最大数量

        [Tooltip("历史输入记录的最大限制")]
        public int maxHistory = 20;//历史输入最大记录数量

        [Tooltip("历史输入信息")]
        public List<string> history = new List<string>(20);//历史输入

        [Header("子级绑定")]
        public GameObject canvas;

        /// <summary>
        /// 显示器对象
        /// </summary>
        public TMP_InputField monitor;//显示器

        /// <summary>
        /// 输入框对象
        /// </summary>
        public TMP_InputField input;//输入框

        private int histortIndex = -1;//历史索引
        private bool inputing = false;//是否正处于输入状态

        #endregion 组件 | 属性

        #region 委托

        public void ActiveInput()
        {
            inputing = true;
        }

        public void InactiveInput()
        {
            inputing = false;
        }

        #endregion 委托

        #region 功能函数

        /// <summary>
        /// 关闭面板
        /// </summary>
        public void CloseUsing()
        {
            canvas.SetActive(false);
            input.DeactivateInputField();
            ControlManager.Instance.UnregisterPower(this);
        }

        /// <summary>
        /// 打开面板
        /// </summary>
        public void OpenUsing()
        {
            canvas.SetActive(true);
            input.ActivateInputField();
            ControlManager.Instance.RegisterPower(this);
        }

        /// <summary>
        /// 切换显示状态
        /// </summary>
        public void SwitchUsing()
        {
            if (canvas.activeSelf)
            {
                CloseUsing();
            }
            else
            {
                OpenUsing();
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
        /// 向控制台输出消息
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="txt"></param>
        public static void Print(string txt, bool newline = true)
        {
            ConsolePanel.Instance._Print(txt, newline);
        }

        /// <summary>
        /// 向控制台输出消息
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="txt"></param>
        public static void PrintError(string txt, bool newline = true)
        {
            ConsolePanel.Instance._PrintError(txt, newline);
        }

        /// <summary>
        /// 向控制台打印消息
        /// </summary>
        /// <param name="txt">消息</param>
        private void _Print(string txt, bool newLine = true)
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
        private void _PrintError(string txt, bool newLine = true)
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

        #endregion 功能函数

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
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.F12))//F12调出控制台
                {
                    SwitchUsing();
                }
            }
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

        #endregion 内部函数
    }
}