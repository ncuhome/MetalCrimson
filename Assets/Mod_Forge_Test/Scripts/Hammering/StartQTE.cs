using UnityEngine;
using UnityEngine.EventSystems;

public class StartQTE : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (HammeringSystem.Instance.startHammering) { QTE.Instance.QTEJudgement(); }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (HammeringSystem.Instance.startHammering) { QTE.Instance.StartQTE(); }
    }
}