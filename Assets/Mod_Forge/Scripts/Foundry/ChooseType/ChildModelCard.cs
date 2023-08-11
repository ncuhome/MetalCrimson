using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ChildModelCard : MonoBehaviour
{
    public ChildType childType;
    public TMP_Text description;
    public TMP_Text costMaterial;
    public Bar sharpness;
    public Bar durability;
    public Bar weight;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RefreshCard()
    {
        description.text = childType.Description;
        costMaterial.text = "耗费材料：" + childType.costMaterialNum.ToString();
        sharpness.barNum = Mathf.FloorToInt(childType.sharpness * 10);
        durability.barNum = Mathf.FloorToInt(childType.durability * 10);
        weight.barNum = Mathf.FloorToInt(childType.weight * 10);
        sharpness.RefreshBar();
        durability.RefreshBar();
        weight.RefreshBar();
    }
}
