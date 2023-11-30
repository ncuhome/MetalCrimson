using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChildModelType : MonoBehaviour, IPointerClickHandler
{
    public Image typeImage;
    public TextMeshProUGUI typeText;
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

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
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
            if (colorTime > 0.4f)
            {
                typeImage.color = targetColor;
                typeText.color = targetColor;
                startColor = false;
                colorTime = 0;
            }
        }
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
}