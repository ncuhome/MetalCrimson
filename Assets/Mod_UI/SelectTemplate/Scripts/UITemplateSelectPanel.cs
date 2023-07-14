using ER.Items;
using System.Collections.Generic;
using UnityEngine;

namespace UI.SelectTemplate
{
    public class UITemplateSelectPanel : MonoBehaviour
    {
        private List<ItemTemplate> tmps;//一级模板表
        private List<ItemTemplate> tmps2;//二级模板表

        private enum Status
        { MainTMP, ChildTMP, Make, Closed }

        private Status state;//面板状态

        /// <summary>
        /// 信息显示面板
        /// </summary>
        public UIItemShow showPanel;

        private void Start()
        {
            tmps = ItemTemplateStore.Instance.FindContainsPart("Tags", " MainModel", ';');//获取基础模具库
        }

        #region 界面管理

        public void Open()
        {
            state = Status.MainTMP;
            gameObject.SetActive(true);
            print("打开模具选择窗口");
        }

        public void Close()
        {
            state = Status.Closed;
            gameObject.SetActive(false);
            print("关闭模具选择窗口");
        }

        /// <summary>
        /// 左翻页
        /// </summary>
        public void LeftPage()
        {
            print("左翻页");
        }

        /// <summary>
        /// 右翻页
        /// </summary>
        public void RightPage()
        {
            print("右翻页");
        }

        private void ToSelectTemplate(ItemTemplate tmp)
        {
        }

        private void ToMake()
        {
        }

        #endregion 界面管理
    }
}