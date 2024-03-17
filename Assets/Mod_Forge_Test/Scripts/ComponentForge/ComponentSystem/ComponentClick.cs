using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComponentClick : MonoBehaviour, IPointerClickHandler
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
        if (ComponentChooseSystem.Instance.showMore)
        {
            ComponentChooseSystem.Instance.showMoreButton.OnPointerClick(eventData);
        }
        if ((float)index / (transform.parent.parent.childCount - 5) > 1)
        {
            ComponentChooseSystem.Instance.targetValue = 1f;
        }
        else
        {
            ComponentChooseSystem.Instance.targetValue = (float)index / (transform.parent.parent.childCount - 5);
        }
        ComponentChooseSystem.Instance.sliderDrag = false;
    }
}
