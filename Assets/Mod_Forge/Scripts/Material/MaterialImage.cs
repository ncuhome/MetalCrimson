using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MaterialImage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// 对应的图片Transform组件
    /// </summary>
    private RectTransform rectTransform;
    /// <summary>
    /// 是否进入高炉判定区
    /// </summary>
    private bool inFurnace = false;
    /// <summary>
    /// 回归坐标
    /// </summary>
    private Vector3 lastPosition;
    /// <summary>
    /// 对应的材料脚本
    /// </summary>
    public MaterialScript materialScript = null;
    /// <summary>
    /// 是否能进行拖动
    /// </summary>
    public bool canBeDrag = true;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    /// <summary>
    /// 开始拖动，记录回归坐标
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canBeDrag) { return; }
        lastPosition = rectTransform.position;
        Debug.Log("开始拖拽");
    }
    /// <summary>
    /// 进行拖动
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        if (!canBeDrag) { return; }
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.enterEventCamera, out pos);
        rectTransform.position = pos;
    }
    /// <summary>
    /// 结束拖动，判断是否在判定区内，如果是则进行添加材料
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canBeDrag) { return; }
        Debug.Log("结束拖拽");
        rectTransform.position = lastPosition;
        if (inFurnace)
        {
            HammeringSystem.Instance.AddMaterialJudgement(materialScript);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BlastFurnace")
        {
            Debug.Log("进入");
            inFurnace = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "BlastFurnace")
        {
            Debug.Log("退出");
            inFurnace = false;
        }
    }
}
