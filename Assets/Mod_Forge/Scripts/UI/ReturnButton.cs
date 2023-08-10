using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ReturnButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Return()
    {
        if (HammeringSystem.Instance && (HammeringSystem.Instance.AddedMaterialNum > 0) && (HammeringSystem.Instance.temperature == 0))
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            HammeringSystem.Instance.materialInFurnaces[HammeringSystem.Instance.AddedMaterialNum - 1].GetComponent<materialInFurnace>().OnPointerClick(eventData);
        }

        if (States.StateSystemManager.Instance.Exist("TypeStateSystem") && (States.StateSystemManager.Instance["TypeStateSystem"].currentState.ID == 2) && (!TypeSystem.Instance.moving))
        {
            TypeSystem.Instance.stateSystem[2].ChangeExitJudgement(1, true);
        }
    }
}
