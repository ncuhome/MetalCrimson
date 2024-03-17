using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ComponentScript : MonoBehaviour
{
    public ComponentImage componentImage = null;
    public ER.Items.ItemVariable ComponentItem = null;
    public ComponentSprite componentSprite = null;
    public LinkPrompt inPrompt = null;
    public LinkPrompt outPrompt = null;
    public int inNum;
    public int outNum;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        componentImage = GetComponentInChildren<ComponentImage>();
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
        inNum = ComponentItem.GetInt("InNum");
        outNum = ComponentItem.GetInt("OutNum");
        if (inPrompt)
        {
            inPrompt.GetComponent<Image>().enabled = false;
            inPrompt.componentScript = this;
            inPrompt.inPrompt = true;
        }
        if (outPrompt)
        {
            outPrompt.GetComponent<Image>().enabled = false;
            outPrompt.componentScript = this;
            outPrompt.inPrompt = false;
        }
        componentImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(ComponentItem.GetText("StaticAddress"));
        componentImage.GetComponent<Image>().SetNativeSize();
    }

}
