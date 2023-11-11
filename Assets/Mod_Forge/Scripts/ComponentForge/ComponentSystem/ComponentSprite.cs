using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class ComponentSprite : MonoBehaviour
{
    /// <summary>
    /// 对应的图片Transform组件
    /// </summary>
    private Transform spriteTransform;
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
    public Vector3 cursorOffset;
    public bool dragFromOtherScript;
    // Start is called before the first frame update
    void Start()
    {
        spriteTransform = GetComponent<Transform>();
    }

    public void Update()
    {
        if (dragFromOtherScript)
        {
            OnMouseDrag();
            if (Input.GetMouseButtonUp(0))
            {
                OnMouseUp();
            }
        }
    }
    /// <summary>
    /// 开始拖动
    /// </summary>

    public void OnMouseDown()
    {
        if (!canBeDrag || ComponentChooseSystem.Instance.showMore) { return; }
        Debug.Log("Down");
        if (!startDrag)
        {
            StartDragEvent();
        }
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButton(1))
        {
            MoveBack();
        }
    }

    public void StartDragEvent()
    {
        Debug.Log("开始拖拽");
        cursorOffset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        startDrag = true;
        if (ComponentSystem.Instance.componentInAnvil.Exists(componentInAnvil => componentInAnvil == componentScript))
        {
            Offset();
        }
        else
        {
            RefreshPrompt();
        }
    }

    /// <summary>
    /// 进行拖动
    /// </summary>
    public void OnMouseDrag()
    {
        if (!canBeDrag || ComponentChooseSystem.Instance.showMore) { return; }
        Debug.Log("Drag");
        if (Input.GetMouseButton(0))
        {

            Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = cursorPos - cursorOffset;

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
    }

    /// <summary>
    /// 结束拖动，判断是否在判定区内，如果是则进行添加材料
    /// </summary>
    public void OnMouseUp()
    {
        if (!canBeDrag || ComponentChooseSystem.Instance.showMore) { return; }
        startDrag = false;
        dragFromOtherScript = false;
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
    }




    public void MoveBack()
    {
        Debug.Log("MoveBack");
        componentScript.componentImage.MoveBack();
        Destroy(this.gameObject);
    }

    public void AddComponent()
    {
        Debug.Log("AddComponent");
        spriteTransform.SetParent(ComponentSystem.Instance.AnvilTrans);
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
        Debug.Log("SplicingJudgeMent");
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
        Debug.Log("Offset");
        for (int i = 0; i < ComponentSystem.Instance.componentInAnvil.Count; i++)
        {
            if (ComponentSystem.Instance.componentInAnvil[i] != componentScript)
            {
                ComponentSprite componentSprite = ComponentSystem.Instance.componentInAnvil[i].componentSprite;
                componentSprite.offset = componentSprite.transform.position - transform.position;
            }
        }
    }

    public void MoveTogether()
    {
        Debug.Log("MoveTogether");
        for (int i = 0; i < ComponentSystem.Instance.componentInAnvil.Count; i++)
        {
            if (ComponentSystem.Instance.componentInAnvil[i] != componentScript)
            {
                ComponentSprite componentSprite = ComponentSystem.Instance.componentInAnvil[i].componentSprite;
                componentSprite.transform.position = transform.position + componentSprite.offset;
            }
        }
    }
}
