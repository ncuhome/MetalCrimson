// Ignore Spelling: Ptage

using ER.Control;
using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ER.UI
{
    public class ValueSliderBar : HandlerBase
    {
        #region 组件 |属性

        public TMP_Text text;
        public Slider slider;
        private float value = 0f;//0.0~1.0
        private bool textDisplay = true;

        /// <summary>
        /// 进度
        /// </summary>
        public float Value
        {
            get
            {
                return value;
            }
            set
            {
                slider.value = value;
                this.value = value;
            }
        }

        /// <summary>
        /// 显示的文本
        /// </summary>
        public string Text
        {
            get
            {
                return text.text;
            }
            set
            {
                text.text = value;
            }
        }

        /// <summary>
        /// 是否显示文本提示
        /// </summary>
        public bool TextDisplay
        {
            get
            {
                return textDisplay;
            }
            set
            {
                textDisplay = value;
                text.gameObject.SetActive(value);
            }
        }

        #endregion 组件 |属性

        #region 方法

        /// <summary>
        /// 取比值，返回一个 0.0~1.0 的数
        /// </summary>
        /// <param name="number1"></param>
        /// <param name="number2"></param>
        /// <returns></returns>
        public static float Ptage(int number1, int number2)
        {
            return (float)number1 / number2;
        }

        /// <summary>
        /// 取比值，返回一个 0.0~1.0 的数
        /// </summary>
        /// <param name="number1"></param>
        /// <param name="number2"></param>
        /// <returns></returns>
        public static float Ptage(float number1, float number2)
        {
            return number1 / number2;
        }

        #endregion 方法

        #region 委托|鼠标监控

        /// <summary>
        /// 被鼠标点击
        /// </summary>
        public event Action OnMouseClickEvent;

        /// <summary>
        /// 被鼠标进入
        /// </summary>
        public event Action OnMouseEnterEvent;

        /// <summary>
        /// 被鼠标离开
        /// </summary>
        public event Action OnMouseExitEvent;

        /// <summary>
        /// 被鼠标覆盖
        /// </summary>
        public event Action OnMouseCoverEvent;

        private bool enter = false;

        public override void OnPointerClick(PointerEventData eventData)
        {
            //print("鼠标点击");
            if (OnMouseClickEvent != null)
                OnMouseClickEvent();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            //print("鼠标离开");
            enter = false;
            if (OnMouseExitEvent != null)
                OnMouseExitEvent.Invoke();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            //print("鼠标进入");
            enter = true;
            if (OnMouseEnterEvent != null)
                OnMouseEnterEvent();
        }

        private void Update()
        {
            if (enter)
            {
                if (OnMouseCoverEvent != null)
                    OnMouseCoverEvent();
            }
        }

        #endregion 委托|鼠标监控
    }
}