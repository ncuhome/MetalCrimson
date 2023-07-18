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
            float materialForgeTemp =  materialScript.MaterialItem.GetInt("ForgeTemp", true);
            if (HammeringSystem.Instance.temperature > materialForgeTemp)
            {
                float intensity = (HammeringSystem.Instance.temperature - materialForgeTemp) / (500f - materialForgeTemp) * 5;
                float factor = Mathf.Pow(2, intensity);
                materialImage.material.color = new Color(HDRColor.r * factor, HDRColor.g * factor, HDRColor.b * factor);
            }
            else
            {
                float t = (float)HammeringSystem.Instance.temperature / materialForgeTemp;
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
}
