using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class materialInFurnace : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// 在炉子里的顺序ID
    /// </summary>
    public int ID;
    private MaterialScript materialScript;
    private Outline outline;
    private ShowOutLine showOutLine;
    private Image materialImage;
    private Color HDRColor;
    private Color c;
    private float t;

    // Start is called before the first frame update
    void Start()
    {
        HDRColor = HammeringSystem.Instance.glowMaterial.color;
        c = materialImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (HammeringSystem.Instance.temperature > 0)
        {
            outline.enabled = false;
            showOutLine.enabled = false;
            materialImage.material = HammeringSystem.Instance.glowMaterial;

            if (!HammeringSystem.Instance.startHammering)
            {
                t += Time.deltaTime;
                if (t > 0.2f)
                {
                    UpdateTemperature();
                    t = 0;
                }
            }

            float materialForgeTemp = materialScript.MaterialItem.GetInt("ForgeTemp", true);
            float materialTemperature = materialScript.MaterialItem.GetFloat("Temperature", true);
            if (materialTemperature > materialForgeTemp)
            {
                float intensity = (materialTemperature - materialForgeTemp) / (500f - materialForgeTemp) * 5;
                float factor = Mathf.Pow(2, intensity);
                materialImage.material.color = new Color(HDRColor.r * factor, HDRColor.g * factor, HDRColor.b * factor);
            }
            else
            {
                float t = materialTemperature / materialForgeTemp;
                Color color = Color.Lerp(c, HDRColor, t);
                materialImage.material.color = color;
            }
        }
        else
        {
            outline.enabled = true;
            showOutLine.enabled = true;
        }
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        materialScript = HammeringSystem.Instance.materialScripts[ID];
        outline = GetComponent<Outline>();
        showOutLine = GetComponent<ShowOutLine>();
        materialImage = GetComponent<Image>();
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        HammeringSystem.Instance.glowMaterial.color = HDRColor;
    }
    /// <summary>
    /// 点击时返还材料
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (HammeringSystem.Instance.temperature > 0) { return; }
        HammeringSystem.Instance.MoveBackMaterial(ID);
    }

    private void UpdateTemperature()
    {
        float materialTemperature = materialScript.MaterialItem.GetFloat("Temperature", true);
        float materialHeatPassage = materialScript.MaterialItem.GetFloat("HeatPassage", true);
        float materialHeatContain = materialScript.MaterialItem.GetFloat("HeatContain", true);
        float dT = materialHeatPassage * (HammeringSystem.Instance.temperature - materialTemperature) * 0.2f / materialHeatContain;
        materialScript.MaterialItem.CreateAttribute("Temperature", materialTemperature + dT);
        Debug.Log("Temperature=" + materialScript.MaterialItem.GetFloat("Temperature"));
    }
}
