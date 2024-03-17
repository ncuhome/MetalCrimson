using ER.Resource;
using Mod_Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoldSelect : MonoBehaviour
{
    [Tooltip("本页面")]
    public GameObject Self;
    [Tooltip("物品容器")]
    public GameObject Content;
    /// <summary>
    /// 页数:
    /// 0:选择模具;类型
    /// 1:选择模具
    /// 2:选择材料
    /// </summary>
    [Tooltip("页数")]
    public int page;

    [Header("按钮")]
    public Button bt_close;
    public Button bt_return;

    /// <summary>
    /// 关闭页面
    /// </summary>
    public void Close()
    {

    }
    /// <summary>
    /// 打开页面
    /// </summary>
    public void Open()
    {
        page = 0;
        UpdateUI();

    }
    /// <summary>
    /// 返回上一页面
    /// </summary>
    public void Return()
    {

    }

    /// <summary>
    /// 更新页面内容
    /// </summary>
    public void UpdatePage()
    {
        RComponentMoldType type = GR.Get<RComponentMoldType>();
    }
    /// <summary>
    /// 更新UI
    /// </summary>
    private void UpdateUI()
    {
        switch(page)
        {
            case 0:
                bt_return.gameObject.SetActive(false);
                break;
            case 1:
                bt_return.gameObject.SetActive(true);
                break;
            case 2:
                bt_return.gameObject.SetActive(true);
                break;
        }
    }

    private void Awake()
    {
        bt_close?.onClick.AddListener(Close);
        bt_return.onClick.AddListener(Return);
    }
}
