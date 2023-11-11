using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WeaponInfoPanel : MonoBehaviour
{
    public Weapon weapon;
    public TMP_Text Durability;
    public TMP_Text Sharpness;
    public TMP_Text Weight;
    public TMP_Text AntiSolution;
    public TMP_Text Pretty;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RefreshInfo()
    {
        Durability.text = "耐用度：" + weapon.weaponItem.GetFloat("Dur");
        Sharpness.text = "锋利度：" + weapon.weaponItem.GetFloat("Sharp");
        Weight.text = "重量：" + weapon.weaponItem.GetFloat("M");
        AntiSolution.text = "抗腐蚀性：" + weapon.weaponItem.GetFloat("AntiSolution");
        Pretty.text = "美观度：" + weapon.weaponItem.GetFloat("Pretty");
    }
}
