using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public ER.Items.ItemStore materialsItemStore = null;
    /// <summary>
    /// 材料物体组
    /// </summary>
    private List<GameObject> materials = null;
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
        InitMaterialItemStore();
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 初始化材料物品库设置
    /// </summary>
    private void InitMaterialItemStore()
    {

        materials = new List<GameObject>();

        for (int i = 0; i < 6; i++)
        {
            AddNormalMaterial("RawIron");
        }
    }
    /// <summary>
    /// 通过NameTmp添加原料,添加成功返回true，否则返回false
    /// </summary>
    public bool AddNormalMaterial(string NameTmp)
    {
        ER.Items.ItemTemplateStore.Instance.LoadItemsList(@"Assets/StreamingAssets/材料信息表.csv");

        if (ER.Items.ItemTemplateStore.Instance[NameTmp] == null) { return false; } // 如果没找到返回false

        // 如果已经在库中，则数量+1
        for (int i = 0; i < materialsItemStore.Count; i++)
        {
            if (materialsItemStore[i].GetBool("IsForged"))
            {
                continue;
            }
            if (materialsItemStore[i].GetText("NameTmp") == NameTmp)
            {
                materialsItemStore[i].CreateAttribute("Num", materialsItemStore[i].GetInt("Num") + 1);
                return true;
            }
        }

        //创建新的物品，并初始化
        materialsItemStore.AddItem(new ER.Items.ItemVariable(ER.Items.ItemTemplateStore.Instance[NameTmp], true));
        ER.Items.ItemVariable newMaterialItem = materialsItemStore[materialsItemStore.Count - 1];

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

        return true;
    }
    /// <summary>
    /// 通过ID添加原料,添加成功返回true，否则返回false
    /// </summary>
    public bool AddNormalMaterial(int id)
    {
        ER.Items.ItemTemplateStore.Instance.LoadItemsList(@"Assets/StreamingAssets/材料信息表.csv");

        if (ER.Items.ItemTemplateStore.Instance[id] == null) { return false; }// 如果没找到返回false

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
                return true;
            }
        }

        //创建新的物品，并初始化
        materialsItemStore.AddItem(new ER.Items.ItemVariable(ER.Items.ItemTemplateStore.Instance[id], true));
        ER.Items.ItemVariable newMaterialItem = materialsItemStore[materialsItemStore.Count - 1];

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
        return true;
    }

    public bool AddForgedMaterial(ER.Items.ItemVariable forgedItem)
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
        return true;
    }

    public void FixedMaterialOjects()
    {
        for (int index = 0; index < materials.Count; index++)
        {
            ER.Items.ItemVariable item = materials[index].GetComponent<MaterialScript>().MaterialItem;
            if (item.GetInt("Num") == 0)
            {
                materialsItemStore.RemoveItem(index);
                Destroy(materials[index]);
                materials.RemoveAt(index);
            }
        }
    }

    public MaterialScript GetMaterialScript(int index)
    {
        if (index > (materialsParentTrans.childCount - 1))
        {
            return null;
        }
        return materialsParentTrans.GetChild(index).GetComponent<MaterialScript>();
    }
}
