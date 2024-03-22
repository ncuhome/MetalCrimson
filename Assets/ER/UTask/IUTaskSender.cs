namespace ER.UTask
{
    /// <summary>
    /// 事件回调状态
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// 等待
        /// </summary>
        Wait,
        /// <summary>
        /// 运行中
        /// </summary>
        Running,
        /// <summary>
        /// 成功完成
        /// </summary>
        Done,
        /// <summary>
        /// 被迫终止
        /// </summary>
        Break,
        /// <summary>
        /// 发生错误中断
        /// </summary>
        Error
    }
    /// <summary>
    /// 更新事件发送接口
    /// </summary>
    public interface IUTaskSender
    {
        /// <summary>
        /// 任务回调接口
        /// </summary>
        /// <param name="status"></param>
        public void TaskCallback(TaskStatus status,string tag);
    }
}