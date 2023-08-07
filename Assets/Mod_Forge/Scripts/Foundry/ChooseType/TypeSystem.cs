using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public ChooseTypeEnum chooseType = ChooseTypeEnum.WaitForBegin;
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
        InitTypes();
    }

    void InitTypes()
    {
        for (int i = 0; i < typeNum; i++)
        {
            InstantiateType(i);
        }
        index = 1;
        chooseType = ChooseTypeEnum.WaitForBegin;
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
        for (int i = 0; i < types[id].childTypes.Length; i++)
        {
            types[id].childTypes[i].typeObject.SetActive(true);
            int j = Mathf.CeilToInt((i + 1) * 8.0f / 7.0f - 1);
            types[id].childTypes[i].typeScript.targetVec = GetPosition(j);
            types[id].childTypes[i].typeScript.oldVec = types[id].childTypes[i].typeObject.transform.localPosition;
            types[id].childTypes[i].typeScript.targetID = types[id].childTypes[i].typeScript.id;
            types[id].childTypes[i].typeScript.time = 0;
            types[id].childTypes[i].typeScript.startMove = true;

            if (i > 7) { types[id].childTypes[i].typeObject.SetActive(false); }
        }
    }

    public void HideChildModels()
    {
        for (int i = 0; i < typeNum; i++)
        {
            for (int j = 0; j < types[i].childTypes.Length; j++)
            {
                if (!types[i].childTypes[j].typeObject.activeSelf) { return; }
                types[i].childTypes[j].typeScript.targetVec = GetPosition(0);
                types[i].childTypes[j].typeScript.oldVec = types[i].childTypes[i].typeObject.transform.localPosition;
                types[i].childTypes[j].typeScript.targetID = types[i].childTypes[i].typeScript.id;
                types[i].childTypes[j].typeScript.time = 0;
                types[i].childTypes[j].typeScript.startMove = true;
            }
        }
    }

    public void RefreshTypes()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
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

        if (chooseType == ChooseTypeEnum.WaitForBegin)
        {
            chooseType = ChooseTypeEnum.FirstLevelMove;
        }
    }

}
