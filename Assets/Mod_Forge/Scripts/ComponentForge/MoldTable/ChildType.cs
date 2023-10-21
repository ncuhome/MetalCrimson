using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
public class ChildModelType : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public Image typeImage;
    public TextMeshProUGUI typeText;
    public Image typeBackground;
    public bool startMove;
    public Vector3 targetVec;
    public Vector3 oldVec;
    public float time;
    public int id;
    public int motherId;
    public int targetID;

    public float oldAlpha;
    public float targetAlpha;
    public Color oldColor;
    public Color targetColor;
    public float colorTime;
    public bool startColor;
    public bool showChildModel;

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
                }
                TypeSystem.Instance.moving = false;
                if (TypeSystem.Instance.stateSystem.currentState.ID == 1)
                {
                    TypeSystem.Instance.MotherModelMoveBack();
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

        showChildModel = (typeImage.color.a != 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (TypeSystem.Instance.moving) { return; }
        if (TypeSystem.Instance.stateSystem.currentState.ID == 2)
        {
            TypeSystem.Instance.chosenChildModelID = id;
            TypeSystem.Instance.chosenChildModel = TypeSystem.Instance.GetChildType(TypeSystem.Instance.currentMotherModelID, id);
            Action<int> action = TypeSystem.Instance.ChildModelStateExit;
            TypeSystem.Instance.stateSystem[2].ChangeExitAction(action);
            TypeSystem.Instance.stateSystem.states[2].ChangeExitJudgement(3, true);
        }
    }

    public void SetAlpha(float a)
    {
        typeImage.color = new Color(typeImage.color.r, typeImage.color.g, typeImage.color.b, a);
        typeText.color = new Color(typeText.color.r, typeText.color.g, typeText.color.b, a);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!showChildModel) { return; }
        TypeSystem.Instance.ShowCard(TypeSystem.Instance.GetChildType(motherId, id));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!showChildModel) { return; }
        TypeSystem.Instance.HideCard();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!showChildModel) { return; }
        Vector2 pos;
        RectTransform rectTransform = UIManager.Instance.canvas.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.enterEventCamera, out pos);
        TypeSystem.Instance.MoveCard(pos + new Vector2(155, -190));
    }
}