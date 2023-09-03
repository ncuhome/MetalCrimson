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
    public Image typeBackground;
    public bool startMove;
    public Vector3 targetVec;
    public Vector3 oldVec;
    public float time;
    public int id;
    public int targetID;

    public float oldAlpha;
    public float targetAlpha;
    public Color oldColor;
    public Color targetColor;
    public float colorTime;
    public bool startColor;
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
                time = 0;
                if (targetID != id)
                {
                    transform.gameObject.SetActive(false);
                    return;
                }
                TypeSystem.Instance.moving = false;
                if (TypeSystem.Instance.stateSystem.currentState.ID == 2)
                {
                    switch (TypeSystem.Instance.stateSystem.lastState.ID)
                    {
                        case 1:
                            TypeSystem.Instance.ShowChildModels(id);
                            break;
                    }
                }
            }
        }

        if (startColor)
        {
            colorTime += Time.deltaTime;
            oldColor = new Color(typeImage.color.r, typeImage.color.g, typeImage.color.b, oldAlpha);
            targetColor = new Color(typeImage.color.r, typeImage.color.g, typeImage.color.b, targetAlpha);
            typeImage.color = Color.Lerp(oldColor, targetColor, colorTime * 2.5f);
            typeText.color = Color.Lerp(oldColor, targetColor, colorTime * 2.5f);
            typeBackground.color = Color.Lerp(oldColor, targetColor, colorTime * 2.5f);
            if (colorTime > 0.4f)
            {
                typeImage.color = targetColor;
                typeText.color = targetColor;
                typeBackground.color = targetColor;
                startColor = false;
                colorTime = 0;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (TypeSystem.Instance.moving) { return; }
        switch (TypeSystem.Instance.stateSystem.currentState.ID)
        {
            case 1:
                Action<int> action1 = MotherModelStateExit;
                TypeSystem.Instance.stateSystem[1].ChangeExitAction(action1);
                TypeSystem.Instance.stateSystem[1].ChangeExitJudgement(2, true);
                break;
            case 2:
                Action<int> action2 = TypeSystem.Instance.ChildModelStateExit;
                TypeSystem.Instance.stateSystem[2].ChangeExitAction(action2);
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

    public void SetAlpha(float a)
    {
        typeImage.color = new Color(typeImage.color.r, typeImage.color.g, typeImage.color.b, a);
        typeText.color = new Color(typeText.color.r, typeText.color.g, typeText.color.b, a);
    }

}
