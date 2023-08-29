using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// 输入重新绑定，与UI控件相关
/// </summary>
public class UIInputRebind : MonoBehaviour
{
    #region 属性

    [SerializeField]
    private InputActionReference reference;

    [SerializeField]
    [Tooltip("是否排除鼠标输入")]
    private bool excludeMouse = true;

    [Range(0, 10)]
    [SerializeField]
    [Tooltip("当前选中的绑定")]
    private int selectedBinding;

    [SerializeField]
    private InputBinding.DisplayStringOptions displayStringOptions;

    [Header("绑定信息 - 不要修改它")]
    [SerializeField]
    private InputBinding inputBinding;

    private int bindingIndex;//实际选中的绑定索引
    private string actionName;//当前动作的名称

    [Header("UI相关")]
    [SerializeField]
    [Tooltip("动作名称文本")]
    private TMP_Text actionText;

    [SerializeField]
    [Tooltip("重新绑定按钮")]
    private Button rebindButton;

    [SerializeField]
    [Tooltip("绑定文本")]
    private TMP_Text rebindText;

    [SerializeField]
    [Tooltip("重设按钮")]
    private Button resetButton;
     
    #endregion 属性

    #region 功能

    private void OnEnable()
    {
        rebindButton.onClick.AddListener(DoRebind);
        resetButton.onClick.AddListener(DoReset);
        if (reference != null)
        {
            GetBindingInfo();
            InputManager.LoadBindingOverride(actionName);
            UpdateUI();
        }
        InputManager.rebindComplete += UpdateUI;
        InputManager.rebindCancelled += UpdateUI;
    }
    private void OnDisable()
    {
        InputManager.rebindComplete -= UpdateUI;
        InputManager.rebindCancelled -= UpdateUI;
    }

    /// <summary>
    /// 当编辑状态 检查器中的属性改变时调用的函数
    /// </summary>
    private void OnValidate()
    {
        if (reference != null)
        {
            GetBindingInfo();
            UpdateUI();
        }
    }

    private void GetBindingInfo()
    {
        if (reference.action != null)
            actionName = reference.action.name;
        if (reference.action.bindings.Count > selectedBinding)
        {
            inputBinding = reference.action.bindings[selectedBinding];
            bindingIndex = selectedBinding;
        }
    }

    private void UpdateUI()
    {
        if (actionText != null)
            actionText.text = actionName;

        if (rebindButton != null)
        {
            if (rebindText != null)
            {
                rebindText.text = InputManager.GetBindingName(actionName, bindingIndex);
            }
            else
            {
                rebindText.text = reference.action.GetBindingDisplayString(bindingIndex);
            }
        }
    }

    private void DoRebind()
    {
        InputManager.InputRebind(actionName, bindingIndex,rebindText,excludeMouse);
    }

    private void DoReset()
    {
        InputManager.ResetBinding(actionName, bindingIndex);
        UpdateUI();
    }

    #endregion 功能
}