using States;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReturnButton : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Return()
    {
        if (HammeringSystem.Instance && (HammeringSystem.Instance.AddedMaterialNum > 0) && (HammeringSystem.Instance.chainTimes == 0))
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            HammeringSystem.Instance.materialInFurnaces[HammeringSystem.Instance.AddedMaterialNum - 1].GetComponent<materialInFurnace>().OnPointerClick(eventData);
        }

        if (StateSystemManager.Instance.Exist("TypeStateSystem") && (!TypeSystem.Instance.moving))
        {
            switch (StateSystemManager.Instance["TypeStateSystem"].currentState.ID)
            {
                case 1:
                    break;

                case 2:
                    Action<int> action1 = TypeSystem.Instance.ChildModelStateExit;
                    TypeSystem.Instance.stateSystem[2].ChangeExitAction(action1);
                    TypeSystem.Instance.stateSystem[2].ChangeExitJudgement(1, true);
                    break;

                case 3:
                    Action<int> action2 = TypeSystem.Instance.ChosenModelExit;
                    TypeSystem.Instance.stateSystem[3].ChangeExitAction(action2);
                    TypeSystem.Instance.stateSystem[3].ChangeExitJudgement(2, true);
                    break;
            }
        }
    }
}