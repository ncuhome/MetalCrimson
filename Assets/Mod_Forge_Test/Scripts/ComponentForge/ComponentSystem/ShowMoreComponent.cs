using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
public class ShowMoreComponent : MonoBehaviour, IPointerClickHandler
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
    /// 点击时运行，判定是展开还是合上
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("ShowMoreButton Clicked");
        ComponentSystem.Instance.ShowMoreButtonClick();
    }
}
