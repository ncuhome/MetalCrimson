using System.Collections.Generic;
using System.Text;
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
            MDAction[] ms= GetComponentsInChildren<MDAction>();
            animator = owner.GetAttribute<ATAnimator>().Animator;
            foreach (MDAction action in ms)
            {
                Add(action);
            }
        }

        #endregion 初始化

        #region 属性

        /// <summary>
        /// 角色拥有的动作集合
        /// </summary>
        private Dictionary<string, MDAction> actions = new();

        /// <summary>
        /// 角色自身的动画机
        /// </summary>
        private Animator animator;

        public List<MDAction> _actions;

        #endregion 属性

        #region 动作管理

        public void OpenMixedLayer(string layerName)
        {
            animator.SetLayerWeight(animator.GetLayerIndex(layerName), 1f);
        }

        /// <summary>
        /// 关闭混合动画层
        /// </summary>
        public void CloseMixedLayer(string layerName)
        {
            animator.SetLayerWeight(animator.GetLayerIndex(layerName), 0f);
        }

        /// <summary>
        /// 强制将所有动作停止
        /// </summary>
        public void ForceBackDefault()
        {
            foreach(var pairs in actions)
            {
                MDAction action = pairs.Value;
                if (action.acting)
                {
                    action.acting = false;
                }
                animator.SetInteger(GetActionLayer(action), 0);
            }
        }
        /// <summary>
        /// 获取动作对应层的控制参数 字符串
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private string GetActionLayer(MDAction action)
        {
            StringBuilder sb = new StringBuilder("act");
            if (action.layer == string.Empty)
            {
                sb.Append("_Base Layer");
            }
            else
            {
                sb.Append('_');
                sb.Append(action.layer);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 添加新的动作
        /// </summary>
        /// <param name="action"></param>
        public void Add(MDAction action)
        {
            if(actions.ContainsKey(action.actionName))
            {
                Debug.LogError($"该动作槽位已经被占用:{action.actionName}");
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
        public void Action(string actionName, params string[] keys)
        {
            if (actions.TryGetValue(actionName, out MDAction action))
            {
                if(action.ActionJudge())
                {
                    Debug.Log($"动作参数:{GetActionLayer(action)}, 值:{action.index}");
                    animator.SetInteger(GetActionLayer(action), action.index);
                    action.StartACT(keys);
                    return;
                }
                Debug.Log($"动作未能执行:{actionName}");
                return;
            }
            Debug.LogError($"未找到指定动作：{actionName}");
        }
        /// <summary>
        /// 终止指定动作
        /// </summary>
        /// <param name="actionName"></param>
        public void Stop(string actionName, params string[] keys)
        {
            if (actions.TryGetValue(actionName, out MDAction action))
            {
                animator.SetInteger(GetActionLayer(action), 0);
                action.StopACT(keys);
                return;
            }
            Debug.LogError($"未找到指定动作：{actionName}");
        }
        #endregion
    }
}