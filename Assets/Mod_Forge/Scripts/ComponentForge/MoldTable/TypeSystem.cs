using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using ER.Items;
using ER.Parser;
using States;

#region  模板结构体
/// <summary>
/// 母模板结构
/// </summary>
[System.Serializable]
public struct Type
{
    /// <summary>
    /// 模板名称
    /// </summary>
    public string name;
    /// <summary>
    /// 模板ID
    /// </summary>
    public int ID;
    /// <summary>
    /// 模板描述
    /// </summary>
    public string Description;
    /// <summary>
    /// 模板标签
    /// </summary>
    public string[] Tags;
    /// <summary>
    /// 模板图像
    /// </summary>
    public Sprite typeSprite;
    /// <summary>
    /// 模板物体
    /// </summary>
    public GameObject typeObject;
    /// <summary>
    /// 模板脚本
    /// </summary>
    public MotherType typeScript;
    /// <summary>
    /// 对应子模板
    /// </summary>
    public ChildType[] childTypes;
}
/// <summary>
/// 子模板结构
/// </summary>
[System.Serializable]
public struct ChildType
{
    /// <summary>
    /// 子模板名称
    /// </summary>
    public string name;
    /// <summary>
    /// 子模板ID
    /// </summary>
    public int ID;
    /// <summary>
    /// 对应母模板ID
    /// </summary>
    public int M_ID;
    /// <summary>
    /// 子模板描述
    /// </summary>
    public string Description;
    /// <summary>
    /// 子模板标签
    /// </summary>
    public string[] Tags;
    /// <summary>
    /// 子模板图像
    /// </summary>
    public Sprite typeSprite;
    /// <summary>
    /// 子模板物体
    /// </summary>
    public GameObject typeObject;
    /// <summary>
    /// 子模板脚本
    /// </summary>
    public ChildModelType typeScript;
    /// <summary>
    /// 消耗费用
    /// </summary>
    public float costMaterialNum;
    /// <summary>
    /// 锋利度
    /// </summary>
    public float sharpness;
    /// <summary>
    /// 耐用度
    /// </summary>
    public float durability;
    /// <summary>
    /// 重量
    /// </summary>
    public float weight;
}
#endregion
/// <summary>
/// 物体系统
/// </summary>
public class TypeSystem : MonoBehaviour
{
    #region  构建单例

    private static TypeSystem instance;
    /// <summary>
    /// 物体系统单例
    /// </summary>
    public static TypeSystem Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    #region 参数
    /// <summary>
    /// 母类型数量
    /// </summary>
    public int typeNum;
    /// <summary>
    /// 模具数组
    /// </summary>
    public Type[] types;
    /// <summary>
    /// 模具预制件
    /// </summary>
    public GameObject motherModelPrefab, childModelPrefab;
    /// <summary>
    /// 模具父物体Trans
    /// </summary>
    public Transform typeParentTrans;
    /// <summary>
    /// 母模具页数
    /// </summary>
    public int index = 1;
    /// <summary>
    /// 子模具页数
    /// </summary>
    public int childIndex = 1;
    /// <summary>
    /// 模拟layout的物件大小
    /// </summary>
    public Vector2 CellSize;
    /// <summary>
    /// 模拟Layout的间隙
    /// </summary>
    public Vector2 Spacing;
    /// <summary>
    /// 前后按钮
    /// </summary>
    public GameObject NextPageButton, LastPageButton;
    /// <summary>
    /// 状态系统
    /// </summary>
    public StateSystem stateSystem;
    /// <summary>
    /// 当前的母模板ID
    /// </summary>
    public int currentMotherModelID;
    /// <summary>
    /// 当前子模板ID
    /// </summary>
    public int chosenChildModelID;
    /// <summary>
    /// 当前母模板
    /// </summary>
    public Type currentMotherModel;
    /// <summary>
    /// 当前子模板
    /// </summary>
    public ChildType chosenChildModel;
    /// <summary>
    /// 卡背
    /// </summary>
    public ModelBack modelBack;
    /// <summary>
    /// 是否正在移动
    /// </summary>
    public bool moving;

    public ChildModelCard childModelCard;

    public MaterialScript materialScript;
    #endregion

    #region 方法
    /// <summary>
    /// 构建单例
    /// </summary>
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

    }
    // Update is called once per frame
    void Update()
    {
        RefreshButton();
    }

    public void InitTypeSystem()
    {
        InitState();
        InitTypeData();
        InitTypes();
    }

    /// <summary>
    /// 初始化状态系统
    /// </summary>
    void InitState()
    {
        StateSystemManager.Instance.CreateStateSystem("TypeStateSystem");
        stateSystem = StateSystemManager.Instance["TypeStateSystem"];

        State showMotherModel = new State(1, "showMotherModel");
        showMotherModel.ChangeExitJudgement(2, false);
        stateSystem.AddState(showMotherModel);

        State showChildModel = new State(2, "showChildModel");
        showChildModel.ChangeExitJudgement(3, false);
        showChildModel.ChangeExitJudgement(1, false);
        Action<int> action = ChildModelStateExit;
        showChildModel.ChangeExitAction(action);
        stateSystem.AddState(showChildModel);

        State chooseMaterial = new State(3, "chooseMaterial");
        chooseMaterial.ChangeExitJudgement(2, false);
        chooseMaterial.ChangeExitJudgement(4, false);
        stateSystem.AddState(chooseMaterial);

        State getComponent = new State(4, "getComponent");
        getComponent.ChangeExitJudgement(3, false);
        stateSystem.AddState(getComponent);
    }
    /// <summary>
    /// 初始化模具信息
    /// </summary>
    void InitTypeData()
    {
        ItemTemplateStore modelStore = TemplateStoreManager.Instance["Item"];
        List<ItemTemplate> models = modelStore.FindContainsPart("Tags", "MainModel", ';');
        types = new Type[models.Count];
        typeNum = models.Count;
        for (int i = 0; i < models.Count; i++)
        {
            types[i].name = models[i].GetText("Name");
            types[i].ID = models[i].GetInt("ID");
            types[i].Description = models[i].GetText("Description");
            types[i].Tags = models[i].SplitText("Tags", ';');
            types[i].typeSprite = Resources.Load<Sprite>(models[i].GetText("Address"));

            Data data = new Data(types[i].ID, DataType.Integer);
            List<ItemTemplate> childModels = modelStore.Find("M_ID", data);

            types[i].childTypes = new ChildType[childModels.Count];
            for (int j = 0; j < childModels.Count; j++)
            {
                types[i].childTypes[j].name = childModels[j].GetText("Name");
                types[i].childTypes[j].ID = childModels[j].GetInt("ID");
                types[i].childTypes[j].M_ID = childModels[j].GetInt("M_ID");
                types[i].childTypes[j].Description = childModels[j].GetText("Description");
                types[i].childTypes[j].Tags = childModels[j].SplitText("Tags", ';');
                Debug.Log(childModels[j].GetText("NameTmp") + " " + childModels[j].ID);
                types[i].childTypes[j].typeSprite = Resources.Load<Sprite>(childModels[j].GetText("Address"));
                types[i].childTypes[j].costMaterialNum = childModels[j].GetFloat("CostNum");
                types[i].childTypes[j].sharpness = childModels[j].GetFloat("Sharpness");
                types[i].childTypes[j].durability = childModels[j].GetFloat("Durability");
                types[i].childTypes[j].weight = childModels[j].GetFloat("Weight");
            }
        }
    }
    /// <summary>
    /// 初始化模具物体
    /// </summary>
    void InitTypes()
    {
        for (int i = 0; i < typeNum; i++)
        {
            InstantiateType(i);
        }
        index = 1;
        childIndex = 1;
    }
    /// <summary>
    /// 生成模具物体
    /// </summary>
    /// <param name="i">模具索引</param>
    void InstantiateType(int i)
    {
        types[i].typeObject = Instantiate(motherModelPrefab);
        types[i].typeObject.transform.parent = typeParentTrans;
        types[i].typeObject.transform.localScale = Vector3.one;
        types[i].typeScript = types[i].typeObject.GetComponent<MotherType>();
        types[i].typeScript.typeImage.sprite = types[i].typeSprite;
        types[i].typeScript.typeText.text = types[i].name;
        types[i].typeScript.id = types[i].ID;
        types[i].typeObject.transform.localPosition = GetPosition(i);
        for (int j = 0; j < types[i].childTypes.Length; j++)
        {
            InstantiateChildType(i, j);
        }
        if (i >= 8) { types[i].typeObject.SetActive(false); }
    }
    /// <summary>
    /// 生成子模具物体
    /// </summary>
    /// <param name="motherIndex">母模具索引</param>
    /// <param name="childIndex">子模具索引</param>
    void InstantiateChildType(int motherIndex, int childIndex)
    {
        types[motherIndex].childTypes[childIndex].typeObject = Instantiate(childModelPrefab);
        types[motherIndex].childTypes[childIndex].typeObject.transform.parent = typeParentTrans;
        types[motherIndex].childTypes[childIndex].typeObject.transform.localPosition = types[motherIndex].typeObject.transform.localPosition;
        types[motherIndex].childTypes[childIndex].typeObject.transform.localScale = Vector3.one;
        types[motherIndex].childTypes[childIndex].typeScript = types[motherIndex].childTypes[childIndex].typeObject.GetComponent<ChildModelType>();
        types[motherIndex].childTypes[childIndex].typeScript.typeImage.sprite = types[motherIndex].childTypes[childIndex].typeSprite;
        types[motherIndex].childTypes[childIndex].typeScript.typeText.text = types[motherIndex].childTypes[childIndex].name;
        types[motherIndex].childTypes[childIndex].typeScript.id = types[motherIndex].childTypes[childIndex].ID;
        types[motherIndex].childTypes[childIndex].typeScript.motherId = types[motherIndex].ID;
        types[motherIndex].childTypes[childIndex].typeObject.SetActive(false);
    }
    /// <summary>
    /// 模拟Layout，返回索引对应位置
    /// </summary>
    /// <param name="i">索引</param>
    /// <returns>模拟Layout的位置</returns>
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
    /// <summary>
    /// 显示对应子模具
    /// </summary>
    /// <param name="id">子模具ID</param>
    public void ShowChildModels(int id)
    {
        currentMotherModelID = id;
        currentMotherModel = GetM_Type(id);
        Debug.Log(currentMotherModel.ID);
        for (int i = 0; i < currentMotherModel.childTypes.Length; i++)
        {
            currentMotherModel.childTypes[i].typeObject.SetActive(true);
            int j = Mathf.CeilToInt((i + 1) * 8.0f / 7.0f - 1);
            if (i >= 7)
            {
                currentMotherModel.childTypes[i].typeObject.transform.localPosition = GetPosition(j % 8);
                currentMotherModel.childTypes[i].typeObject.SetActive(false);
            }
            else
            {
                currentMotherModel.childTypes[i].typeScript.targetVec = GetPosition(j % 8);
                currentMotherModel.childTypes[i].typeScript.oldVec = GetPosition(0);
                currentMotherModel.childTypes[i].typeScript.targetID = currentMotherModel.childTypes[i].typeScript.id;
                currentMotherModel.childTypes[i].typeScript.time = 0;
                currentMotherModel.childTypes[i].typeScript.startMove = true;
            }
        }
        moving = true;
        childIndex = 1;
    }
    /// <summary>
    /// 隐藏所有子模具
    /// </summary>
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
    /// <summary>
    /// 刷新模具位置
    /// </summary>
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
                currentMotherModel = (Type)GetM_Type(currentMotherModelID);
                for (int i = 0; i < currentMotherModel.childTypes.Length; i++)
                {
                    int j = Mathf.CeilToInt((i + 1) * 8.0f / 7.0f - 1);
                    if ((j >= (childIndex - 1) * 8) && (j < childIndex * 8))
                    {
                        currentMotherModel.childTypes[i].typeObject.SetActive(true);
                        currentMotherModel.childTypes[i].typeObject.transform.localPosition = GetPosition(j - (childIndex - 1) * 8);
                    }
                    else
                    {
                        currentMotherModel.childTypes[i].typeObject.SetActive(false);
                    }
                }
                break;
        }
    }
    /// <summary>
    /// 刷新按钮是否显示
    /// </summary>
    void RefreshButton()
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
                currentMotherModel = GetM_Type(currentMotherModelID);
                if (childIndex == (currentMotherModel.childTypes.Length / 8 + 1))
                {
                    NextPageButton.SetActive(false);
                }
                else
                {
                    NextPageButton.SetActive(true);
                }
                break;
            case 3:
                LastPageButton.SetActive(false);
                NextPageButton.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// 所有母模具移动至指定位置
    /// </summary>
    /// <param name="targetVec">指定位置</param>
    /// <param name="id">目标模具ID</param>
    public void AllMoveTo(Vector3 targetVec, int id)
    {
        currentMotherModelID = id;
        for (int i = (index - 1) * 8; i < index * 8; i++)
        {
            if (i >= typeNum) { break; }
            if (types[i].ID == id)
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
    /// <summary>
    /// 母模具移动回原有位置
    /// </summary>
    public void MotherModelMoveBack()
    {
        for (int i = (index - 1) * 8; i < index * 8; i++)
        {
            if (i >= typeNum) { break; }
            types[i].typeObject.SetActive(true);
            types[i].typeScript.targetVec = GetPosition(i - 8 * (index - 1));
            types[i].typeScript.oldVec = types[i].typeScript.transform.localPosition;
            types[i].typeScript.startMove = true;
            types[i].typeScript.targetID = types[i].ID;
            types[i].typeScript.time = 0;
        }
        moving = true;
    }

    /// <summary>
    /// 选择子模具
    /// </summary>
    public void ChosenChildModel()
    {
        currentMotherModel = GetM_Type(currentMotherModelID);
        currentMotherModel.typeScript.oldAlpha = 1f;
        currentMotherModel.typeScript.targetAlpha = 0f;
        currentMotherModel.typeScript.startColor = true;
        for (int i = 0; i < currentMotherModel.childTypes.Length; i++)
        {
            if (i > 6) { return; }
            if (currentMotherModel.childTypes[i].ID != chosenChildModelID)
            {
                currentMotherModel.childTypes[i].typeScript.oldAlpha = 1f;
                currentMotherModel.childTypes[i].typeScript.targetAlpha = 0f;
                currentMotherModel.childTypes[i].typeScript.startColor = true;
            }
            else
            {
                currentMotherModel.childTypes[i].typeScript.oldVec = currentMotherModel.childTypes[i].typeObject.transform.localPosition;
                currentMotherModel.childTypes[i].typeScript.targetVec = new Vector3(0, 100, 0);
                currentMotherModel.childTypes[i].typeScript.startMove = true;
            }
        }

        MaterialSystem.Instance.needMaterialNum = chosenChildModel.costMaterialNum;

        modelBack.gameObject.SetActive(true);
        modelBack.targetAlpha = 1f;
        modelBack.oldAlpha = 0f;
        modelBack.startColor = true;

        moving = true;
    }
    /// <summary>
    /// 退回子模具选择界面
    /// </summary>
    public void ReturnChildModel()
    {
        currentMotherModel.typeScript.SetAlpha(0.75f);
        currentMotherModel.typeScript.oldVec = new Vector3(0, 100, 0);
        currentMotherModel.typeScript.targetVec = GetPosition(0);
        currentMotherModel.typeScript.startMove = true;
        currentMotherModel.typeScript.targetID = currentMotherModelID;
        for (int i = 0; i < currentMotherModel.childTypes.Length; i++)
        {
            if (i > 6) { return; }
            ChildType childType = currentMotherModel.childTypes[i];
            childType.typeScript.SetAlpha(0.75f);
            childType.typeScript.oldVec = new Vector3(0, 100, 0);
            childType.typeScript.targetVec = GetPosition(Mathf.CeilToInt((i + 1) * 8f / 7f - 1));
            childType.typeScript.startMove = true;
            childType.typeScript.targetID = childType.ID;
        }

        modelBack.gameObject.SetActive(true);
        modelBack.targetAlpha = 0f;
        modelBack.oldAlpha = 1f;
        modelBack.startColor = true;

        moving = true;
    }
    /// <summary>
    /// 获取ID对应母模具
    /// </summary>
    /// <param name="M_ID">母模具ID</param>
    /// <returns>对应母模具</returns>
    public Type GetM_Type(int M_ID)
    {
        foreach (Type motherType in types)
        {
            if (motherType.ID == M_ID) return motherType;
        }
        return new Type();
    }
    /// <summary>
    /// 获取对应子模具
    /// </summary>
    /// <param name="M_ID">母模具ID</param>
    /// <param name="C_ID">子模具ID</param>
    /// <returns>对应子模具</returns>
    public ChildType GetChildType(int M_ID, int C_ID)
    {
        foreach (ChildType childType in GetM_Type(M_ID).childTypes)
        {
            if (childType.ID == C_ID) return childType;
        }
        return new ChildType();
    }

    /// <summary>
    /// 闭合卡背
    /// </summary>
    public void CloseModelBack()
    {
        modelBack.targetVec = chosenChildModel.typeObject.transform.localPosition;
        modelBack.oldVec = chosenChildModel.typeObject.transform.localPosition + new Vector3(400, 0, 0);
        modelBack.startMove = true;
        moving = true;
    }
    /// <summary>
    /// 打开卡背
    /// </summary>
    public void OpenModelBack()
    {
        AddComponent();
        modelBack.targetVec = chosenChildModel.typeObject.transform.localPosition + new Vector3(400, 0, 0);
        modelBack.oldVec = chosenChildModel.typeObject.transform.localPosition;
        modelBack.startMove = true;
        moving = true;
    }

    public void AddComponent()
    {
        ItemTemplate childModelItem = TemplateStoreManager.Instance["Item"][chosenChildModelID];
        ComponentSystem.Instance.AddComponent(childModelItem.GetInt("P_ID"), materialScript.MaterialItem);
        materialScript.MaterialItem.CreateAttribute("Num", materialScript.MaterialItem.GetFloat("Num") - childModelItem.GetFloat("CostNum"));
        materialScript.RefreshInfo();
        UIManager.Instance.tipScript.ShowTips(childModelItem.GetInt("P_ID"));
    }

    public void ShowCard(ChildType childType)
    {
        childModelCard.gameObject.SetActive(true);
        childModelCard.childType = childType;
        childModelCard.RefreshCard();
    }

    public void HideCard()
    {
        childModelCard.gameObject.SetActive(false);
    }

    public void MoveCard(Vector2 targetVec)
    {
        childModelCard.transform.localPosition = targetVec;
    }

    /// <summary>
    /// 状态2退出事件
    /// </summary>
    /// <param name="targetID"></param>
    public void ChildModelStateExit(int targetID)
    {
        switch (targetID)
        {
            case 1:
                HideChildModels();
                break;
            case 3:
                MaterialSystem.Instance.ShowMaterialPanel();
                ChosenChildModel();
                break;
        }
    }
    /// <summary>
    /// 状态3退出事件
    /// </summary>
    /// <param name="targetID"></param>
    public void ChosenModelExit(int targetID)
    {
        switch (targetID)
        {
            case 2:
                MaterialSystem.Instance.HideMaterialPanel();
                ReturnChildModel();
                break;
            case 4:
                CloseModelBack();
                break;
        }
    }
    /// <summary>
    /// 状态4退出事件
    /// </summary>
    /// <param name="targetID"></param>
    public void GetComponentExit(int targetID)
    {
        switch (targetID)
        {
            case 3:
                OpenModelBack();
                break;
        }
    }

    #endregion

}
