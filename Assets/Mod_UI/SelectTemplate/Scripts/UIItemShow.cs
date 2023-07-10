﻿using ER.Items;
using JetBrains.Annotations;
using UnityEngine;

namespace UI
{
    public class UIItemShow:MonoBehaviour
    {
        public Animator animator;
        private ItemTemplate tmp;

        /// <summary>
        /// 更新显示内容(待完善)
        /// </summary>
        /// <param name="_tmp">需要展示信息的物品模板</param>
        public void UpdateTemplate(ItemTemplate _tmp)
        {
            print("更新显示信息");
            tmp = _tmp;
        }
        /// <summary>
        /// 显示此面板
        /// </summary>
        public void Show()
        {
            print("显示信息面板");
            animator.SetBool("show",true);
        }
        /// <summary>
        /// 隐藏此面板
        /// </summary>
        public void Hide()
        {
            print("关闭信息面板");
            animator.SetBool("show", false);
            
        }
        /// <summary>
        /// 跟随鼠标移动
        /// </summary>
        private void Update()
        {
            transform.position = Input.mousePosition;
        }
    }
}