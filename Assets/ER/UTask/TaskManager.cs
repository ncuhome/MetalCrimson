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

        private List<UpdateTask> remove = new List<UpdateTask>();//待移除的缓存
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
            remove.Add(task);
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

        private void Remove()
        {
            if (remove.Count == 0) return;
            for(int i=0;i<remove.Count; i++)
            {
                tasks.Remove(remove[i]);
            }
            remove.Clear();
        }

        private void CheckAndUpdate()
        {
            if (tasks.Count == 0) return;
            Remove();
            foreach (UpdateTask task in tasks)
            {
                if (task.Status != TaskStatus.Running) continue;
                if (task.Action == null)
                {
                    task.Done();
                    continue;
                }
                if (task.Action()) task.Done();
                
                    
            }
        }

        private void Update()
        {
            CheckAndUpdate();
        }
    }

}