using ER.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        public Animator animator;
        public Action<ItemTemplate> callback;

        #region 鼠标监听
        public void OnMouseDown()
        {
            callback(tmp);
        }

        public void OnMouseOver()
        {
            
        }
        #endregion
    }
}
