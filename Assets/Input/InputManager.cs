using ER.Save;
using System;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 用于 InputSystem 重新绑定 Action 输入键
/// </summary>
public class InputManager : MonoBehaviour
{
    private static DefaultControl inputActions;

    public static DefaultControl InputActions
    {
        get
        {
            if(inputActions == null)
            {
                inputActions = new DefaultControl();
                inputActions.Enable();
            }
            return inputActions;
        }
    }

    /// <summary>
    /// 按键绑定完成
    /// </summary>
    public static event Action rebindComplete;
    /// <summary>
    /// 按键绑定取消
    /// </summary>
    public static event Action rebindCancelled;
    /// <summary>
    /// 按键绑定开始
    /// </summary>
    public static event Action<InputAction,int> rebindStarted;


    private void Awake()
    {
        if(inputActions == null)
        {
            inputActions = new DefaultControl();
            inputActions.Enable();
        }
    }
    /// <summary>
    /// 重新绑定一个动作
    /// </summary>
    /// <param name="actionName">动作名称</param>
    /// <param name="bindingIndex">绑定索引</param>
    /// <param name="text">消息文本</param>
    /// <param name="excludMouse">是否排除鼠标</param>
    public static void InputRebind(string actionName, int bindingIndex,TMP_Text text,bool excludeMouse)
    {
        InputAction action = inputActions.asset.FindAction(actionName);
        if (action == null || action.bindings.Count <= bindingIndex)
        {
            Debug.LogError("未找到指定动作绑定");
            return;
        }
        //检查是否是复合绑定体
        if (action.bindings[bindingIndex].isComposite)
        {
            int firstPartIndex = bindingIndex + 1;
            //下一份绑定体是复合绑定体
            if(firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isComposite)
            {
                Rebind(action, bindingIndex, true,text, excludeMouse);
            }
        }
        else
        {
            Rebind(action,bindingIndex,false,text, excludeMouse);
        }
    }

    /// <summary>
    /// 重新绑定指定动作的输入
    /// </summary>
    /// <param name="actionToRebind">需要重绑的动作</param>
    /// <param name="bindingIndex">动作的输入索引</param>
    private static void Rebind(InputAction actionToRebind, int bindingIndex,bool composite, TMP_Text text,bool excludeMouse)
    {
        if (actionToRebind == null || bindingIndex < 0)
            return;
        text.text = $"Press a {actionToRebind.expectedControlType}";

        //先禁用这个动作
        actionToRebind.Disable();
        //获取绑定设置类
        var rebind = actionToRebind.PerformInteractiveRebinding(bindingIndex);
        //结束绑定/取消绑定操作时 启用动作，并释放资源
        rebind.OnComplete(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();
            //如果是复合绑定体，则递归继续绑定复合体的下一个绑定
            if (composite)
            {
                int nextBindingIndex = bindingIndex + 1;
                if(nextBindingIndex < actionToRebind.bindings.Count && actionToRebind.bindings[nextBindingIndex].isComposite)
                    Rebind(actionToRebind,nextBindingIndex, composite, text, excludeMouse);
            }
            SaveBindingOverride( actionToRebind );
            rebindComplete?.Invoke();
        });
        rebind.OnCancel(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();
            rebindCancelled?.Invoke();
        });

        rebind.WithCancelingThrough("<Keyboard/escape>");
        if(excludeMouse)
        {
            rebind.WithControlsExcluding("Mouse");
        }

        rebindStarted?.Invoke(actionToRebind, bindingIndex);
        //开启绑定操作
        rebind.Start();
    }

    public static string GetBindingName(string actionName,int bindingIndex)
    {
        if (inputActions == null)
        {
            inputActions = new DefaultControl();
        }
        InputAction action = inputActions.asset.FindAction(actionName);
        return action.GetBindingDisplayString(bindingIndex);
    }
    
    public static void SaveBindingOverride(InputAction action)
    {
        for(int i=0;i<action.bindings.Count;i++)
        {
            SettingsManager.Instance[action.actionMap + action.name + i] = action.bindings[i].overridePath;
        }
    }

    public static void LoadBindingOverride(string actionName)
    {
        if (inputActions == null)
        {
            inputActions = new DefaultControl();
        }
        Debug.Log("正在读取覆盖按键");
        InputAction action = inputActions.asset.FindAction(actionName);
        for(int i=0;i<action.bindings.Count;i++)
        {
            string overridePath = SettingsManager.Instance[action.actionMap + action.name + i];
            if(overridePath != null)
            {
                action.ApplyBindingOverride(i,overridePath);
            }
            else
            {
                Debug.Log("未设置覆盖路径");
            }
        }
        action.Enable();
    }
    public static void ResetBinding(string actionName,int bindingIndex)
    {
        InputAction action = inputActions.asset.FindAction(actionName);

        if(action ==null|| action.bindings.Count<=bindingIndex)
        {
            Debug.LogError("找不到指定动作或者绑定");
            return;
        }
        if (action.bindings[bindingIndex].isComposite)//如果是个复合绑定
        {
            for (int i = bindingIndex; i < action.bindings.Count && action.bindings[i].isComposite; i++)
                action.RemoveBindingOverride(i);
        }
        else
        {
            action.RemoveBindingOverride(bindingIndex);
        }
    }
}