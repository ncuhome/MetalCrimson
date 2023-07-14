using ER.Items;
using System;
using UnityEngine;

namespace UI.SelectTemplate
{
    /// <summary>
    /// 用于选择模具界面的 物品按钮选项
    /// </summary>
    public class UIItemSelect : MonoBehaviour
    {
        /// <summary>
        /// 所关联的物品模板
        /// </summary>
        public ItemTemplate tmp;

        /// <summary>
        /// 所在面板对象
        /// </summary>
        public UITemplateSelectPanel owner;

        public Action<ItemTemplate> callback;

        #region 鼠标监听

        public void OnMouseDown()
        {
            print("选项按下");
            callback.Invoke(tmp);
        }

        public void OnMouseOver()
        {
            print("选项高亮");
            if (owner != null)
            {
                owner.showPanel.UpdateTemplate(tmp);
                owner.showPanel.Show();
            }
        }

        public void OnMouseExit()
        {
            print("选项离开");
            if (owner != null)
            {
                owner.showPanel.Hide();
            }
        }

        #endregion 鼠标监听

        private void Update()
        {
            //print("正在更新:"+ Input.mousePosition);
        }
    }
}