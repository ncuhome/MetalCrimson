using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowMoreButton : MonoBehaviour,IPointerClickHandler
{
    /// <summary>
    /// 选择界面的Transform组件
    /// </summary>
    public Transform chooseMaterialTransform = null;
    /// <summary>
    /// 材料组对应的Layout组件
    /// </summary>
    public GridLayoutGroup materialLayout = null;
    /// <summary>
    /// 目标坐标
    /// </summary>
    private Vector3 targetVec;
    /// <summary>
    /// 原坐标
    /// </summary>
    private Vector3 oldVec;
    /// <summary>
    /// 是否正在移动
    /// </summary>
    public bool move = false;
    /// <summary>
    /// 移动时间
    /// </summary>
    private float moveTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //向目标坐标移动
    void Update()
    {
        if (Vector3.Magnitude(chooseMaterialTransform.localPosition - targetVec) < 0.5f)
        {
            chooseMaterialTransform.localPosition = targetVec;
            moveTime = 0f;
            move = false;
        }
        else
        {
            move  = true;
            moveTime += Time.deltaTime * 5;
            Vector3 newVec = Vector3.Lerp(oldVec,targetVec,moveTime);
            chooseMaterialTransform.localPosition = newVec;
        }
    }
    /// <summary>
    /// 点击时运行，判定是展开还是合上
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("ShowMoreButton Clicked");
        oldVec = chooseMaterialTransform.localPosition;
        if (MaterialChooseSystem.Instance.showMore)
        {
            targetVec = Vector3.zero;
            materialLayout.constraintCount = 1;
            MaterialChooseSystem.Instance.showMore = false;
        }
        else
        {
            targetVec = new Vector3(0,616,0);
            materialLayout.constraintCount = 3;
            MaterialChooseSystem.Instance.showMore = true;
        }
    }
}
