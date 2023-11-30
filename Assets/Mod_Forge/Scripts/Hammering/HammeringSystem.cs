using UnityEngine;
using UnityEngine.UI;

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
    public GameObject ChooseMaterialPanel, Slider;
    private ER.Items.ItemVariable newItem;
    private int HitTimes;
    private bool isNewMaterial;

    private void Awake()
    {
        //构筑单例，并初始化
        if (instance == null)
        {
            instance = this;
            materialScripts = new MaterialScript[3];
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartHammering();
        }

        if (temperature > 0)
        {
            Slider.SetActive(false);
            ChooseMaterialPanel.SetActive(false);
        }
        else
        {
            Slider.SetActive(true);
            ChooseMaterialPanel.SetActive(true);
        }

        if (temperature < 0)
        {
            temperature = 0;
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

        if ((AddedMaterialNum < 3) && canFindMaterial)
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
        materialScript.MaterialItem.CreateAttribute("Num", materialScript.MaterialItem.GetInt("Num") - 1);
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
            MaterialSystem.Instance.AddForgedMaterial(newItem);
            materialScripts[id] = null;
            materialInFurnaces[id].SetActive(false);
            isNewMaterial = false;
            AddedMaterialNum = 0;
            startHammering = false;
        }
        else
        {
            materialScripts[id].MaterialItem.CreateAttribute("Num", materialScripts[id].MaterialItem.GetInt("Num") + 1);
            materialScripts[id] = null;
            materialInFurnaces[id].SetActive(false);
            AddedMaterialNum--;
            materialScripts[id].RefreshInfo();
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
        foreach (MaterialScript materialScript in materialScripts)
        {
            if ((materialScript != null) && (materialScript.MaterialItem.GetInt("ForgeTemp", true) > materialScript.MaterialItem.GetFloat("Temperature", true))) { return; }
        }
        newItem = materialScripts[0].MaterialItem.Clone();
        startHammering = true;
        isNewMaterial = true;
        HitTimes = 0;
        QTE.Instance.StartQTE();
    }

    /// <summary>
    /// 结束锻造
    /// </summary>
    public void FinishHammering()
    {
        temperature = 0;

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
    public void HammerMaterial(float QTEPerf)
    {
        if (AddedMaterialNum > 1)
        {
            newItem = SynthesisMaterial();
        }
        else
        {
            newItem = ForgedMaterial(QTEPerf);
        }
    }

    /// <summary>
    /// 锻造数值变换
    /// </summary>
    private ER.Items.ItemVariable ForgedMaterial(float QTEPerf)
    {
        ER.Items.ItemVariable item = newItem.Clone();
        float n = Mathf.Floor(HitTimes / 5);
        float Pref = QTEPerf;
        float FbR = (1 + (2 * newItem.GetFloat("Temperature", true) - newItem.GetInt("ForgeTemp")) * (newItem.GetInt("MeltTemp") - newItem.GetFloat("Temperature", true))) * 25 / (Mathf.Pow((2 * newItem.GetInt("MeltTemp") - newItem.GetInt("ForgeTemp")), 2) * 32);
        float deltaFb = (4 / (n + 2) - (1 * (1 - FbR))) * Pref;
        float ThR = Mathf.Pow((newItem.GetFloat("Temperature", true) * newItem.GetFloat("Temperature", true) / newItem.GetInt("ForgeTemp") / newItem.GetInt("MeltTemp")), newItem.GetFloat("HeatPreference"));
        float deltaTh = Mathf.Pow((newItem.GetFloat("Toughness") * (1 + newItem.GetFloat("Pressability")) / newItem.GetFloat("Toughness", true)), ((1 + n - newItem.GetFloat("Stubborn")) / 2)) * ThR * Pref;
        float AtsR = FbR;
        float deltaAts = newItem.GetFloat("AntiSolution") * (1 + newItem.GetFloat("AtsGrowth")) / (2 * newItem.GetFloat("AntiSolution", true)) * AtsR * Pref;
        item.CreateAttribute("Flexability", item.GetFloat("Flexability", true) + FbR);
        item.CreateAttribute("Toughness", item.GetFloat("Toughness", true) + ThR);
        item.CreateAttribute("AntiSolution", item.GetFloat("AntiSolution", true) + AtsR);
        HitTimes++;
        item.CreateAttribute("IsForged", true);

        Debug.Log("Flexability=" + item.GetFloat("Flexability", true) + " ,Toughness=" + item.GetFloat("Toughness", true) + " ,AntiSolution=" + item.GetFloat("AntiSolution", true) + " ,HitTimes=" + HitTimes + ", IsForged=" + item.GetBool("IsForged", true));

        return item.Clone();
    }

    /// <summary>
    /// 叠锻数值变换
    /// </summary>
    private ER.Items.ItemVariable SynthesisMaterial()
    {
        return null;
    }
}