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
            /*
            MDAction[] ms= GetComponentsInChildren<MDAction>();
            animator = owner.GetAttribute<ATAnimator>().Animator;
            foreach (MDAction action in ms)
            {
                Add(action);
            }*/
            ATAnimator at = null;
            if (owner.TryGetAttribute("ATAnimator", ref at, (IAttribute _at) =>
                {
                    animator = ((ATAnimator)_at).Animator;
                }))
            {
                animator = at.Animator;
            }
            for (int i = 0; i < _actions.Count; i++)
            {
                Add(_actions[i], i);
            }
            _actions = null;
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

        /// <summary>
        /// 动画机
        /// </summary>
        public Animator Animator
        { get { return animator; } }

        [SerializeField]
        [Tooltip("预加载动作列表 - 不要在运行后修改")]
        private List<MDAction> _actions;

        #endregion 属性

        #region 动作管理

        /// <summary>
        /// 打开混合动画层
        /// </summary>
        public void OpenMixedLayer(int layerIndex)
        {
            animator.SetLayerWeight(layerIndex, 1f);
        }

        /// <summary>
        /// 关闭混合动画层
        /// </summary>
        public void CloseMixedLayer(int layerIndex)
        {
            animator.SetLayerWeight(layerIndex, 0f);
        }

        /// <summary>
        /// 打开混合动画层
        /// </summary>
        /// <param name="layerName"></param>
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
        /// 获取动作对应控制参数
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public string GetActionParamName(MDAction action)
        {
            StringBuilder sb = new StringBuilder("act_");
            sb.Append(action.actionName);
            return sb.ToString();
        }

        /// <summary>
        /// 添加新的动作
        /// </summary>
        /// <param name="action"></param>
        public void Add(MDAction action, int layer)
        {
            if (actions.ContainsKey(action.actionName))
            {
                Debug.LogWarning($"该动作槽位已经被占用:{action.actionName}");
            }
            if (layer < 0) { Debug.LogError($"动作层级索引无效!:{action.actionName}"); return; }

            action.manager = this;
            actions[action.actionName] = action;

            action.Initialize();
        }

        /// <summary>
        /// 触发角色动作
        /// </summary>
        /// <param name="actionName">动作名称</param>
        public void Action(string actionName, params string[] keys)
        {
            Debug.Log($"激发动作: {actionName}");
            if (actions.TryGetValue(actionName, out MDAction action))
            {
                if (action.ActionJudge())
                {
                    switch (action.controlType)
                    {
                        case MDAction.ControlType.Bool:
                            animator.SetBool(GetActionParamName(action), true);
                            break;

                        case MDAction.ControlType.Trigger:
                            animator.SetTrigger(GetActionParamName(action));
                            break;

                        default:
                            Debug.LogError($"未知控制类型:{action.controlType}");
                            break;
                    }
                    Debug.Log($"执行动作:{actionName}");
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
                switch (action.controlType)
                {
                    case MDAction.ControlType.Bool:
                        animator.SetBool(GetActionParamName(action), false);
                        break;

                    case MDAction.ControlType.Trigger:
                        break;

                    default:
                        Debug.LogError($"未知控制类型:{action.controlType}");
                        break;
                }
                action.StopACT();
                return;
            }
            Debug.LogError($"未找到指定动作：{actionName}");
        }

        /// <summary>
        /// 触发动作的特定函数
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="key"></param>
        public void ActionFunction(string actionName, string key)
        {
            if (actions.TryGetValue(actionName, out MDAction action))
            {
                action.ActionFunction(key);
                return;
            }
            Debug.LogError($"未找到指定动作：{actionName}");
        }

        /// <summary>
        /// 中断指定动作
        /// </summary>
        /// <param name="actionName"></param>
        public void Break(string actionName)
        {
            if (actions.TryGetValue(actionName, out MDAction action))
            {
                switch (action.controlType)
                {
                    case MDAction.ControlType.Bool:
                        animator.SetBool(GetActionParamName(action), false);
                        break;

                    case MDAction.ControlType.Trigger:
                        break;

                    default:
                        Debug.LogError($"未知控制类型:{action.controlType}");
                        break;
                }
                action.BreakACT();
                return;
            }
            Debug.LogError($"未找到指定动作：{actionName}");
        }

        #endregion 动作管理
    }
}