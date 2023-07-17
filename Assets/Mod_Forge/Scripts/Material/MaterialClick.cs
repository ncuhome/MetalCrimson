using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MaterialClick : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 点击材料时移动至材料所在位置
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        int index = transform.parent.GetSiblingIndex();
        if (MaterialChooseSystem.Instance.showMore)
        {
            MaterialChooseSystem.Instance.showMoreButton.OnPointerClick(eventData);
        }
        if ((float)index / (transform.parent.parent.childCount - 6) > 1)
        {
            MaterialChooseSystem.Instance.targetValue = 1f;
        }
        else
        {
            MaterialChooseSystem.Instance.targetValue = (float)index / (transform.parent.parent.childCount - 6);
        }
        MaterialChooseSystem.Instance.sliderDrag = false;
    }
}
