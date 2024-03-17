using UnityEngine;
using UnityEngine.EventSystems;

public class StartHammeringClick : MonoBehaviour, IPointerDownHandler
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
        HammeringSystem.Instance.StartHammering();
    }
}