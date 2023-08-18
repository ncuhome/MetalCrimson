using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ModelBack : MonoBehaviour
{
    public Image image;
    public bool startMove;
    public Vector3 targetVec;
    public Vector3 oldVec;
    public float time;

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
                TypeSystem.Instance.moving = false;
                if (TypeSystem.Instance.stateSystem.currentState.ID == 4)
                {
                    StartCoroutine(WaitForExit());
                }
            }
        }
        if (startColor)
        {
            colorTime += Time.deltaTime;
            oldColor = new Color(image.color.r, image.color.g, image.color.b, oldAlpha);
            targetColor = new Color(image.color.r, image.color.g, image.color.b, targetAlpha);
            image.color = Color.Lerp(oldColor, targetColor, colorTime * 2.5f);
            if (colorTime > 0.4f)
            {
                image.color = targetColor;
                startColor = false;
                colorTime = 0;
            }
        }
    }

    IEnumerator WaitForExit()
    {
        yield return new WaitForSeconds(1f);
        Action<int> action2 = TypeSystem.Instance.GetComponentExit;
        TypeSystem.Instance.stateSystem[4].ChangeExitAction(action2);
        TypeSystem.Instance.stateSystem.states[4].ChangeExitJudgement(3, true);
    }
}
