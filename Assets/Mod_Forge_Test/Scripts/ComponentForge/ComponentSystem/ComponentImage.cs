using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ComponentImage : MonoBehaviour, IPointerDownHandler
{

    /// <summary>
    /// 对应的材料脚本
    /// </summary>
    public ComponentScript componentScript = null;


    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Update()
    {

    }
 

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            InitSprite();
        }
    }


    public void InitSprite()
    {
        GetComponent<Image>().enabled = false;
        GameObject spritePrefab = Resources.Load<GameObject>("Prefabs/Components/" + componentScript.ComponentItem.GetText("NameTmp"));
        GameObject spriteObject = Instantiate(spritePrefab, ComponentSystem.Instance.AnvilTrans);
        spriteObject.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, spriteObject.transform.position.z);
        componentScript.componentSprite = spriteObject.GetComponent<ComponentSprite>();
        componentScript.componentSprite.componentScript = componentScript;
        componentScript.componentSprite.dragFromOtherScript = true;
        LinkPrompt[] linkPrompts = spriteObject.GetComponentsInChildren<LinkPrompt>();
        foreach(LinkPrompt linkPrompt in linkPrompts)
        {
            if (linkPrompt.inPrompt)
            {
                componentScript.inPrompt = linkPrompt;
            }
            else
            {
                componentScript.outPrompt = linkPrompt;
            }
            linkPrompt.componentScript = componentScript;
        }
        componentScript.componentSprite.StartDragEvent();
    }

    public void MoveBack()
    {
        GetComponent<Image>().enabled = true;
        Destroy(componentScript.componentSprite.gameObject);
        ComponentSystem.Instance.RemoveComponentFromAnvil(componentScript);
    }

}