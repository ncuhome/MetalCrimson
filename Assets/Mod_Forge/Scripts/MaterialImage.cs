using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MaterialImage : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private RectTransform rectTransform;
    private bool inFurnace = false;
    private Vector3 lastPosition;
    public MaterialScript materialScript = null;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
 
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastPosition = rectTransform.position;
        Debug.Log("开始拖拽");
    }
 
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.enterEventCamera, out pos);
        rectTransform.position = pos;
    }
 
    public void OnEndDrag(PointerEventData eventData)
    {
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
