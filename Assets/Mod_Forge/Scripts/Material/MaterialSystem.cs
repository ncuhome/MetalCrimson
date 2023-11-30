using ER.Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSystem : MonoBehaviour
{
    #region 单例封装

    private static MaterialSystem instance;

    public static MaterialSystem Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion 单例封装

    /// <summary>
    /// 材料父物体Transform组件
    /// </summary>
    public Transform materialsParentTrans = null;

    /// <summary>
    /// 材料预制件
    /// </summary>
    public GameObject materialPrefab = null;

    /// <summary>
    /// 材料物品库
    /// </summary>
    public ItemStore materialsItemStore = null;

    /// <summary>
    /// 材料物体组
    /// </summary>
    public List<GameObject> materials = null;

    /// <summary>
    /// 选择界面的Transform组件
    /// </summary>
    public Transform chooseMaterialTransform = null;

    /// <summary>
    /// 材料组对应的Layout组件
    /// </summary>
    public GridLayoutGroup materialLayout = null;

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

    public int needMaterialNum;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        InitMaterialItemStore();
        targetVec = chooseMaterialTransform.localPosition;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Vector3.Magnitude(chooseMaterialTransform.localPosition - targetVec) < 0.5f)
        {
            chooseMaterialTransform.localPosition = targetVec;
            moveTime = 0f;
            move = false;
        }
        else
        {
            move = true;
            moveTime += Time.deltaTime * 5;
            Vector3 newVec = Vector3.Lerp(oldVec, targetVec, moveTime);
            chooseMaterialTransform.localPosition = newVec;
        }
    }

    /// <summary>
    /// 初始化材料物品库设置
    /// </summary>
    private void InitMaterialItemStore()
    {
        ItemStoreManager.Instance.Create("materialItemStore");
        materialsItemStore = ItemStoreManager.Instance.Stores["materialItemStore"];
        materials = new List<GameObject>();

        for (int i = 0; i < 6; i++)
        {
            Debug.Log("1");
            Debug.Log(AddNormalMaterial("RawIron"));
        }
    }

    /// <summary>
    /// 通过NameTmp添加原料,添加成功返回true，否则返回false
    /// </summary>
    public bool AddNormalMaterial(string NameTmp)
    {
        if (TemplateStoreManager.Instance["Item"][NameTmp] == null) { return false; } // 如果没找到返回false
        Debug.Log("FindTemplate");
        // 如果已经在库中，则数量+1
        for (int i = 0; i < materialsItemStore.Count; i++)
        {
            if ((materialsItemStore[i].GetText("NameTmp") == NameTmp) && (!materialsItemStore[i].GetBool("IsForged")))
            {
                materialsItemStore[i].CreateAttribute("Num", materialsItemStore[i].GetInt("Num") + 1);
                GetMaterialScript(i).RefreshInfo();
                return true;
            }
        }
        //创建新的物品，并初始化
        materialsItemStore.AddItem(new ItemVariable(TemplateStoreManager.Instance["Item"][NameTmp], true));
        ItemVariable newMaterialItem = materialsItemStore[materialsItemStore.Count - 1];

        newMaterialItem.CreateAttribute("Num", 1);
        newMaterialItem.CreateAttribute("IsForged", false);
        newMaterialItem.CreateAttribute("Name", newMaterialItem.GetText("Name", false));
        newMaterialItem.CreateAttribute("Temperature", 0f);

        GameObject newMaterialObject = Instantiate(materialPrefab);
        materials.Add(newMaterialObject);
        newMaterialObject.transform.SetParent(materialsParentTrans);
        newMaterialObject.transform.localScale = Vector3.one;

        MaterialScript newMaterialScript = newMaterialObject.GetComponent<MaterialScript>();
        newMaterialScript.MaterialItem = newMaterialItem;
        newMaterialScript.RefreshInfo();

        return true;
    }

    /// <summary>
    /// 通过ID添加原料,添加成功返回true，否则返回false
    /// </summary>
    public bool AddNormalMaterial(int id)
    {
        if (TemplateStoreManager.Instance["Item"][id] == null) { return false; }// 如果没找到返回false

        // 如果已经在库中，则数量+1
        for (int i = 0; i < materialsItemStore.Count; i++)
        {
            if (materialsItemStore[i].GetBool("IsForged"))
            {
                continue;
            }
            if (materialsItemStore[i].GetInt("ID") == id)
            {
                materialsItemStore[i].CreateAttribute("Num", materialsItemStore[i].GetInt("Num") + 1);
                GetMaterialScript(i).RefreshInfo();
                return true;
            }
        }

        //创建新的物品，并初始化
        materialsItemStore.AddItem(new ItemVariable(TemplateStoreManager.Instance["Item"][id], true));
        ItemVariable newMaterialItem = materialsItemStore[materialsItemStore.Count - 1];

        newMaterialItem.CreateAttribute("Num", 1);
        newMaterialItem.CreateAttribute("IsForged", false);
        newMaterialItem.CreateAttribute("Name", newMaterialItem.GetText("Name", false));
        newMaterialItem.CreateAttribute("Temperature", 0f);

        GameObject newMaterialObject = Instantiate(materialPrefab);
        materials.Add(newMaterialObject);
        newMaterialObject.transform.SetParent(materialsParentTrans);
        newMaterialObject.transform.localScale = Vector3.one;

        MaterialScript newMaterialScript = newMaterialObject.GetComponent<MaterialScript>();
        newMaterialScript.MaterialItem = newMaterialItem;
        newMaterialScript.RefreshInfo();
        return true;
    }

    public bool AddForgedMaterial(ItemVariable forgedItem)
    {
        forgedItem.CreateAttribute("Num", 1);
        forgedItem.CreateAttribute("Temperature", 0f);
        materialsItemStore.AddItem(forgedItem);
        GameObject forgedMaterialObject = Instantiate(materialPrefab);
        materials.Add(forgedMaterialObject);
        forgedMaterialObject.transform.SetParent(materialsParentTrans);
        forgedMaterialObject.transform.localScale = Vector3.one;

        MaterialScript forgedMaterialScript = forgedMaterialObject.GetComponent<MaterialScript>();
        forgedMaterialScript.MaterialItem = forgedItem;
        forgedMaterialScript.RefreshInfo();
        return true;
    }

    public void FixedMaterialOjects()
    {
        for (int index = 0; index < materials.Count; index++)
        {
            ItemVariable item = materials[index].GetComponent<MaterialScript>().MaterialItem;
            if (item.GetInt("Num") == 0)
            {
                materialsItemStore.RemoveItem(index);
                Destroy(materials[index]);
                materials.RemoveAt(index);
            }
        }
    }

    /// <summary>
    /// 通过索引获取材料脚本
    /// </summary>
    /// <param name="index">索引</param>
    /// <returns>索引对应脚本</returns>
    public MaterialScript GetMaterialScript(int index)
    {
        if (index > (materialsParentTrans.childCount - 1))
        {
            return null;
        }
        return materialsParentTrans.GetChild(index).GetComponent<MaterialScript>();
    }

    /// <summary>
    /// 隐藏材料面板
    /// </summary>
    public void HideMaterialPanel()
    {
        oldVec = chooseMaterialTransform.localPosition;
        targetVec = new Vector3(0, -400, 0);
    }

    /// <summary>
    /// 显示材料面板
    /// </summary>
    public void ShowMaterialPanel()
    {
        oldVec = chooseMaterialTransform.localPosition;
        targetVec = Vector3.zero;
    }

    /// <summary>
    /// 点击显示更多按钮事件
    /// </summary>
    public void ShowMoreButtonClick()
    {
        oldVec = chooseMaterialTransform.localPosition;
        if (MaterialChooseSystem.Instance.showMore)
        {
            targetVec = Vector3.zero;
            materialLayout.constraintCount = 1;
            MaterialChooseSystem.Instance.showMore = false;
        }
        else
        {
            targetVec = new Vector3(0, 616, 0);
            materialLayout.constraintCount = 3;
            MaterialChooseSystem.Instance.showMore = true;
        }
    }
}