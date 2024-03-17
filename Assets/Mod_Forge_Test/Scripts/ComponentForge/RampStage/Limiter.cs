using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
public class Limiter : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool startMove;
    public Vector3 targetVec;
    public Vector3 oldVec;
    public float time;

    public Animator animator;

    public bool canDrag;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (startMove)
        {
            time += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(oldVec, targetVec, time * 2.5f);
            if (time > 0.4f)
            {
                transform.localPosition = targetVec;
                time = 0;
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("LimiterOut"))
                {
                    animator.SetTrigger("PoleOut");
                }
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("End"))
                {
                    animator.SetTrigger("End");
                }
                startMove = false;
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            canDrag = true;
        }
        else
        {
            canDrag = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canDrag) { Debug.Log("OnBeingDrag"); }

    }
    public void OnDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            Debug.Log("OnDrag");
            Vector2 pos;
            RectTransform rectTransform = UIManager.Instance.canvas.GetComponent<RectTransform>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.enterEventCamera, out pos);
            RampStageSystem.Instance.value = RampStageSystem.Instance.GetValue(pos.x);
            transform.localPosition = RampStageSystem.Instance.GetPos(RampStageSystem.Instance.value);
        }

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            RampStageSystem.Instance.value = RampStageSystem.Instance.GetNearValue(RampStageSystem.Instance.value);
            transform.localPosition = RampStageSystem.Instance.GetPos(RampStageSystem.Instance.value);
        }
    }


}


