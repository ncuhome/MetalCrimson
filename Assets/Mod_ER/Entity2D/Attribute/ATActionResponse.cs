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
        #endregion

        #region 函数
        /// <summary>
        /// 响应指定动作
        /// </summary>
        /// <param name="info">动作事件的信息</param>
        public void ActionResponse(ActionInfo info)
        {
            if (ActionEvent != null) { ActionEvent(info); }
        }
        #endregion

    }
}