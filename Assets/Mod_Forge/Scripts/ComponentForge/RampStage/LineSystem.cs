using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ER.Items;
public class LineSystem : MonoBehaviour
{
    #region 单例封装

    private static LineSystem instance;

    public static LineSystem Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion 单例封装
    /// <summary>
    /// 绕线杆父物体Transform组件
    /// </summary>
    public Transform LinesParentTrans = null;
    /// <summary>
    /// 绕线杆预制件
    /// </summary>
    public GameObject LinePrefab = null;
    /// <summary>
    /// 选择界面的Transform组件
    /// </summary>
    public Transform chooseLineTransform = null;
    /// <summary>
    /// 绕线杆组对应的Layout组件
    /// </summary>
    public GridLayoutGroup LinesLayout = null;
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
    /// 材料物体组
    /// </summary>
    public List<GameObject> lines = null;
    /// <summary>
    /// 材料物品库
    /// </summary>
    public ItemStore linesItemStore = null;
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
        InitLineItemStore();
        targetVec = chooseLineTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Magnitude(chooseLineTransform.localPosition - targetVec) < 0.5f)
        {
            chooseLineTransform.localPosition = targetVec;
            moveTime = 0f;
            move = false;
        }
        else
        {
            move = true;
            moveTime += Time.deltaTime * 5;
            Vector3 newVec = Vector3.Lerp(oldVec, targetVec, moveTime);
            chooseLineTransform.localPosition = newVec;
        }
    }
    /// <summary>
    /// 初始化材料物品库设置
    /// </summary>
    private void InitLineItemStore()
    {
        ItemStoreManager.Instance.Create("lineItemStore");
        linesItemStore = ItemStoreManager.Instance.Stores["lineItemStore"];
        lines = new List<GameObject>();

        for (int i = 0; i < 6; i++)
        {
            AddNormalLine("RawIron", 1);
        }
    }
    /// <summary>
    /// 通过NameTmp添加原料,添加成功返回true，否则返回false
    /// </summary>
    public bool AddNormalLine(string NameTmp, float length)
    {
        if (TemplateStoreManager.Instance["Item"][NameTmp] == null) { return false; } // 如果没找到返回false
        Debug.Log("FindTemplate");
        // 如果已经在库中，则数量+1
        for (int i = 0; i < linesItemStore.Count; i++)
        {
            if (linesItemStore[i].GetText("NameTmp") == NameTmp)
            {
                linesItemStore[i].CreateAttribute("Length", linesItemStore[i].GetFloat("Length") + length);
                GetLineScript(i).RefreshInfo();
                return true;
            }
        }
        //创建新的物品，并初始化
        linesItemStore.AddItem(new ItemVariable(TemplateStoreManager.Instance["Item"][NameTmp], true));
        ItemVariable newLineItem = linesItemStore[linesItemStore.Count - 1];

        newLineItem.CreateAttribute("Length", length);
        newLineItem.CreateAttribute("Name", newLineItem.GetText("Name", false));

        GameObject newLineObject = Instantiate(LinePrefab);
        lines.Add(newLineObject);
        newLineObject.transform.SetParent(LinesParentTrans);
        newLineObject.transform.localScale = Vector3.one;

        LineScript newLineScript = newLineObject.GetComponent<LineScript>();
        newLineScript.LineItem = newLineItem;
        newLineScript.RefreshInfo();

        return true;
    }
    /// <summary>
    /// 通过ID添加原料,添加成功返回true，否则返回false
    /// </summary>
    public bool AddNormalLine(int id, float length)
    {

        if (TemplateStoreManager.Instance["Item"][id] == null) { return false; }// 如果没找到返回false

        // 如果已经在库中，则数量+1
        for (int i = 0; i < linesItemStore.Count; i++)
        {
            if (linesItemStore[i].GetInt("ID") == id)
            {
                linesItemStore[i].CreateAttribute("Length", linesItemStore[i].GetFloat("Length") + length);
                GetLineScript(i).RefreshInfo();
                return true;
            }
        }

        //创建新的物品，并初始化
        linesItemStore.AddItem(new ItemVariable(TemplateStoreManager.Instance["Item"][id], true));
        ItemVariable newLineItem = linesItemStore[linesItemStore.Count - 1];

        newLineItem.CreateAttribute("Length", length);
        newLineItem.CreateAttribute("Name", newLineItem.GetText("Name", false));

        GameObject newLineObject = Instantiate(LinePrefab);
        lines.Add(newLineObject);
        newLineObject.transform.SetParent(LinesParentTrans);
        newLineObject.transform.localScale = Vector3.one;

        LineScript newLineScript = newLineObject.GetComponent<LineScript>();
        newLineScript.LineItem = newLineItem;
        newLineScript.RefreshInfo();
        return true;
    }


    public void FixedLineObjects()
    {
        for (int index = 0; index < lines.Count; index++)
        {
            ItemVariable item = lines[index].GetComponent<LineScript>().LineItem;
            if (item.GetFloat("Length") == 0)
            {
                linesItemStore.RemoveItem(index);
                Destroy(lines[index]);
                lines.RemoveAt(index);
            }
        }
    }
    /// <summary>
    /// 通过索引获取材料脚本
    /// </summary>
    /// <param name="index">索引</param>
    /// <returns>索引对应脚本</returns>
    public LineScript GetLineScript(int index)
    {
        if (index > (LinesParentTrans.childCount - 1))
        {
            return null;
        }
        return LinesParentTrans.GetChild(index).GetComponent<LineScript>();
    }
    /// <summary>
    /// 隐藏材料面板
    /// </summary>
    public void HideLinePanel()
    {
        oldVec = chooseLineTransform.localPosition;
        targetVec = new Vector3(-400, 0, 0);
    }
    /// <summary>
    /// 显示材料面板
    /// </summary>
    public void ShowLinePanel()
    {
        oldVec = chooseLineTransform.localPosition;
        targetVec = Vector3.zero;
    }

    /// <summary>
    /// 点击显示更多按钮事件
    /// </summary>
    public void ShowMoreButtonClick()
    {
        oldVec = chooseLineTransform.localPosition;
        if (ChooseLineSystem.Instance.showMore)
        {
            targetVec = Vector3.zero;
            LinesLayout.constraintCount = 1;
            ChooseLineSystem.Instance.showMore = false;
        }
        else
        {
            targetVec = new Vector3(400, 0, 0);
            LinesLayout.constraintCount = 3;
            ChooseLineSystem.Instance.showMore = true;
        }
    }
}
