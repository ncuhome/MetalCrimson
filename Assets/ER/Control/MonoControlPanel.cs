using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ER.Control
{
    /// <summary>
    /// 控制面板基类（组件）
    /// </summary>
    public abstract class MonoControlPanel : MonoBehaviour, IControlPanel
    {
        [SerializeField]
        [Tooltip("面板名称")]
        protected string handleName;

        /// <summary>
        /// 是否激活（不要修改）
        /// </summary>
        protected bool isEnable = false;

        /// <summary>
        /// 所在控制栈索引
        /// </summary>
        protected List<int> stackIndexes = new List<int>();

        protected IControlPanel.PanelType _panelType;

        public IControlPanel.PanelType panelType { get => _panelType; }

        public string HandleName => handleName;

        public bool IsEnable
        {
            get => isEnable;
            set => isEnable = value;
        }

        public int[] StackIndex
        {
            get
            {
                return stackIndexes.ToArray();
            }
        }

        public void AddStackIndex(int index)
        {
            if (!stackIndexes.Contains(index))
            {
                stackIndexes.Add(index);
            }
        }

        public void RemoveStackIndex(int index)
        {
            if (stackIndexes.Contains(index))
            {
                stackIndexes.Remove(index);
            }
        }

        public Action PackNormal(Action action)
        {
            return delegate ()
            {
                if (isEnable)
                {
                    if (action != null)
                        action();
                }
            };
        }

        public Action<InputAction.CallbackContext> PackDelegate(Action action)
        {
            return delegate (InputAction.CallbackContext context)
            {
                if (IsEnable)
                {
                    if (action != null)
                        action();
                }
            };
        }

        public Action<InputAction.CallbackContext> PackDelegate(Action<InputAction.CallbackContext> action)
        {
            return delegate (InputAction.CallbackContext context)
            {
                if (IsEnable)
                {
                    if (action != null)
                        action(context);
                }
            };
        }

        public virtual void OnPanelDestroy()
        {
            ControlManager.Instance.UnregisterDictionary(this);
        }

        protected virtual void Awake()
        {
            ControlManager.Instance.RegisterDictionary(this);
        }

        protected virtual void OnDestroy()
        {
            OnPanelDestroy();
        }
    }

    /// <summary>
    /// 控制面板基类
    /// </summary>
    public abstract class ControlPanel : IControlPanel
    {
        /// <summary>
        /// 面板名称
        /// </summary>
        protected string handleName;

        /// <summary>
        /// 是否激活（不要修改）
        /// </summary>
        protected bool isEnable = false;

        /// <summary>
        /// 所在控制栈索引
        /// </summary>
        protected List<int> stackIndexes = new List<int>();

        protected IControlPanel.PanelType _panelType;

        public IControlPanel.PanelType panelType { get => _panelType; }

        public string HandleName => handleName;

        public bool IsEnable
        {
            get => isEnable;
            set => isEnable = value;
        }

        public int[] StackIndex
        {
            get
            {
                return stackIndexes.ToArray();
            }
        }

        public void AddStackIndex(int index)
        {
            if (!stackIndexes.Contains(index))
            {
                stackIndexes.Add(index);
            }
        }

        public void RemoveStackIndex(int index)
        {
            if (stackIndexes.Contains(index))
            {
                stackIndexes.Remove(index);
            }
        }

        public Action PackNormal(Action action)
        {
            return delegate ()
            {
                if (isEnable)
                {
                    if (action != null)
                        action();
                }
            };
        }

        public Action<InputAction.CallbackContext> PackDelegate(Action action)
        {
            return delegate (InputAction.CallbackContext context)
            {
                if (IsEnable)
                {
                    if (action != null)
                        action();
                }
            };
        }

        public Action<InputAction.CallbackContext> PackDelegate(Action<InputAction.CallbackContext> action)
        {
            return delegate (InputAction.CallbackContext context)
            {
                if (IsEnable)
                {
                    if (action != null)
                        action(context);
                }
            };
        }

        public virtual void OnPanelDestroy()
        {
            ControlManager.Instance.RegisterDictionary(this);
        }

        private ControlPanel()
        {
            ControlManager.Instance.RegisterDictionary(this);
        }

        ~ControlPanel()
        {
            OnPanelDestroy();
        }
    }
}