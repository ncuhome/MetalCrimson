﻿using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 对角色动作的管理
    /// </summary>
    public class ATActionManager : MonoAttribute
    {
        #region 初始化

        public ATActionManager()
        { AttributeName = nameof(ATActionManager); }

        public override void Initialize()
        {
        }

        #endregion 初始化

        #region 属性
        /// <summary>
        /// 动作预加载栏
        /// </summary>
        public List<MDAction> _actions = new List<MDAction>();

        /// <summary>
        /// 角色拥有的动作集合
        /// </summary>
        private Dictionary<string, MDAction> actions = new();

        /// <summary>
        /// 角色自身的动画机
        /// </summary>
        private Animator animator;

        #endregion 属性

        #region 动作管理
        public void Add(MDAction action)
        {
            if(actions.ContainsKey(action.actionName))
            {
                Debug.LogError("该动作槽位已经被占用");
            }
            action.manager = this;
            action.index = actions.Count;
            actions[action.actionName] = action;
            action.Initialize();
        }

        /// <summary>
        /// 触发角色动作
        /// </summary>
        /// <param name="actionName">动作名称</param>
        public void Action(string actionName)
        {
            if (actions.TryGetValue(actionName, out MDAction action))
            {
                animator.SetInteger("act", action.index);
                return;
            }
            Debug.LogError($"未找到指定动作：{actionName}");
        }
        #endregion
    }
}