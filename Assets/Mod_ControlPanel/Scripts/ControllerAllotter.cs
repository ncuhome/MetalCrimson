using System.Collections.Generic;
using UnityEngine;

namespace Mod_ControlPanel
{
    /// <summary>
    /// 控制权分配器（非组件单例模式）
    /// </summary>
    public class ControllerAllotter
    {
        #region 单例模式

        private static ControllerAllotter instance;

        /// <summary>
        /// 私有构造函数，防止外部实例化
        /// </summary>
        private ControllerAllotter()
        {
            InitializeControlStacks();
        }

        /// <summary>
        /// 单例对象
        /// </summary>
        public static ControllerAllotter Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ControllerAllotter();
                }
                return instance;
            }
        }

        #endregion 单例模式

        #region 属性

        /// <summary>
        /// 最大同时控制数量（注意：重设权栈的数量时会清空当前所有面板的控制权）
        /// </summary>
        public int MaxCount
        {
            get => maxCount;
            set
            {
                OffAll();
                maxCount = value;
                while (controlStacks.Count < maxCount)
                {
                    controlStacks.Add(new Stack<ControlPanel>());
                }
                while (controlStacks.Count > maxCount)
                {
                    controlStacks.RemoveAt(controlStacks.Count - 1);
                }
            }
        }

        private int maxCount = 4;

        /// <summary>
        /// 控制权栈集合
        /// </summary>
        private List<Stack<ControlPanel>> controlStacks;

        #endregion 属性

        /// <summary>
        /// 取消所有控制权
        /// </summary>
        public void OffAll()
        {
            foreach (var cstk in controlStacks)
            {
                while (cstk.Count > 0)
                {
                    cstk.Pop().PowerOff();
                }
            }
        }

        private void InitializeControlStacks()
        {
            controlStacks = new List<Stack<ControlPanel>>();
            for (int i = 0; i < MaxCount; i++)
            {
                controlStacks.Add(new Stack<ControlPanel>());
            }
        }

        /// <summary>
        /// 获取主栈控制权
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        public bool GetPower(ControlPanel panel)
        {
            Debug.Log($"有面板尝试获取权限：{panel.panelName}");

            // 关闭同一栈内的控制面板
            ControlPanel last = controlStacks[0].Count > 0 ? controlStacks[0].Peek() : null;
            if (last != null)
            {
                last.PowerOff();
            }

            // 将面板加入主栈
            controlStacks[0].Push(panel);
            panel.PowerOn();

            return true;
        }

        /// <summary>
        /// 移除指定面板的控制权（前提是当前面板处于有控制权状态）
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        public bool OffPower(ControlPanel panel)
        {
            bool success = false;

            // 在栈集合中查找并关闭控制权
            foreach (var stack in controlStacks)
            {
                if (stack.Count > 0 && stack.Peek() == panel)
                {
                    panel.PowerOff();
                    stack.Pop();

                    // 恢复栈顶面板的控制权
                    if (stack.Count > 0)
                    {
                        ControlPanel newTop = stack.Peek();
                        newTop.PowerOn();
                    }

                    success = true;
                    break;
                }
            }

            return success;
        }

        /// <summary>
        /// 获取并行栈控制权
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        public bool GetParallelPower(ControlPanel panel)
        {
            // 在栈集合中找到一个空栈，并将面板加入
            foreach (var stack in controlStacks)
            {
                if (stack.Count == 0)
                {
                    panel.PowerOn();
                    stack.Push(panel);
                    return true;
                }
            }

            return false;
        }
    }
}