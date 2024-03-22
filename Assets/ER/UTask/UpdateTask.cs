using System;

namespace ER.UTask
{
    /// <summary>
    /// 更新任务
    /// </summary>
    public class UpdateTask
    {
        private IUTaskSender owner;
        private Func<bool> action;
        private TaskStatus status;
        /// <summary>
        /// 所属对象
        /// </summary>
        public IUTaskSender Owner => owner;
        /// <summary>
        /// 任务函数
        /// </summary>
        public Func<bool> Action => action;
        /// <summary>
        /// 标签,用于区分同一个对象引出的不同任务对象
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 当前状态
        /// </summary>
        public TaskStatus Status => status;

        public UpdateTask(IUTaskSender owner, Func<bool> action)
        {
            this.owner = owner;
            this.action = action;
            status = TaskStatus.Wait;
            TaskManager.Instance.Register(this);
        }
        /// <summary>
        /// 启用该任务
        /// </summary>
        public void Start()
        {
            status = TaskStatus.Running;
        }
        /// <summary>
        /// 完成该任务
        /// </summary>
        public void Done()
        {
            status = TaskStatus.Done;
            TaskManager.Instance.Unregister(this);
            owner.TaskCallback(status,Tag);
        }
        /// <summary>
        /// 中断该任务
        /// </summary>
        public void Break()
        {
            status = TaskStatus.Break;
            TaskManager.Instance.Unregister(this);
            owner.TaskCallback(status, Tag);
        }
        /// <summary>
        /// 异常中断该任务
        /// </summary>
        public void ErrorBreak()
        {
            status = TaskStatus.Error;
            TaskManager.Instance.Unregister(this);
            owner.TaskCallback(status, Tag);
        }

    }
    public static class UTask
    {

        /// <summary>
        /// 创建更新任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="func"></param>
        public static UpdateTask CreateTask(this IUTaskSender sender, Func<bool> action)
        {
            return new UpdateTask(sender, action);
        }
    }

}