using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum ForgeCompleteness
{
    None, Bad, Great, Perfect, Legend
}
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
    /// 最大添加的材料数
    /// </summary>
    public int MaxMaterialNum = 1;
    /// <summary>
    /// 添加的材料对应的材料脚本
    /// </summary>
    public MaterialScript[] materialScripts = null;
    /// <summary>
    /// 炉子里的材料
    /// </summary>
    public GameObject[] materialInFurnaces = null;
    public Material glowMaterial1, glowMaterial2;
    public int chainTimes = 0;
    public bool startHammering;
    public GameObject ChooseMaterialPanel, Slider;
    private int HitTimes;
    private bool isNewMaterial;
    public Animator fireAnimator;
    public ForgeCompleteness forgeCompleteness;
    public float temperature;
    readonly string[] temperatureString = { "DarkRed", "Red", "LightRed", "Orange", "Yellow", "LightYellow" };
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

        if (chainTimes > 0)
        {
            Slider.SetActive(false);
            ChooseMaterialPanel.SetActive(false);
            UIManager.Instance.ReturnButton.interactable = false;
            UIManager.Instance.CancelButton.interactable = false;
        }
        else
        {
            Slider.SetActive(true);
            ChooseMaterialPanel.SetActive(true);
            UIManager.Instance.ReturnButton.interactable = true;
            UIManager.Instance.CancelButton.interactable = true;
        }

    }
    /// <summary>
    /// 判断是否能添加材料
    /// </summary>
    public bool AddMaterialJudgement(MaterialScript materialScript, out bool canFindMaterial)
    {
        if (materialScript == null)
        {
            canFindMaterial = false;
        }
        else
        {
            canFindMaterial = true;
        }

        if ((AddedMaterialNum < MaxMaterialNum) && canFindMaterial && (materialScript.MaterialItem.GetText("Tags") != "Product"))
        {
            AddMaterial(materialScript);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 添加材料
    /// </summary>
    private void AddMaterial(MaterialScript materialScript)
    {
        materialScripts[AddedMaterialNum] = materialScript;
        materialInFurnaces[AddedMaterialNum].SetActive(true);
        materialScript.MaterialItem.CreateAttribute("Num", materialScript.MaterialItem.GetFloat("Num") - 1);
        AddedMaterialNum++;
        materialScript.RefreshInfo();
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
    public bool MoveBackMaterial(int id)
    {
        if (id >= AddedMaterialNum) { return false; }
        Debug.Log("MoveBack");
        if (isNewMaterial)
        {
            MaterialSystem.Instance.AddForgedMaterial(materialScripts[0].MaterialItem.GetText("NameTmp"), forgeCompleteness);
            materialScripts[id] = null;
            materialInFurnaces[id].SetActive(false);
            isNewMaterial = false;
            AddedMaterialNum = 0;
            startHammering = false;
        }
        else
        {
            materialScripts[id].MaterialItem.CreateAttribute("Num", materialScripts[id].MaterialItem.GetFloat("Num") + 1);
            materialInFurnaces[id].SetActive(false);
            AddedMaterialNum--;
            materialScripts[id].RefreshInfo();
            materialScripts[id] = null;
        }
        FixMaterials();
        return true;
    }

    /// <summary>
    /// 开始锻造
    /// </summary>
    public void StartHammering()
    {
        if (startHammering) { return; }
        if (AddedMaterialNum == 0) { return; }
        if (chainTimes == 0) { return; }
        startHammering = true;
        isNewMaterial = true;
        HitTimes = 0;
        forgeCompleteness = 0;
        //QTE.Instance.StartQTE();
    }

    /// <summary>
    /// 结束锻造
    /// </summary>
    public void FinishHammering()
    {
        if (AddedMaterialNum > 1)
        {

        }
        else
        {
            materialInFurnaces[0].GetComponent<Image>().material = null;
        }
        MaterialSystem.Instance.FixedMaterialOjects();
    }

    /// <summary>
    /// 进行材料的公式计算
    /// </summary>
    public void HammerMaterial()
    {
        if (AddedMaterialNum > 1)
        {

        }
        else
        {
            forgeCompleteness = ForgedMaterial();
            chainTimes = 0;
            Debug.Log(forgeCompleteness);
        }
    }

    /// <summary>
    /// 锻造数值变换
    /// </summary>
    private ForgeCompleteness ForgedMaterial()
    {
        if (GetTemperature().Equals(materialScripts[0].MaterialItem.GetText("SuitableTemperature")))
        {
            Debug.Log(GetTemperature());
            if (QTE.Instance.QTEPerfect())
            {
                if (Random.value > 0.05f)
                {
                    return ForgeCompleteness.Legend;
                }
                else
                {
                    return ForgeCompleteness.Perfect;
                }
            }
        }
        if (GetTemperatures(GetTemperature()).Contains(materialScripts[0].MaterialItem.GetText("SuitableTemperature")))
        {
            Debug.Log(GetTemperatures(GetTemperature()));
            if (QTE.Instance.QTEFailed() <= 1)
            {
                return ForgeCompleteness.Great;
            }
            else
            {
                return ForgeCompleteness.Bad;
            }
        }
        if ((QTE.Instance.QTEFailed() > 2) || (!GetTemperatures(GetTemperature()).Contains(materialScripts[0].MaterialItem.GetText("SuitableTemperature"))))
        {
            return ForgeCompleteness.Bad;
        }
        return 0;
    }
    /// <summary>
    /// 叠锻数值变换
    /// </summary>
    private ER.Items.ItemVariable SynthesisMaterial()
    {

        return null;
    }

    public void StartFire()
    {
        fireAnimator.SetTrigger("fire");
        chainTimes++;
        StartCoroutine("DelayRefreshColor");
    }

    public IEnumerator DelayRefreshColor()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (var material in materialInFurnaces)
        {
            if (material.activeSelf)
            {
                material.GetComponent<materialInFurnace>().HDRScript.RefreshColor();
            }
        }
    }

    private string GetTemperature()
    {
        switch (chainTimes)
        {
            case 1:
            case 2:
                return "DarkRed";
            case 3:
            case 4:
                return "Red";
            case 5:
            case 6:
                return "LightRed";
            case 7:
            case 8:
                return "Orange";
            case 9:
                return "Yellow";
            case 10:
                return "LightYellow";
            default:
                return "Error";
        }
    }

    private string[] GetTemperatures(string tempString)
    {
        if (tempString.Equals(temperatureString[0]))
        {
            return new string[2] { temperatureString[0], temperatureString[1] };
        }
        for (int i = 1; i < temperatureString.Length - 1; i++)
        {
            if (tempString.Equals(temperatureString[i]))
            {
                return new string[3] { temperatureString[i - 1], temperatureString[i], temperatureString[i + 1] };
            }
        }
        if (tempString.Equals(temperatureString[temperatureString.Length - 1]))
        {
            return new string[2] { temperatureString[temperatureString.Length - 2], temperatureString[temperatureString.Length - 1] };
        }
        return null;
    }
}
