using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
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
                }
                TypeSystem.Instance.moving = false;
                if (TypeSystem.Instance.stateSystem.currentState.ID == 1)
                {
                    TypeSystem.Instance.MotherModelMoveBack();
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }
}