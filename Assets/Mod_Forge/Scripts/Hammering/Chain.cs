using UnityEngine;
using UnityEngine.EventSystems;

public class Chain : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// 锁链对应图片
    /// </summary>
    private RectTransform rectTransform;

    /// <summary>
    /// 鼠标坐标与原坐标差
    /// </summary>
    private Vector3 posDiff;

    /// <summary>
    /// 原坐标
    /// </summary>
    private Vector3 lastPos;

    /// <summary>
    /// 拖动后的坐标y分量
    /// </summary>
    private float newPosY;

    private float lastPosY;

    // Start is called before the first frame update
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// 拖动开始，记录原坐标，计算坐标差
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (HammeringSystem.Instance.startHammering) { return; }
        //屏幕坐标转世界坐标
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.enterEventCamera, out posDiff);

        posDiff -= rectTransform.position;
        lastPos = rectTransform.position;
        lastPosY = newPosY;
    }

    /// <summary>
    /// 拖动，跟随拖动并且减去坐标差，且控制拖动范围，只允许改变y轴
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        if (HammeringSystem.Instance.startHammering) { return; }
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.enterEventCamera, out pos);

        newPosY = pos.y - posDiff.y;
        if (newPosY - lastPos.y > 0)
        {
            newPosY = lastPos.y;
        }
        if (newPosY - lastPos.y < -20)
        {
            newPosY = lastPos.y - 20;
        }

        rectTransform.position = new Vector3(rectTransform.position.x, newPosY, rectTransform.position.z);

        IncreaseTemperature();
    }

    /// <summary>
    /// 拖动结束，自动弹回原坐标
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (HammeringSystem.Instance.startHammering) { return; }
        rectTransform.position = lastPos;
    }

    private void IncreaseTemperature()
    {
        if (HammeringSystem.Instance.AddedMaterialNum == 0) { return; }
        float chainTimes = (lastPosY - newPosY) / 20;

        if (newPosY < lastPosY)
        {
            if (HammeringSystem.Instance.temperature <= 150)
            {
                HammeringSystem.Instance.temperature += chainTimes * 25f;
            }
            else if (HammeringSystem.Instance.temperature <= 300f)
            {
                HammeringSystem.Instance.temperature += chainTimes * 12.5f;
            }
            else if (HammeringSystem.Instance.temperature <= 400f)
            {
                HammeringSystem.Instance.temperature += chainTimes * 10f;
            }
            else if (HammeringSystem.Instance.temperature <= 500f)
            {
                HammeringSystem.Instance.temperature += chainTimes * 5f;
            }
        }
        lastPosY = newPosY;
    }
}