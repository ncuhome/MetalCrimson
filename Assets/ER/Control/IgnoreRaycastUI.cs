// Ignore Spelling: Raycast
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ER.Control
{
    /// <summary>
    /// 完全忽略鼠标动作的UI控件
    /// </summary>
    [Obsolete("该类有问题, 不要继承")]
    public class IgnoreRaycastUI : MonoBehaviour, IHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            eventData.PassEvent(ExecuteEvents.pointerEnterHandler);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            eventData.PassEvent(ExecuteEvents.pointerExitHandler);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            eventData.PassEvent(ExecuteEvents.pointerClickHandler);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            eventData.PassEvent(ExecuteEvents.beginDragHandler);
        }

        public void OnDrag(PointerEventData eventData)
        {
            eventData.PassEvent(ExecuteEvents.dragHandler);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            eventData.PassEvent(ExecuteEvents.endDragHandler);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            eventData.PassEvent(ExecuteEvents.pointerMoveHandler);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            eventData.PassEvent(ExecuteEvents.pointerDownHandler);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            eventData.PassEvent(ExecuteEvents.pointerUpHandler);
        }
    }
}