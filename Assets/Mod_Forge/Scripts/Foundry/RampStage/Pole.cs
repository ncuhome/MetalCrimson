using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class Pole : MonoBehaviour, IPointerClickHandler
{
    public int diameter;
    public TMP_Text diameterText;
    public RectTransform poleImageTrans;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        diameterText.text = diameter.ToString() + "mm";
        poleImageTrans.sizeDelta = new Vector2(diameter * 1.5f, diameter * 1.5f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }
}
