using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class LineScript : MonoBehaviour, IPointerClickHandler
{
    public float Length;
    public TMP_Text LengthText;
    public RectTransform LineImageTrans;
    public ER.Items.ItemVariable LineItem = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void RefreshInfo()
    {
        Length = LineItem.GetFloat("Length");
        LengthText.text = Length.ToString() + "M";
    }
}
