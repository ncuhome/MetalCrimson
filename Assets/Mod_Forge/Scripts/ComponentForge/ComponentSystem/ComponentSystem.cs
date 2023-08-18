using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ER.Items;
using UnityEngine.UI;

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
    /// <summary>
    /// 部件预制件
    /// </summary>
    public GameObject componentPrefab = null;
    /// <summary>
    /// 部件物品库
    /// </summary>
    public ItemStore componentsItemStore = null;
    /// <summary>
    /// 部件物体组
    /// </summary>
    public List<GameObject> components = null;
    /// <summary>
    /// 选择界面的Transform组件
    /// </summary>
    public Transform chooseComponentTransform = null;
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
        ShowComponentPanel();
    }
    /// <summary>
    /// 初始化部件物品库设置
    /// </summary>
    private void InitComponentItemStore()
    {
        ItemStoreManager.Instance.Creat("componentItemStore");
        componentsItemStore = ItemStoreManager.Instance.Stores["componentItemStore"];
        components = new List<GameObject>();


        Debug.Log(AddNormalComponent("StraightSwordModelComponent"));
    }
    /// <summary>
    /// 通过NameTmp添加部件,添加成功返回true，否则返回false
    /// </summary>
    public bool AddNormalComponent(string NameTmp)
    {
        if (TemplateStoreManager.Instance["Item"][NameTmp] == null) { return false; } // 如果没找到返回false

        //创建新的物品，并初始化
        componentsItemStore.AddItem(new ItemVariable(TemplateStoreManager.Instance["Item"][NameTmp], true));
        ItemVariable newComponentItem = componentsItemStore[componentsItemStore.Count - 1];

        newComponentItem.CreateAttribute("Name", newComponentItem.GetText("Name", false));

        GameObject newComponentObject = Instantiate(componentPrefab);
        components.Add(newComponentObject);
        newComponentObject.transform.SetParent(componentsParentTrans);
        newComponentObject.transform.localScale = Vector3.one;

        ComponentScript newComponentScript = newComponentObject.GetComponent<ComponentScript>();
        newComponentScript.ComponentItem = newComponentItem;
        newComponentScript.RefreshInfo();

        return true;
    }
    /// <summary>
    /// 通过ID添加部件,添加成功返回true，否则返回false
    /// </summary>
    public bool AddNormalComponent(int id)
    {

        if (TemplateStoreManager.Instance["Item"][id] == null) { return false; }// 如果没找到返回false

        //创建新的物品，并初始化
        componentsItemStore.AddItem(new ItemVariable(TemplateStoreManager.Instance["Item"][id], true));
        ItemVariable newComponentItem = componentsItemStore[componentsItemStore.Count - 1];

        newComponentItem.CreateAttribute("Name", newComponentItem.GetText("Name", false));

        GameObject newComponentObject = Instantiate(componentPrefab);
        components.Add(newComponentObject);
        newComponentObject.transform.SetParent(componentsParentTrans);
        newComponentObject.transform.localScale = Vector3.one;

        ComponentScript newComponentScript = newComponentObject.GetComponent<ComponentScript>();
        newComponentScript.ComponentItem = newComponentItem;
        newComponentScript.RefreshInfo();
        return true;
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
    /// 隐藏部件面板
    /// </summary>
    public void HideComponentPanel()
    {
        oldVec = chooseComponentTransform.localPosition;
        targetVec = new Vector3(0, -400, 0);
    }
    /// <summary>
    /// 显示部件面板
    /// </summary>
    public void ShowComponentPanel()
    {
        oldVec = chooseComponentTransform.localPosition;
        targetVec = Vector3.zero;
    }
    /// <summary>
    /// 点击显示更多按钮事件
    /// </summary>
    public void ShowMoreButtonClick()
    {
        oldVec = chooseComponentTransform.localPosition;
        if (ComponentChooseSystem.Instance.showMore)
        {
            targetVec = Vector3.zero;
            componentLayout.constraintCount = 1;
            ComponentChooseSystem.Instance.showMore = false;
        }
        else
        {
            targetVec = new Vector3(0, 616, 0);
            componentLayout.constraintCount = 3;
            ComponentChooseSystem.Instance.showMore = true;
        }
    }
}
