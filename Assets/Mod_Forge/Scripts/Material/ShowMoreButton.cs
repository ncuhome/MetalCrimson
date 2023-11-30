using UnityEngine;
using UnityEngine.EventSystems;

public class ShowMoreButton : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    //向目标坐标移动
    private void Update()
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