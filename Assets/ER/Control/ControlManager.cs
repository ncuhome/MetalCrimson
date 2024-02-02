using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Control
{
    /// <summary>
    /// 控制管理器;
    /// 用于管理输入控制权的系统;
    /// 适用于 InputSystem 可覆盖绑定的控制版本;
    /// </summary>
    public class ControlManager : Singleton<ControlManager>
    {
        #region 属性

        /// <summary>
        /// 控制面板列表
        /// </summary>
        private Dictionary<string, IControlPanel> panelDic = new Dictionary<string, IControlPanel>();

        /// <summary>
        /// 控制权栈列
        /// </summary>
        private List<List<IControlPanel>> panels = new List<List<IControlPanel>>();

        [Tooltip("控制权栈数 - 仅在初始化时修改有效")]
        [SerializeField]
        [Range(1, 10)]
        private int _PowerStackCount = 2;

        /// <summary>
        /// 控制权栈数，重新设置会清空所有栈内的控制信息
        /// </summary>
        public int PowerStackCount
        {
            get => _PowerStackCount;
            set
            {
                if (value <= 0 || value > 10)
                {
                    Debug.LogWarning("权栈数量设置过大或者过小");
                }
                else
                {
                    _PowerStackCount = value;
                }
            }
        }

        #endregion 属性

        public ControlManager()
        {
            for (int i = 0; i < PowerStackCount; i++)
            {
                panels.Add(new List<IControlPanel>());
            }
        }

        /// <summary>
        /// 更新所有控制面板的控制权
        /// </summary>
        private void UpdatePower()
        {
            foreach (KeyValuePair<string, IControlPanel> pair in panelDic)
            {
                pair.Value.IsEnable = false;
            }
            for (int i = 0; i < panels.Count; i++)
            {
                if (panels[i].Count >= 1)
                {
                    panels[i][panels[i].Count - 1].IsEnable = true;
                }
            }
        }

        /// <summary>
        /// 将面板注册到管理字典
        /// </summary>
        /// <param name="panel"></param>
        public void RegisterDictionary(IControlPanel panel)
        {
            if (panelDic.ContainsKey(panel.HandleName))
            {
                Debug.LogError($"已经存在相同名称的控制面板{panel.HandleName}");
            }
            else
            {
                panelDic[panel.HandleName] = panel;
            }
        }

        /// <summary>
        /// 将面板从管理字典中注销
        /// </summary>
        /// <param name="panel"></param>
        public void UnregisterDictionary(IControlPanel panel)
        {
            if (panelDic.ContainsKey(panel.HandleName))
            {
                panelDic.Remove(panel.HandleName);
            }
        }

        /// <summary>
        /// 注册新的控制权
        /// </summary>
        /// <param name="panel">控制面板</param>
        /// <param name="stackIndex">申请的栈索引</param>
        public void RegisterPower(IControlPanel panel, int stackIndex = 0)
        {
            if (stackIndex.InRange(0, panels.Count - 1))
            {
                List<IControlPanel> stack = panels[stackIndex];
                if (panel.panelType == IControlPanel.PanelType.Single)//单控制权
                {
                    //先移出栈中全部有关该面板的控制点
                    UnregisterPower(panel, -1);
                }
                stack.Add(panel);//将面板添加至栈顶
                UpdatePower();//更新控制权
            }
            else
            {
                Debug.LogError("访问的控制权栈不存在");
            }
        }

        /// <summary>
        /// 注销面板的指定栈的控制权
        /// </summary>
        /// <param name="panel">控制面板</param>
        /// <param name="stackIndex">值大于零时表示栈索引,小于0时表示移除全部栈中的控制点</param>
        /// <param name="onlyTop">是否仅移除栈顶的控制点</param>
        /// <param name="notUpdatePower">是否不在移动栈顶元素后更新控制权</param>
        public void UnregisterPower(IControlPanel panel, int stackIndex = -1, bool onlyTop = false, bool notUpdatePower = false)
        {
            if (stackIndex < 0)
            {
                for (int i = 0; i < panels.Count; i++)
                {
                    UnregisterPower(panel, i, onlyTop, true);
                }
            }
            else if (stackIndex <= panels.Count - 1)
            {
                List<IControlPanel> stack = panels[stackIndex];

                //从顶部开始检测移除目标

                for (int i = stack.Count - 1; i >= 1; i--)
                {
                    if (stack[i] == panel)
                    {
                        stack.RemoveAt(i);
                    }
                    if (onlyTop) break;
                }
            }
            else
            {
                Debug.LogError("访问的控制权栈不存在");
            }

            if (!notUpdatePower)
                UpdatePower();
        }
    }
}