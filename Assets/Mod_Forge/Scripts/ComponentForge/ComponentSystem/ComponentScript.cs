using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ComponentScript : MonoBehaviour
{
    public TextMeshProUGUI componentNameText = null;
    public TextMeshProUGUI componentNumText = null;
    public Image componentImage = null;
    public ER.Items.ItemVariable ComponentItem = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    /// <summary>
    /// 刷新信息
    /// </summary>
    public void RefreshInfo()
    {
        if (ComponentItem == null) { return; }
        componentNameText.text = ComponentItem.GetText("Name");
        componentNumText.text = "拥有：" + ComponentItem.GetInt("Num");
    }
}
