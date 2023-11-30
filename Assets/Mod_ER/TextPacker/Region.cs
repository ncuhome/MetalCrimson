using ER.Control;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ER.TextPacker
{
    /// <summary>
    /// 接收区域
    /// </summary>
    public class Region : MonoBehaviour, IHandler
    {
        #region 鼠标控制

        private Image image;

        [SerializeField]
        [Tooltip("最大透明度")]
        private float maxAlpha = 0.5f;

        public bool coverable = false;//是否可覆盖, 用于区分大的 Region 和 排序的小Region

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            image.color = image.color.ModifyAlpha(maxAlpha);
            if (PackagePanel.Instance.catchRegion == null || PackagePanel.Instance.catchRegion.coverable)
            {
                PackagePanel.Instance.catchRegion = this;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            image.color = image.color.ModifyAlpha(0);
            if (PackagePanel.Instance.catchRegion == this)
            {
                PackagePanel.Instance.catchRegion = null;
            }
        }

        public void OnPointerMove(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }

        #endregion 鼠标控制

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            image.color = image.color.ModifyAlpha(0);
        }
    }
}