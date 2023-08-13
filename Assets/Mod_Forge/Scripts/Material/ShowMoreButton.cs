using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowMoreButton : MonoBehaviour, IPointerClickHandler
{

    // Start is called before the first frame update
    void Start()
    {

    }

    //向目标坐标移动
    void Update()
    {

    }
    /// <summary>
    /// 点击时运行，判定是展开还是合上
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("ShowMoreButton Clicked");
        MaterialSystem.Instance.ShowMoreButtonClick();
    }
}
