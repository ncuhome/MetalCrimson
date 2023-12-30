using ER.Entity2D;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Mod_Level
{
    /// <summary>
    /// AI 基类
    /// </summary>
    public abstract class BaseAI : MonoAttribute
    {
        #region 相关

        /// <summary>
        /// 目标描述类
        /// </summary>
        [Serializable]
        protected struct ToDo
        {
            public string name;//目标名称
            public Dictionary<string, object> infos;//行为描述
        }

        #endregion 相关

        public bool active = false;//AI是否活动

        public BaseAI()
        { AttributeName = "AI"; }

        #region 状态机

        /// <summary>
        /// 行动目标
        /// </summary>
        [SerializeField]
        protected List<ToDo> ToDoList = new();

        protected void Do()
        {
            if(ToDoList.Count == 0)
            {
                DoDefault();
            }
            else
            {
                ToDo todo = ToDoList[ToDoList.Count-1];
                if (ParseToDo(todo))
                {
                    ToDoList.Remove(todo);
                }
            }
        }

        /// <summary>
        /// 解析做指定事情
        /// </summary>
        /// <returns>在解析该行为后是否从列表中移除该行为</returns>
        protected abstract bool ParseToDo(ToDo toDo);

        /// <summary>
        /// 默认行为
        /// </summary>
        protected abstract void DoDefault();

        protected virtual void Update()
        {
            if (!active) return;
            Do();
        }

        #endregion 状态机
    }
}