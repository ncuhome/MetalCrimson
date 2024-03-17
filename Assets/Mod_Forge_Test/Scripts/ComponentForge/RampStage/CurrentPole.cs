using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CurrentPole : MonoBehaviour
{
    public int diameter = 60;
    public RectTransform rectTransform;
    public Animator animator;
    public Limiter limiter;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("StayOut"))
        {
            MoveIn();
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("LimiterIn"))
        {
            if (!limiter.startMove)
            {
                animator.SetTrigger("LimiterIn");
                limiter.oldVec = limiter.transform.localPosition;
                limiter.targetVec = limiter.transform.localPosition - new Vector3(0, 270, 0);
                limiter.startMove = true;
            }
        }
    }

    public void MoveOut()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) { return; }
        if (limiter.startMove) { return; }
        limiter.oldVec = limiter.transform.localPosition;
        limiter.targetVec = limiter.transform.localPosition + new Vector3(0, 270, 0);
        limiter.startMove = true;
        animator.SetTrigger("Start");
    }

    public void MoveIn()
    {
        rectTransform.localScale = new Vector3(1, diameter / 60f, 1);
        animator.SetTrigger("PoleIn");
    }
}
