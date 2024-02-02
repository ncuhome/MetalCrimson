using System;
using UnityEngine.InputSystem;

namespace ER.Control
{
    /// <summary>
    /// 拥有控制权的面板;
    /// 在添加控制事件时，需要将委托封装成 能检测控制权的委托
    /// </summary>
    public interface IControlPanel
    {
        /// <summary>
        /// 面板控制权类型
        /// </summary>
        public enum PanelType
        {
            /// <summary>
            /// 单控制栈模式
            /// </summary>
            Single,

            /// <summary>
            /// 多控制栈模式,面板的控制权点可以存在于多个栈中,只要有一个栈中的点是激活的那么这个面板就有控制权
            /// </summary>
            Mutipy
        }

        /// <summary>
        /// 控制权模式
        /// </summary>
        public PanelType panelType { get; }

        /// <summary>
        /// 面板名称
        /// </summary>
        public string HandleName { get; }

        /// <summary>
        /// 控制权是否激活
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 所在控制栈的索引
        /// </summary>
        public int[] StackIndex { get; }

        /// <summary>
        /// 添加控制栈索引
        /// </summary>
        /// <param name="idnex"></param>
        public void AddStackIndex(int index);

        /// <summary>
        /// 移除控制栈索引
        /// </summary>
        /// <param name="index"></param>
        public void RemoveStackIndex(int index);

        /// <summary>
        /// 将非输入监听的委托封装成仅在激活时执行的委托
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Action PackNormal(Action action);

        /// <summary>
        /// 将普通委托封装成检测面板状态的控制委托
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Action<InputAction.CallbackContext> PackDelegate(Action action);

        /// <summary>
        /// 将普通委托封装成检测面板状态的控制委托
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Action<InputAction.CallbackContext> PackDelegate(Action<InputAction.CallbackContext> action);

        /// <summary>
        /// 在面板被销毁时触发的函数
        /// </summary>
        public void OnPanelDestroy();
    }
}