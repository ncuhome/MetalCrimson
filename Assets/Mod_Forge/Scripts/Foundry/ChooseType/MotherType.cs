using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
public class MotherType : MonoBehaviour, IPointerClickHandler
{
    public Image typeImage;
    public TextMeshProUGUI typeText;
    public bool startMove;
    public Vector3 targetVec;
    public Vector3 oldVec;
    public float time;
    public int id;
    public int targetID;
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
                startMove = false;
                if (targetID != id)
                {
                    transform.gameObject.SetActive(false);
                    return;
                }
                TypeSystem.Instance.moving = false;
                if (TypeSystem.Instance.stateSystem.currentState.ID == 2)
                {
                    TypeSystem.Instance.ShowChildModels(id);
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (TypeSystem.Instance.moving) { return; }
        switch (TypeSystem.Instance.stateSystem.currentState.ID)
        {
            case 1:
                Action<int> action = MotherModelStateExit;
                TypeSystem.Instance.stateSystem[1].ChangeExitAction(action);
                TypeSystem.Instance.stateSystem[1].ChangeExitJudgement(2, true);
                break;
            case 2:
                TypeSystem.Instance.stateSystem[2].ChangeExitJudgement(1, true);
                break;
        }
    }

    public void MotherModelStateExit(int targetID)
    {
        switch (targetID)
        {
            case 2:
                TypeSystem.Instance.AllMoveTo(TypeSystem.Instance.GetPosition(0), id);
                break;
        }
    }

}
