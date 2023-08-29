using UnityEngine;
using UnityEngine.EventSystems;

public class ShowOutLine : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Outline outline = null;

    // Start is called before the first frame update
    private void Start()
    {
        outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    /// <summary>
    /// 鼠标移入显示高亮
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.OutlineColor = new Color(outline.OutlineColor.r, outline.OutlineColor.g, outline.OutlineColor.b, 1f);
    }

    /// <summary>
    /// 鼠标移出取消高亮
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        outline.OutlineColor = new Color(outline.OutlineColor.r, outline.OutlineColor.g, outline.OutlineColor.b, 0);
    }
}