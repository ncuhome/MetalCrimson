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
    private Color c;

    // Start is called before the first frame update
    void Start()
    {
        c = HammeringSystem.Instance.glowMaterial.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (HammeringSystem.Instance.temperature > 0)
        {
            outline.enabled = false;
            showOutLine.enabled = false;
            if (HammeringSystem.Instance.temperature > materialScript.MaterialItem.GetInt("ForgeTemp"))
            {
                materialImage.material = HammeringSystem.Instance.glowMaterial;
                float intensity = (HammeringSystem.Instance.temperature - 150f) / 35f;
                float factor = Mathf.Pow(2, intensity);
                materialImage.material.color = new Color(c.r * factor, c.g * factor, c.b * factor);
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
        HammeringSystem.Instance.glowMaterial.color = c;
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
