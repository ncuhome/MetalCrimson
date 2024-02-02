using System.Collections;
using TMPro;
using UnityEngine;

namespace ER.UI
{
    internal delegate void DelTest(string parma);

    public class TxtBubble : MonoBehaviour
    {
        #region 组件

        public RectTransform box;//大小控制组件
        public RectTransform trf;//比例控制组件
        public TMP_Text text;//文本

        #endregion 组件

        #region 属性

        /// <summary>
        /// 打印间隔
        /// </summary>
        public float print_cd = 0.25f;

        /// <summary>
        /// 单行最大长度限制
        /// </summary>
        public int max_row = 20;

        /// <summary>
        /// 动画播放速度
        /// </summary>
        public float animation_speed = 6;

        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color color = Color.black;

        /// <summary>
        /// 文本缓存
        /// </summary>
        private string txtTemp;

        private enum panel
        { closed, openning, opened, closing }

        private panel status = panel.closed;
        private int charIndex = 0;//打字机字符索引

        #endregion 属性

        #region 回调委托

        private void Opened()
        {
            status = panel.opened;
            text.gameObject.SetActive(true);
            text.color = color;
        }

        private void Closed()
        {
            status = panel.closed;
            gameObject.SetActive(false);
        }

        #endregion 回调委托

        #region 功能函数

        private void AutoSize()
        {
            Debug.Log(new Vector2(Mathf.Min(max_row, text.preferredWidth) + 10, text.preferredHeight + 10));
            if (max_row < text.preferredWidth)
            {
                box.sizeDelta = new Vector2(max_row + 10, text.preferredHeight + 10);
            }
            else
            {
                box.sizeDelta = new Vector2(text.preferredWidth + 10, text.preferredHeight / 2 + 10);
            }
        }

        /// <summary>
        /// 打开面板
        /// </summary>
        public void OpenPanel()
        {
            UIAnimator.Instance.AddAnimation(trf, UIAnimator.AnimationType.BoxOpen_Bottom, Opened);
            UIAnimator.Instance.GetAnimationInfo(trf).speed = animation_speed;
            UIAnimator.Instance.StartAnimation(trf);
            charIndex = 0;
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
            text.gameObject.SetActive(false);
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        public void ClosePanel()
        {
            text.color = new Color(0, 0, 0, 0);
            UIAnimator.Instance.AddAnimation(trf, UIAnimator.AnimationType.BoxClose_Top, Closed);
            UIAnimator.Instance.GetAnimationInfo(trf).speed = animation_speed;
            UIAnimator.Instance.StartAnimation(trf);
        }

        /// <summary>
        /// 添加显示文本
        /// </summary>
        public void AddTxt(string txt)
        {
            txtTemp += txt;
            StartCoroutine(PrintAnimation());
        }

        public void Clear()
        {
            text.text = txtTemp = "";
            charIndex = 0;
            AutoSize();
        }

        private IEnumerator PrintAnimation()
        {
            while (text.text.Length < txtTemp.Length)
            {
                text.text += txtTemp[charIndex++];
                AutoSize();
                yield return new WaitForSeconds(print_cd);
            }
        }

        #endregion 功能函数

        #region Unity

        private void Update()
        {
        }

        #endregion Unity
    }
}