using UnityEngine;
using UnityEngine.EventSystems;

public class LastPage : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (TypeSystem.Instance.stateSystem.currentState.ID)
        {
            case 1:
                TypeSystem.Instance.index--;
                TypeSystem.Instance.RefreshTypes();
                break;

            case 2:
                TypeSystem.Instance.childIndex--;
                TypeSystem.Instance.RefreshTypes();
                break;
        }
    }
}