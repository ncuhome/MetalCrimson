using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ER.Items;
using UnityEngine.UI;

[System.Serializable]
public class ComponentType
{
    public GameObject typeObject;
    public string typeNameTmp;
    public string typeName;
    public int typeID;
    public List<GameObject> components;
    public GameObject typeChooseObject;
    public ComponentType(int i = 0)
    {
        typeID = i;
        typeNameTmp = TemplateStoreManager.Instance["Item"][typeID].NameTmp;
        typeName = TemplateStoreManager.Instance["Item"][typeID].GetText("Name");
        components = new List<GameObject>();
    }
    public ComponentType(string nameTmp)
    {
        typeNameTmp = nameTmp;
        typeID = TemplateStoreManager.Instance["Item"][typeNameTmp].ID;
        typeName = TemplateStoreManager.Instance["Item"][typeNameTmp].GetText("Name");
        components = new List<GameObject>();
    }
    public void Add(GameObject gameObject)
    {
        components.Add(gameObject);
    }
}

public class ComponentSystem : MonoBehaviour
{
    #region 单例封装

    private static ComponentSystem instance;

    public static ComponentSystem Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion 单例封装

    /// <summary>
    /// 部件父物体Transform组件
    /// </summary>
    public Transform componentsParentTrans = null;
    public Transform chooseTypeParentTrans = null;
    public Transform AnvilTrans = null;
    /// <summary>
    /// 部件预制件
    /// </summary>
    public GameObject componentPrefab = null;
    public GameObject componentTypePrefab = null;
    public GameObject typeChoosePrefab = null;
    public GameObject linkPromptPrefab = null;
    /// <summary>
    /// 部件物品库
    /// </summary>
    public ItemStore componentsItemStore = null;
    /// <summary>
    /// 部件物体组
    /// </summary>
    public List<ComponentType> componentTypes = null;
    /// <summary>
    /// 选择界面的Transform组件
    /// </summary>
    public Transform chooseComponentTransform = null;
    public Transform chooseTypeTransform = null;
    /// <summary>
    /// 部件组对应的Layout组件
    /// </summary>
    public GridLayoutGroup componentLayout = null;
    /// <summary>
    /// 目标坐标
    /// </summary>
    private Vector3 targetVec;
    /// <summary>
    /// 原坐标
    /// </summary>
    private Vector3 oldVec;
    /// <summary>
    /// 是否正在移动
    /// </summary>
    public bool move = false;
    /// <summary>
    /// 移动时间
    /// </summary>
    private float moveTime;
    public int currentTypeID;
    public int currentComponentNum;

    public List<ComponentScript> componentInAnvil = null;

    public LinkPrompt outPort = null;
    public LinkPrompt inPort = null;

    public LinkPrompt outLink = null;
    public LinkPrompt inLink = null;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
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
        if (Vector3.Magnitude(chooseComponentTransform.localPosition - targetVec) < 0.5f)
        {
            chooseComponentTransform.localPosition = targetVec;
            moveTime = 0f;
            move = false;
        }
        else
        {
            move = true;
            moveTime += Time.deltaTime * 5;
            Vector3 newVec = Vector3.Lerp(oldVec, targetVec, moveTime);
            chooseComponentTransform.localPosition = newVec;
        }
    }

    public void InitComponentSystem()
    {
        InitComponentItemStore();
        targetVec = chooseComponentTransform.localPosition;
    }
    /// <summary>
    /// 初始化部件物品库设置
    /// </summary>
    private void InitComponentItemStore()
    {
        ItemStoreManager.Instance.Create("componentItemStore");
        componentsItemStore = ItemStoreManager.Instance.Stores["componentItemStore"];
        componentTypes = new List<ComponentType>();
        componentInAnvil = new List<ComponentScript>();

        ItemVariable rawIron = new ItemVariable(TemplateStoreManager.Instance["Item"]["RawIron"], true);
        for (int i = 0; i < 2; i++)
        {
            Debug.Log(AddComponent(8083771, rawIron));
            Debug.Log(AddComponent(8083772, rawIron));
            Debug.Log(AddComponent(8083773, rawIron));
        }
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(AddComponent(8065881, rawIron));
            Debug.Log(AddComponent(8083801, rawIron));
            Debug.Log(AddComponent(8080731, rawIron));
            Debug.Log(AddComponent(8072731, rawIron));
            Debug.Log(AddComponent(8072691, rawIron));
            Debug.Log(AddComponent(8066731, rawIron));
        }

    }
    /// <summary>
    /// 通过NameTmp添加部件,添加成功返回true，否则返回false
    /// </summary>
    public bool AddComponent(string NameTmp, ItemVariable materialItem)
    {
        if (TemplateStoreManager.Instance["Item"][NameTmp] == null) { return false; } // 如果没找到返回false

        //创建新的物品，并初始化
        componentsItemStore.AddItem(new ItemVariable(TemplateStoreManager.Instance["Item"][NameTmp], true));
        ItemVariable newComponentItem = componentsItemStore[componentsItemStore.Count - 1];

        newComponentItem.CreateAttribute("Name", newComponentItem.GetText("Name", false));
        newComponentItem.CreateAttribute("Material_ID", materialItem.ID);

        ItemTemplate modelItem = TemplateStoreManager.Instance["Item"][newComponentItem.GetInt("Model_ID")];
        newComponentItem.CreateAttribute("Density", materialItem.GetFloat("Density", false));
        newComponentItem.CreateAttribute("Mm", newComponentItem.GetFloat("Density") * modelItem.GetFloat("CostNum"));
        newComponentItem.CreateAttribute("Flexability", materialItem.GetFloat("Flexability", false));
        newComponentItem.CreateAttribute("Toughness", materialItem.GetFloat("Toughness", false));
        newComponentItem.CreateAttribute("AntiSolution", materialItem.GetFloat("AntiSolution", false));
        newComponentItem.CreateAttribute("M", newComponentItem.GetFloat("Mm") * newComponentItem.GetFloat("Weight"));
        newComponentItem.CreateAttribute("Dur", (newComponentItem.GetFloat("Flexability") / 8 + newComponentItem.GetFloat("Toughness")) * newComponentItem.GetFloat("Duability"));
        newComponentItem.CreateAttribute("Sharp", newComponentItem.GetFloat("Sharpness") * (10 + newComponentItem.GetFloat("Toughness") / 10));

        componentPrefab = Resources.Load<GameObject>("Prefabs/Components/" + NameTmp);
        ComponentType componentType = GetComponentType(TemplateStoreManager.Instance["Item"][NameTmp].GetInt("MotherModel_ID"));
        GameObject newComponentObject = Instantiate(componentPrefab, componentType.typeObject.transform);
        componentType.Add(newComponentObject);
        newComponentObject.transform.localScale = Vector3.one;


        ComponentScript newComponentScript = newComponentObject.GetComponent<ComponentScript>();
        newComponentScript.ComponentItem = newComponentItem;
        newComponentScript.RefreshInfo();

        if (currentTypeID == 0) { currentTypeID = componentType.typeID; }

        RefreshTypes();

        return true;
    }
    /// <summary>
    /// 通过ID添加部件,添加成功返回true，否则返回false
    /// </summary>
    public bool AddComponent(int id, ItemVariable materialItem)
    {

        if (TemplateStoreManager.Instance["Item"][id] == null) { return false; }// 如果没找到返回false

        //创建新的物品，并初始化
        componentsItemStore.AddItem(new ItemVariable(TemplateStoreManager.Instance["Item"][id], true));
        ItemVariable newComponentItem = componentsItemStore[componentsItemStore.Count - 1];

        newComponentItem.CreateAttribute("Name", newComponentItem.GetText("Name", false));
        newComponentItem.CreateAttribute("Material_ID", materialItem.ID);

        ItemTemplate modelItem = TemplateStoreManager.Instance["Item"][newComponentItem.GetInt("Model_ID")];
        newComponentItem.CreateAttribute("Density", materialItem.GetFloat("Density", false));
        newComponentItem.CreateAttribute("Mm", newComponentItem.GetFloat("Density") * modelItem.GetFloat("CostNum"));
        newComponentItem.CreateAttribute("Flexability", materialItem.GetFloat("Flexability", false));
        newComponentItem.CreateAttribute("Toughness", materialItem.GetFloat("Toughness", false));
        newComponentItem.CreateAttribute("AntiSolution", materialItem.GetFloat("AntiSolution", false));
        newComponentItem.CreateAttribute("M", newComponentItem.GetFloat("Mm") * newComponentItem.GetFloat("Weight"));
        newComponentItem.CreateAttribute("Dur", (newComponentItem.GetFloat("Flexability") / 8 + newComponentItem.GetFloat("Toughness")) * newComponentItem.GetFloat("Duability"));
        newComponentItem.CreateAttribute("Sharp", newComponentItem.GetFloat("Sharpness") * (10 + newComponentItem.GetFloat("Toughness") / 10));

        componentPrefab = Resources.Load<GameObject>("Prefabs/Components/" + newComponentItem.GetText("NameTmp"));
        ComponentType componentType = GetComponentType(TemplateStoreManager.Instance["Item"][id].GetInt("MotherModel_ID"));
        GameObject newComponentObject = Instantiate(componentPrefab, componentType.typeObject.transform);
        componentType.Add(newComponentObject);
        newComponentObject.transform.localScale = Vector3.one;


        ComponentScript newComponentScript = newComponentObject.GetComponent<ComponentScript>();
        newComponentScript.ComponentItem = newComponentItem;

        newComponentScript.RefreshInfo();

        if (currentTypeID == 0) { currentTypeID = componentType.typeID; }

        RefreshTypes();
        return true;
    }

    public void RemoveComponent(ComponentScript componentScript)
    {
        int typeId = componentScript.ComponentItem.GetInt("MotherModel_ID");
        GetComponentType(typeId).components.Remove(componentScript.gameObject);
        for (int i = 0; i < componentsItemStore.Count; i++)
        {
            if (componentsItemStore[i] == componentScript.ComponentItem)
            {
                componentsItemStore.RemoveItem(i);
            }
        }
        Destroy(componentScript.gameObject);
    }

    /// <summary>
    /// 通过索引获取部件脚本
    /// </summary>
    /// <param name="index">索引</param>
    /// <returns>索引对应脚本</returns>
    public ComponentScript GetComponentScript(int index)
    {
        if (index > (componentsParentTrans.childCount - 1))
        {
            return null;
        }
        return componentsParentTrans.GetChild(index).GetComponent<ComponentScript>();
    }

    /// <summary>
    /// 点击显示更多按钮事件
    /// </summary>
    public void ShowMoreButtonClick()
    {
        oldVec = chooseComponentTransform.localPosition;
        if (ComponentChooseSystem.Instance.showMore)
        {
            targetVec = new Vector3(-126, 0, 0);
            if (componentLayout)
            {
                componentLayout.constraintCount = 1;
            }
            ComponentChooseSystem.Instance.showMore = false;
        }
        else
        {
            targetVec = new Vector3(-126, 457, 0);
            if (componentLayout)
            {
                componentLayout.constraintCount = 3;
            }
            ComponentChooseSystem.Instance.showMore = true;
        }
    }

    public ComponentType GetComponentType(int id)
    {
        for (int i = 0; i < componentTypes.Count; i++)
        {
            if (componentTypes[i].typeID == id) { return componentTypes[i]; }
        }
        ComponentType componentType = new ComponentType(id);
        componentType.typeObject = Instantiate(componentTypePrefab);
        componentType.typeObject.transform.SetParent(componentsParentTrans);
        componentType.typeObject.transform.localScale = Vector3.one;
        componentType.typeObject.name = componentType.typeName;

        componentType.typeChooseObject = Instantiate(typeChoosePrefab, chooseTypeParentTrans);
        componentType.typeChooseObject.transform.localScale = Vector3.one;
        componentType.typeChooseObject.name = componentType.typeName;
        componentType.typeChooseObject.GetComponent<ComponentTypeScript>().typeID = id;
        componentType.typeChooseObject.GetComponent<ComponentTypeScript>().typeText.text = componentType.typeName;
        componentType.typeChooseObject.GetComponent<ComponentTypeScript>().typeImage.sprite = Resources.Load<Sprite>(TemplateStoreManager.Instance["Item"][id].GetText("ComponentAddress"));
        componentType.typeChooseObject.GetComponent<ComponentTypeScript>().typeImage.SetNativeSize();

        componentTypes.Add(componentType);

        return componentType;
    }

    public ComponentType GetComponentType(string nameTmp)
    {
        for (int i = 0; i < componentTypes.Count; i++)
        {
            if (componentTypes[i].typeNameTmp.Equals(nameTmp)) { return componentTypes[i]; }
        }

        ComponentType componentType = new ComponentType(nameTmp);
        componentType.typeObject = Instantiate(componentTypePrefab, componentsParentTrans);
        componentType.typeObject.transform.localScale = Vector3.one;
        componentType.typeObject.name = componentType.typeName;

        componentType.typeChooseObject = Instantiate(typeChoosePrefab);
        componentType.typeChooseObject.transform.SetParent(chooseTypeParentTrans);
        componentType.typeChooseObject.transform.localScale = Vector3.one;
        componentType.typeChooseObject.name = componentType.typeName;
        componentType.typeChooseObject.GetComponent<ComponentTypeScript>().typeID = componentType.typeID;
        componentType.typeChooseObject.GetComponent<ComponentTypeScript>().typeText.text = componentType.typeName;
        componentType.typeChooseObject.GetComponent<ComponentTypeScript>().typeImage.sprite = Resources.Load<Sprite>(TemplateStoreManager.Instance["Item"][nameTmp].GetText("ComponentAddress"));
        componentType.typeChooseObject.GetComponent<ComponentTypeScript>().typeImage.SetNativeSize();

        componentTypes.Add(componentType);

        return componentType;
    }

    public void RefreshTypes()
    {
        for (int i = 0; i < componentTypes.Count; i++)
        {
            if (componentTypes[i].typeID == currentTypeID)
            {
                componentTypes[i].typeObject.SetActive(true);
            }
            else
            {
                componentTypes[i].typeObject.SetActive(false);
            }
        }
        componentLayout = GetComponentType(currentTypeID).typeObject.GetComponent<GridLayoutGroup>();
        currentComponentNum = GetComponentType(currentTypeID).typeObject.transform.childCount;
    }

    // public void AddLinkPrompt(ItemVariable newComponentItem, GameObject componentObject, ComponentScript componentScript)
    // {
    //     if (newComponentItem.GetBool("In"))
    //     {
    //         GameObject linkPrompt = Instantiate(linkPromptPrefab, componentObject.transform);
    //         RectTransform rectTransform = componentObject.GetComponent<RectTransform>();
    //         linkPrompt.transform.localPosition = new Vector3(0, rectTransform.sizeDelta.y / 2f, 0);
    //         componentScript.inPrompt = linkPrompt;
    //         linkPrompt.GetComponent<Image>().enabled = false;
    //     }
    //     if (newComponentItem.GetBool("Out"))
    //     {
    //         GameObject linkPrompt = Instantiate(linkPromptPrefab, componentObject.transform);
    //         RectTransform rectTransform = componentObject.GetComponent<RectTransform>();
    //         linkPrompt.transform.localPosition = new Vector3(0, -rectTransform.sizeDelta.y / 2f, 0);
    //         componentScript.outPrompt = linkPrompt;
    //         linkPrompt.GetComponent<Image>().enabled = false;
    //     }
    // }

    public void AddComponentToAnvil(ComponentScript componentScript)
    {
        if (componentInAnvil.Exists(componentInAnvil => componentInAnvil == componentScript))
        {
            return;
        }
        if (inLink && outLink)
        {
            inLink.Match(outLink);
        }
        componentInAnvil.Add(componentScript);
        WeaponSystem.Instance.AddAttribute(componentScript);
        WeaponSystem.Instance.RefreshWeaponInfo();
        FindNextInPrompt();
        FindNextOutPrompt();
    }

    public void RemoveComponentFromAnvil(ComponentScript componentScript)
    {
        componentInAnvil.Remove(componentScript);
        WeaponSystem.Instance.RemoveAttribute(componentScript);
        WeaponSystem.Instance.RefreshWeaponInfo();
        if (componentScript.inPrompt && componentScript.inPrompt.linkedPrompt) { componentScript.inPrompt.RemoveLink(); }
        if (componentScript.outPrompt && componentScript.outPrompt.linkedPrompt) { componentScript.outPrompt.RemoveLink(); }
        FindNextInPrompt();
        FindNextOutPrompt();
    }

    public void FindNextOutPrompt()
    {
        for (int i = 0; i < componentInAnvil.Count; i++)
        {
            if (componentInAnvil[i].outPrompt && !componentInAnvil[i].outPrompt.linkedPrompt)
            {
                outPort = componentInAnvil[i].outPrompt;
                return;
            }
        }
        outPort = null;
    }

    public void FindNextInPrompt()
    {
        for (int i = 0; i < componentInAnvil.Count; i++)
        {
            if (componentInAnvil[i].inPrompt && !componentInAnvil[i].inPrompt.linkedPrompt)
            {
                inPort = componentInAnvil[i].inPrompt;
                return;
            }
        }
        inPort = null;
    }

    public void FinishBuild()
    {
        for (int i = 0; i < componentInAnvil.Count; i++)
        {
            RemoveComponent(componentInAnvil[i]);
        }
        componentInAnvil = new List<ComponentScript>();
    }

    public void Undo()
    {
        if (componentInAnvil.Count != 0)
        {
            componentInAnvil[componentInAnvil.Count - 1].componentImage.MoveBack();
        }
    }

    public void Reset()
    {
        for (int i = componentInAnvil.Count - 1; i >= 0; i--)
        {
            componentInAnvil[i].componentImage.MoveBack();
        }
    }
}
