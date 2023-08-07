using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum ChooseTypeEnum { WaitForBegin, FirstLevelMove, FirstLevelEnd, SecondLevelMove, SecondLevelEnd }

[System.Serializable]
public struct Type
{
    public string name;
    public Sprite typeSprite;
    public GameObject typeObject;
    public MotherType typeScript;
    public ChildType[] childTypes;
}
[System.Serializable]
public struct ChildType
{
    public string name;
    public Sprite typeSprite;
    public GameObject typeObject;
    public ChildModelType typeScript;
}
public class TypeSystem : MonoBehaviour
{

    private static TypeSystem instance;
    public static TypeSystem Instance
    {
        get
        {
            return instance;
        }
    }
    public int typeNum;
    public Type[] types;
    public GameObject motherModelPrefab, childModelPrefab;
    public Transform typeParentTrans;
    public int index = 1;
    public int childIndex = 1;
    public Vector2 CellSize;
    public Vector2 Spacing;
    public GameObject NextPageButton, LastPageButton;
    public States.StateSystem stateSystem;
    public int currentMotherModelID;
    public bool moving;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        InitState();
        InitTypes();
    }


    void InitState()
    {
        States.StateSystemManager.Instance.CreateStateSystem("TypeStateSystem");
        stateSystem = States.StateSystemManager.Instance["TypeStateSystem"];

        States.State showMotherModel = new States.State(1, "showMotherModel");
        showMotherModel.ChangeExitJudgement(2, false);
        stateSystem.AddState(showMotherModel);

        States.State showChildModel = new States.State(2, "showChildModel");
        showChildModel.ChangeExitJudgement(3, false);
        showChildModel.ChangeExitJudgement(1, false);
        Action<int> action = ChildModelStateExit;
        showChildModel.ChangeExitAction(action);
        stateSystem.AddState(showChildModel);

        States.State chooseMaterial = new States.State(3, "chooseMaterial");
        showChildModel.ChangeExitJudgement(2, false);
        stateSystem.AddState(chooseMaterial);
    }

    void InitTypes()
    {
        for (int i = 0; i < typeNum; i++)
        {
            InstantiateType(i);
        }
        index = 1;
        childIndex = 1;
    }

    void InstantiateType(int i)
    {
        types[i].typeObject = Instantiate(motherModelPrefab);
        types[i].typeObject.transform.parent = typeParentTrans;
        types[i].typeObject.transform.localScale = Vector3.one;
        types[i].typeScript = types[i].typeObject.GetComponent<MotherType>();
        types[i].typeScript.typeImage.sprite = types[i].typeSprite;
        types[i].typeScript.typeText.text = types[i].name;
        types[i].typeScript.id = i;
        types[i].typeObject.transform.localPosition = GetPosition(i);
        for (int j = 0; j < types[i].childTypes.Length; j++)
        {
            InstantiateChildType(i, j);
        }
        if (i >= 8) { types[i].typeObject.SetActive(false); }
    }

    void InstantiateChildType(int motherId, int childId)
    {
        types[motherId].childTypes[childId].typeObject = Instantiate(childModelPrefab);
        types[motherId].childTypes[childId].typeObject.transform.parent = typeParentTrans;
        types[motherId].childTypes[childId].typeObject.transform.localPosition = types[motherId].typeObject.transform.localPosition;
        types[motherId].childTypes[childId].typeObject.transform.localScale = Vector3.one;
        types[motherId].childTypes[childId].typeScript = types[motherId].childTypes[childId].typeObject.GetComponent<ChildModelType>();
        types[motherId].childTypes[childId].typeScript.typeImage.sprite = types[motherId].childTypes[childId].typeSprite;
        types[motherId].childTypes[childId].typeScript.typeText.text = types[motherId].childTypes[childId].name;
        types[motherId].childTypes[childId].typeScript.id = childId;
        types[motherId].childTypes[childId].typeScript.motherId = motherId;
        types[motherId].childTypes[childId].typeObject.SetActive(false);
    }

    public Vector2 GetPosition(int i)
    {
        if (i < 4)
        {
            return new Vector2((i - 1.5f) * (CellSize.x + Spacing.x), 0.5f * (CellSize.y + Spacing.y));
        }
        else
        {
            return new Vector2((i - 5.5f) * (CellSize.x + Spacing.x), -0.5f * (CellSize.y + Spacing.y));
        }
    }

    public void ShowChildModels(int id)
    {
        currentMotherModelID = id;
        for (int i = 0; i < types[id].childTypes.Length; i++)
        {
            types[id].childTypes[i].typeObject.SetActive(true);
            int j = Mathf.CeilToInt((i + 1) * 8.0f / 7.0f - 1);
            if (i >= 7)
            {
                types[id].childTypes[i].typeObject.transform.localPosition = GetPosition(j % 8);
                types[id].childTypes[i].typeObject.SetActive(false);
            }
            else
            {
                types[id].childTypes[i].typeScript.targetVec = GetPosition(j % 8);
                types[id].childTypes[i].typeScript.oldVec = GetPosition(0);
                types[id].childTypes[i].typeScript.targetID = types[id].childTypes[i].typeScript.id;
                types[id].childTypes[i].typeScript.time = 0;
                types[id].childTypes[i].typeScript.startMove = true;
            }
        }
        moving = true;
        childIndex = 1;
    }

    public void HideChildModels()
    {
        for (int i = 0; i < typeNum; i++)
        {
            for (int j = 0; j < types[i].childTypes.Length; j++)
            {
                if (!types[i].childTypes[j].typeObject.activeSelf) { continue; }
                types[i].childTypes[j].typeScript.targetVec = GetPosition(0);
                types[i].childTypes[j].typeScript.oldVec = types[i].childTypes[j].typeObject.transform.localPosition;
                types[i].childTypes[j].typeScript.targetID = -1;
                types[i].childTypes[j].typeScript.time = 0;
                types[i].childTypes[j].typeScript.startMove = true;
            }
        }
        moving = true;
    }

    public void RefreshTypes()
    {
        switch (stateSystem.currentState.ID)
        {
            case 1:
                for (int i = 0; i < typeNum; i++)
                {
                    if ((i >= (index - 1) * 8) && (i < index * 8))
                    {
                        types[i].typeObject.SetActive(true);
                        types[i].typeObject.transform.localPosition = GetPosition(i - (index - 1) * 8);
                    }
                    else
                    {
                        types[i].typeObject.SetActive(false);
                    }
                }
                break;
            case 2:
                for (int i = 0; i < types[currentMotherModelID].childTypes.Length; i++)
                {
                    int j = Mathf.CeilToInt((i + 1) * 8.0f / 7.0f - 1);
                    if ((j >= (childIndex - 1) * 8) && (j < childIndex * 8))
                    {
                        types[currentMotherModelID].childTypes[i].typeObject.SetActive(true);
                        types[currentMotherModelID].childTypes[i].typeObject.transform.localPosition = GetPosition(j - (childIndex - 1) * 8);
                    }
                    else
                    {
                        types[currentMotherModelID].childTypes[i].typeObject.SetActive(false);
                    }
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (stateSystem.currentState.ID)
        {
            case 1:
                if (index == 1)
                {
                    LastPageButton.SetActive(false);
                }
                else
                {
                    LastPageButton.SetActive(true);
                }

                if (index == (typeNum / 8 + 1))
                {
                    NextPageButton.SetActive(false);
                }
                else
                {
                    NextPageButton.SetActive(true);
                }
                break;
            case 2:
                if (childIndex == 1)
                {
                    LastPageButton.SetActive(false);
                }
                else
                {
                    LastPageButton.SetActive(true);
                }
                if (childIndex == (types[currentMotherModelID].childTypes.Length / 8 + 1))
                {
                    NextPageButton.SetActive(false);
                }
                else
                {
                    NextPageButton.SetActive(true);
                }
                break;
            case 3:
                break;
        }

    }

    public void AllMoveTo(Vector3 targetVec, int id)
    {
        for (int i = (index - 1) * 8; i < index * 8; i++)
        {
            if (i >= typeNum) { break; }
            if (i == id)
            {
                types[i].typeScript.transform.SetSiblingIndex(types[i].typeScript.transform.parent.childCount - 1);
            }
            types[i].typeScript.targetVec = targetVec;
            types[i].typeScript.oldVec = types[i].typeScript.transform.localPosition;
            types[i].typeScript.startMove = true;
            types[i].typeScript.targetID = id;
            types[i].typeScript.time = 0;
        }
        moving = true;
    }

    public void MotherModelMoveBack()
    {
        for (int i = (index - 1) * 8; i < index * 8; i++)
        {
            if (i >= typeNum) { break; }
            types[i].typeObject.SetActive(true);
            types[i].typeScript.targetVec = GetPosition(i - 8 * (index - 1));
            types[i].typeScript.oldVec = types[i].typeScript.transform.localPosition;
            types[i].typeScript.startMove = true;
            types[i].typeScript.targetID = i;
            types[i].typeScript.time = 0;
        }
        moving = true;
    }

    public void ChildModelStateExit(int targetID)
    {
        switch (targetID)
        {
            case 1:
                HideChildModels();
                break;
            case 3:
                break;
        }
    }



}
