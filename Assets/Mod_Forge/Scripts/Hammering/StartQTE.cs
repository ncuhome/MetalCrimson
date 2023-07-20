using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class StartQTE : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (HammeringSystem.Instance.startHammering) { QTE.Instance.StartQTE(); }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (HammeringSystem.Instance.startHammering) { HammeringSystem.Instance.HammerMaterial(); }
    }
}
