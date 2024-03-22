// Ignore Spelling: Unregister

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.UTask
{
    /// <summary>
    /// 更新任务管理器
    /// </summary>
    public class TaskManager:MonoSingletonAutoCreate<TaskManager>
    {
        private HashSet<UpdateTask> tasks = new HashSet<UpdateTask>();
        /// <summary>
        /// 注册任务进表
        /// </summary>
        /// <param name="task"></param>
        public void Register(UpdateTask task)
        {
            tasks.Add(task);
        }
        /// <summary>
        /// 注销任务
        /// </summary>
        /// <param name="task"></param>
        public void Unregister(UpdateTask task)
        {
            tasks.Remove(task);
        }
        /// <summary>
        /// 中断指定对象的所有任务
        /// </summary>
        /// <param name="sender"></param>
        public void Break(IUTaskSender sender)
        {
            foreach (UpdateTask task in tasks)
            {
                if(task.Owner == sender)
                {
                    task.Break();
                }
            }
        }

        private void Update()
        {
            foreach (UpdateTask task in tasks)
            {
                if (task.Status != TaskStatus.Running) continue;
                if(task.Action==null)
                {
                    task.Done();
                }
                else
                {
                    if(task.Action())
                    {
                        task.Done();
                    }
                }
            }
        }
    }

}