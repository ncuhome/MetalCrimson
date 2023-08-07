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

    public void returnButton()
    {
        if (HammeringSystem.Instance && (HammeringSystem.Instance.AddedMaterialNum > 0) && (HammeringSystem.Instance.temperature == 0))
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            HammeringSystem.Instance.materialInFurnaces[HammeringSystem.Instance.AddedMaterialNum - 1].GetComponent<materialInFurnace>().OnPointerClick(eventData);
        }

        if (TypeSystem.Instance && (TypeSystem.Instance.chooseType == ChooseTypeEnum.FirstLevelEnd))
        {
            TypeSystem.Instance.chooseType = ChooseTypeEnum.WaitForBegin;
            TypeSystem.Instance.HideChildModels();
        }
    }
}
