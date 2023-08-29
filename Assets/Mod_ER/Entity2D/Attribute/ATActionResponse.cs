using System;

namespace ER.Entity2D
{
    /// <summary>
    /// 动作响应区域特征，用于接收动作消息，需要挂载 Collider 组件
    /// </summary>
    public class ATActionResponse : MonoAttribute
    {
        #region 初始化
        public ATActionResponse() { AttributeName = nameof(ATActionResponse); }
        public override void Initialize()
        {
            
        }
        #endregion

        #region 事件
        /// <summary>
        /// 接收动作判定时触发的事件
        /// </summary>
        public event Action<ActionInfo> ActionEvent;
        /// <summary>
        /// 预先响应动作的事件
        /// </summary>
        public event Action<ActionInfo> PreActionEvent;
        /// <summary>
        /// 预先响应动作，并给出是否中断该动作的委托
        /// </summary>
        public Func<ActionInfo, bool> JudgeBreak;
        #endregion

        #region 函数
        /// <summary>
        /// 预先响应动作
        /// </summary>
        /// <returns>是否终止此动作继续影响其他的判定</returns>
        public bool PreResponse(ActionInfo info)
        {
            bool breaks = false;
            if (PreActionEvent != null) PreActionEvent(info);
            if(JudgeBreak != null) breaks = JudgeBreak(info);
            return breaks;
        }

        /// <summary>
        /// 响应指定动作
        /// </summary>
        /// <param name="info">动作事件的信息</param>
        public void ActionResponse(ActionInfo info)
        {
            //print($"接受动作，响应动作:{info.actor},{info.name}");
            if (ActionEvent != null) { ActionEvent(info); }
        }
        #endregion

    }
}