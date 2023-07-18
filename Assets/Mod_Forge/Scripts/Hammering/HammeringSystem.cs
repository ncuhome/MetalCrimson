using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HammeringSystem : MonoBehaviour
{
    #region 单例封装

    private static HammeringSystem instance;

    public static HammeringSystem Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion 单例封装
    /// <summary>
    /// 已经添加的材料数
    /// </summary>
    public int AddedMaterialNum = 0;
    /// <summary>
    /// 添加的材料对应的材料脚本
    /// </summary>
    public MaterialScript[] materialScripts = null;
    /// <summary>
    /// 炉子里的材料
    /// </summary>
    public GameObject[] materialInFurnaces = null;
    public Material glowMaterial;
    public float temperature = 0f;
    public bool startHammering;
    void Awake()
    {
        //构筑单例，并初始化
        if (instance == null)
        {
            instance = this;
            materialScripts = new MaterialScript[3];
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 判断是否能添加材料
    /// </summary>
    public void AddMaterialJudgement(MaterialScript materialScript)
    {
        if (AddedMaterialNum < 3)
        {
            AddMaterial(materialScript);
        }
    }
    /// <summary>
    /// 添加材料
    /// </summary>
    private void AddMaterial(MaterialScript materialScript)
    {
        materialScripts[AddedMaterialNum] = materialScript;
        materialInFurnaces[AddedMaterialNum].SetActive(true);
        materialScript.MaterialItem.CreateAttribute("Num", materialScript.MaterialItem.GetInt("Num") - 1);
        AddedMaterialNum++;
    }
    /// <summary>
    /// 放回材料后修复顺序
    /// </summary>
    private void FixMaterials()
    {
        for (int i = 0; i < 2; i++)
        {
            if ((materialScripts[i] == null) && (materialScripts[i + 1] != null))
            {
                materialScripts[i] = materialScripts[i + 1];
                materialInFurnaces[i].SetActive(true);
                materialScripts[i + 1] = null;
                materialInFurnaces[i + 1].SetActive(false);
            }
        }
    }
    /// <summary>
    /// 放回材料
    /// </summary>
    public void MoveBackMaterial(int id)
    {
        materialScripts[id].MaterialItem.CreateAttribute("Num", materialScripts[id].MaterialItem.GetInt("Num") + 1);
        materialScripts[id] = null;
        materialInFurnaces[id].SetActive(false);
        AddedMaterialNum--;
        FixMaterials();
    }

    public void StartHammering()
    {
        if (startHammering) { return; }
        if (AddedMaterialNum == 0) { return; }
        foreach (MaterialScript materialScript in materialScripts)
        {
            if ((materialScript != null) && (materialScript.MaterialItem.GetInt("ForgeTemp", true) > temperature)) { return; }
        }
        startHammering = true;
    }
}
