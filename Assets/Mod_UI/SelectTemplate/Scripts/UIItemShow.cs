using ER.Items;
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
            tmp = _tmp;
        }
        /// <summary>
        /// 显示此面板
        /// </summary>
        public void Show()
        {
            animator.SetBool("show",true);
            gameObject.SetActive(true);
        }
        /// <summary>
        /// 隐藏此面板
        /// </summary>
        public void Hide()
        {
            animator.SetBool("show", false);
            gameObject.SetActive(false);
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