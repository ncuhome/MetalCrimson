using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ComponentImage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    /// <summary>
    /// 对应的图片Transform组件
    /// </summary>
    private RectTransform rectTransform;
    /// <summary>
    /// 回归坐标
    /// </summary>
    private Vector3 lastPosition;
    /// <summary>
    /// 对应的材料脚本
    /// </summary>
    public ComponentScript componentScript = null;
    /// <summary>
    /// 是否能进行拖动
    /// </summary>
    public bool canBeDrag = true;
    /// <summary>
    /// 是否开始拖动
    /// </summary>
    public bool startDrag = false;
    /// <summary>
    /// 是否进入拼接区域
    /// </summary>
    public bool inAnvil = false;

    public bool canSplicing = false;

    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        lastPosition = rectTransform.localPosition;
    }

    public void Update()
    {

    }
    /// <summary>
    /// 开始拖动，记录回归坐标
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canBeDrag || ComponentChooseSystem.Instance.showMore) { return; }
        if (eventData.button != PointerEventData.InputButton.Left) { return; }
        if (ComponentSystem.Instance.componentInAnvil.Exists(componentInAnvil => componentInAnvil == componentScript))
        {
            Offset();
        }
        else
        {
            rectTransform.eulerAngles = new Vector3(0, 0, 45);
            rectTransform.localScale = new Vector3(1.5f, 1.5f, 1);
            RefreshPrompt();
        }
        startDrag = true;
        Debug.Log("开始拖拽");
    }
    /// <summary>
    /// 进行拖动
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        if (!canBeDrag || ComponentChooseSystem.Instance.showMore) { return; }
        if (eventData.button != PointerEventData.InputButton.Left) { return; }
        rectTransform.eulerAngles = new Vector3(0, 0, 45);
        rectTransform.localScale = new Vector3(1.5f, 1.5f, 1);
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.enterEventCamera, out pos);
        rectTransform.position = pos;

        if (ComponentSystem.Instance.componentInAnvil.Exists(componentInAnvil => componentInAnvil == componentScript))
        {
            MoveTogether();
        }
        else
        {
            if (inAnvil)
            {
                SplicingJudgeMent();
            }
        }
    }
    /// <summary>
    /// 结束拖动，判断是否在判定区内，如果是则进行添加材料
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canBeDrag || ComponentChooseSystem.Instance.showMore) { return; }
        if (eventData.button != PointerEventData.InputButton.Left) { return; }
        startDrag = false;
        Debug.Log("结束拖拽");
        RefreshPrompt();

        if (ComponentSystem.Instance.componentInAnvil.Exists(componentInAnvil => componentInAnvil == componentScript)) { return; }
        if (!inAnvil)
        {
            MoveBack();
        }
        else
        {
            if (canSplicing)
            {

                AddComponent();
            }
            else
            {
                MoveBack();
            }
        }

        ComponentSystem.Instance.inLink = null;
        ComponentSystem.Instance.outLink = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            MoveBack();
        }
    }

    public void MoveBack()
    {
        rectTransform.SetParent(componentScript.transform);
        rectTransform.localPosition = lastPosition;
        rectTransform.eulerAngles = new Vector3(0, 0, 0);
        rectTransform.localScale = Vector3.one;
        ComponentSystem.Instance.RemoveComponentFromAnvil(componentScript);
    }

    public void AddComponent()
    {
        rectTransform.SetParent(ComponentSystem.Instance.AnvilTrans);
        if (ComponentSystem.Instance.inLink)
        {
            if (ComponentSystem.Instance.inLink == componentScript.inPrompt)
            {
                transform.position = ComponentSystem.Instance.outLink.transform.position + transform.position - componentScript.inPrompt.transform.position;
            }
            else
            {
                transform.position = ComponentSystem.Instance.inLink.transform.position + transform.position - componentScript.outPrompt.transform.position;
            }
        }
        ComponentSystem.Instance.AddComponentToAnvil(componentScript);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Anvil")
        {
            inAnvil = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Anvil")
        {
            inAnvil = false;
        }
    }

    public void RefreshPrompt()
    {
        if (ComponentSystem.Instance.inPort) { ComponentSystem.Instance.inPort.Hide(); }
        if (ComponentSystem.Instance.outPort) { ComponentSystem.Instance.outPort.Hide(); }
        if (componentScript.inPrompt) { componentScript.inPrompt.Hide(); }
        if (componentScript.outPrompt) { componentScript.outPrompt.Hide(); }
        LineManager.Insatnce.DrawLine(Vector3.zero, Vector3.zero);
    }

    public void SplicingJudgeMent()
    {
        if (ComponentSystem.Instance.componentInAnvil.Count == 0)
        {
            canSplicing = true;
            return;
        }
        if ((!ComponentSystem.Instance.inPort) && (!ComponentSystem.Instance.outPort)) { canSplicing = false; return; }

        if ((componentScript.inNum != 0) && (componentScript.outNum == 0))
        {
            if (ComponentSystem.Instance.outPort)
            {
                if ((componentScript.inNum == ComponentSystem.Instance.outPort.componentScript.outNum) || (componentScript.inNum == -1) || (ComponentSystem.Instance.outPort.componentScript.outNum == -1))
                {
                    ComponentSystem.Instance.outPort.Correct();
                    componentScript.inPrompt.Correct();
                    ComponentSystem.Instance.outLink = ComponentSystem.Instance.outPort;
                    ComponentSystem.Instance.inLink = componentScript.inPrompt;
                    LineManager.Insatnce.DrawLine(ComponentSystem.Instance.outPort.transform.position, componentScript.inPrompt.transform.position);
                    canSplicing = true;
                }
                else
                {
                    ComponentSystem.Instance.outPort.Error();
                    componentScript.inPrompt.Error();
                    canSplicing = false;
                }
            }
            else
            {
                ComponentSystem.Instance.inPort.Error();
                componentScript.inPrompt.Error();
                canSplicing = false;
            }
        }

        if ((componentScript.inNum == 0) && (componentScript.outNum != 0))
        {
            if (ComponentSystem.Instance.inPort)
            {
                if ((componentScript.outNum == ComponentSystem.Instance.inPort.componentScript.inNum) || (componentScript.outNum == -1) || (ComponentSystem.Instance.inPort.componentScript.inNum == -1))
                {
                    ComponentSystem.Instance.inPort.Correct();
                    componentScript.outPrompt.Correct();
                    ComponentSystem.Instance.inLink = ComponentSystem.Instance.inPort;
                    ComponentSystem.Instance.outLink = componentScript.outPrompt;
                    LineManager.Insatnce.DrawLine(ComponentSystem.Instance.inPort.transform.position, componentScript.outPrompt.transform.position);
                    canSplicing = true;
                }
                else
                {
                    ComponentSystem.Instance.inPort.Error();
                    componentScript.outPrompt.Error();
                    canSplicing = false;
                }
            }
            else
            {
                ComponentSystem.Instance.outPort.Error();
                componentScript.outPrompt.Error();
                canSplicing = false;
            }
        }

        if ((componentScript.inNum != 0) && (componentScript.outNum != 0))
        {
            if (ComponentSystem.Instance.inPort)
            {
                if ((componentScript.outNum == ComponentSystem.Instance.inPort.componentScript.inNum) || (componentScript.outNum == -1) || (ComponentSystem.Instance.inPort.componentScript.inNum == -1))
                {
                    ComponentSystem.Instance.inPort.Correct();
                    componentScript.outPrompt.Correct();
                    ComponentSystem.Instance.inLink = ComponentSystem.Instance.inPort;
                    ComponentSystem.Instance.outLink = componentScript.outPrompt;
                    LineManager.Insatnce.DrawLine(ComponentSystem.Instance.inPort.transform.position, componentScript.outPrompt.transform.position);
                    canSplicing = true;
                }
                else
                {
                    ComponentSystem.Instance.inPort.Error();
                    componentScript.outPrompt.Error();
                    canSplicing = false;
                }
                return;
            }
            if (ComponentSystem.Instance.outPort)
            {
                if ((componentScript.inNum == ComponentSystem.Instance.outPort.componentScript.outNum) || (componentScript.inNum == -1) || (ComponentSystem.Instance.outPort.componentScript.outNum == -1))
                {
                    ComponentSystem.Instance.outPort.Correct();
                    componentScript.inPrompt.Correct();
                    ComponentSystem.Instance.outLink = ComponentSystem.Instance.outPort;
                    ComponentSystem.Instance.inLink = componentScript.inPrompt;
                    LineManager.Insatnce.DrawLine(ComponentSystem.Instance.outPort.transform.position, componentScript.inPrompt.transform.position);
                    canSplicing = true;
                }
                else
                {
                    ComponentSystem.Instance.outPort.Error();
                    componentScript.inPrompt.Error();
                    canSplicing = false;
                }
                return;
            }
        }
    }


    public void Offset()
    {
        for (int i = 0; i < ComponentSystem.Instance.componentInAnvil.Count; i++)
        {
            if (ComponentSystem.Instance.componentInAnvil[i] != componentScript)
            {
                ComponentImage componentImage = ComponentSystem.Instance.componentInAnvil[i].componentImage.GetComponent<ComponentImage>();
                componentImage.offset = componentImage.transform.position - transform.position;
            }
        }
    }

    public void MoveTogether()
    {
        for (int i = 0; i < ComponentSystem.Instance.componentInAnvil.Count; i++)
        {
            if (ComponentSystem.Instance.componentInAnvil[i] != componentScript)
            {
                ComponentImage componentImage = ComponentSystem.Instance.componentInAnvil[i].componentImage.GetComponent<ComponentImage>();
                componentImage.transform.position = transform.position + componentImage.offset;
            }
        }
    }

}