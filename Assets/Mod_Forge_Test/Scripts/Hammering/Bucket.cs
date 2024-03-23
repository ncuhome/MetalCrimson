﻿using UnityEngine;
using UnityEngine.EventSystems;

public class Bucket : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private RectTransform rectTransform;
    private bool inFurnace = false;
    private Vector3 lastPosition;

    //Start is called before the first frame update
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (HammeringSystem.Instance.startHammering) { return; }
        if (HammeringSystem.Instance.temperature == 0) { return; }
        lastPosition = rectTransform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (HammeringSystem.Instance.startHammering) { return; }
        if (HammeringSystem.Instance.temperature == 0) { return; }
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.enterEventCamera, out pos);
        rectTransform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (HammeringSystem.Instance.startHammering) { return; }
        if (HammeringSystem.Instance.temperature == 0) { return; }
        rectTransform.position = lastPosition;
        if (inFurnace)
        {
            HammeringSystem.Instance.temperature -= 50;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "FBlastFurnace")
        {
            Debug.Log("进入");
            inFurnace = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "FBlastFurnace")
        {
            Debug.Log("退出");
            inFurnace = false;
        }
    }
}