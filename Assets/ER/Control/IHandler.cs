using UnityEngine;
using UnityEngine.EventSystems;

namespace ER.Control
{
    /// <summary>
    /// UI 鼠标事件回调接口
    /// </summary>
    public interface IHandler : IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
        IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
    {
    }

    public class HandlerBase : MonoBehaviour, IHandler
    {
        public virtual void OnBeginDrag(PointerEventData eventData)
        { }

        public virtual void OnDrag(PointerEventData eventData)
        { }

        public virtual void OnEndDrag(PointerEventData eventData)
        { }

        public virtual void OnPointerClick(PointerEventData eventData)
        { }

        public virtual void OnPointerDown(PointerEventData eventData)
        { }

        public virtual void OnPointerEnter(PointerEventData eventData)
        { }

        public virtual void OnPointerExit(PointerEventData eventData)
        { }

        public virtual void OnPointerMove(PointerEventData eventData)
        { }

        public virtual void OnPointerUp(PointerEventData eventData)
        { }
    }
}