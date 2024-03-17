using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ER.Items;
public class WeaponSystem : MonoBehaviour
{

    #region 单例封装

    private static WeaponSystem instance;

    public static WeaponSystem Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion 单例封装

    public ItemStore weaponItemStore;

    public List<Weapon> weapons;

    public Transform weaponBuildTrans;

    public Transform weaponPlaceTrans;

    public GameObject weaponPrefab;

    public Weapon currentWeapon;

    public WeaponInfoPanel weaponInfoPanel;

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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitWeaponSystem()
    {
        InitWeaponItemStore();
    }

    private void InitWeaponItemStore()
    {
        ItemStoreManager.Instance.Create("weaponItemStore");
        weaponItemStore = ItemStoreManager.Instance.Stores["componentItemStore"];
        weapons = new List<Weapon>();
        WeaponBuild();
    }

    public void WeaponBuild()
    {
        //创建新的物品，并初始化
        weaponItemStore.AddItem(new ItemVariable(TemplateStoreManager.Instance["Item"]["Weapon"], true));
        currentWeapon = Instantiate(weaponPrefab, weaponBuildTrans).GetComponent<Weapon>();
        currentWeapon.weaponItem = weaponItemStore[weaponItemStore.Count - 1];
        ComponentSystem.Instance.AnvilTrans = currentWeapon.transform;

        currentWeapon.weaponItem.CreateAttribute("AntiSolution", 0f);
        currentWeapon.weaponItem.CreateAttribute("M", 0f);
        currentWeapon.weaponItem.CreateAttribute("Dur", 0f);
        currentWeapon.weaponItem.CreateAttribute("Sharp", 0f);
        currentWeapon.weaponItem.CreateAttribute("TotalPretty", 0f);
        currentWeapon.weaponItem.CreateAttribute("Pretty", 0f);


        RefreshWeaponInfo();

        Debug.Log("WeaponBuild");
    }

    public void RefreshWeaponInfo()
    {
        weaponInfoPanel.weapon = currentWeapon;
        weaponInfoPanel.RefreshInfo();
        TetherSystem.Instance.RefreshTether();
    }

    public void FinishWeaponBuild()
    {
        if (ComponentSystem.Instance.outPort || ComponentSystem.Instance.inPort || (ComponentSystem.Instance.componentInAnvil.Count == 0)) { return; }
        currentWeapon.FinishBuild();
        weapons.Add(currentWeapon);
        currentWeapon.transform.SetParent(weaponPlaceTrans);
        ComponentSystem.Instance.FinishBuild();
        WeaponBuild();
    }



    public void AddAttribute(ComponentScript componentScript)
    {
        // currentWeapon.weaponItem.CreateAttribute("AntiSolution", currentWeapon.weaponItem.GetFloat("AntiSolution") + componentScript.ComponentItem.GetFloat("AntiSolution"));
        // currentWeapon.weaponItem.CreateAttribute("M", currentWeapon.weaponItem.GetFloat("M") + componentScript.ComponentItem.GetFloat("M"));
        // currentWeapon.weaponItem.CreateAttribute("Dur", currentWeapon.weaponItem.GetFloat("Dur") + componentScript.ComponentItem.GetFloat("Dur"));
        // currentWeapon.weaponItem.CreateAttribute("Sharp", currentWeapon.weaponItem.GetFloat("Sharp") + componentScript.ComponentItem.GetFloat("Sharp"));
        // currentWeapon.weaponItem.CreateAttribute("TotalPretty", currentWeapon.weaponItem.GetFloat("TotalPretty") + componentScript.ComponentItem.GetFloat("Pretty"));
        // if (ComponentSystem.Instance.componentInAnvil.Count > 0)
        // {
        //     currentWeapon.weaponItem.CreateAttribute("Pretty", currentWeapon.weaponItem.GetFloat("TotalPretty") / ComponentSystem.Instance.componentInAnvil.Count);
        // }
        // else
        // {
        //     currentWeapon.weaponItem.CreateAttribute("Pretty", 0f);
        // }
    }

    public void RemoveAttribute(ComponentScript componentScript)
    {
        // currentWeapon.weaponItem.CreateAttribute("AntiSolution", currentWeapon.weaponItem.GetFloat("AntiSolution") - componentScript.ComponentItem.GetFloat("AntiSolution"));
        // currentWeapon.weaponItem.CreateAttribute("M", currentWeapon.weaponItem.GetFloat("M") - componentScript.ComponentItem.GetFloat("M"));
        // currentWeapon.weaponItem.CreateAttribute("Dur", currentWeapon.weaponItem.GetFloat("Dur") - componentScript.ComponentItem.GetFloat("Dur"));
        // currentWeapon.weaponItem.CreateAttribute("Sharp", currentWeapon.weaponItem.GetFloat("Sharp") - componentScript.ComponentItem.GetFloat("Sharp"));
        // currentWeapon.weaponItem.CreateAttribute("TotalPretty", currentWeapon.weaponItem.GetFloat("TotalPretty") - componentScript.ComponentItem.GetFloat("Pretty"));
        // if (ComponentSystem.Instance.componentInAnvil.Count > 0)
        // {
        //     currentWeapon.weaponItem.CreateAttribute("Pretty", currentWeapon.weaponItem.GetFloat("TotalPretty") / ComponentSystem.Instance.componentInAnvil.Count);
        // }
        // else
        // {
        //     currentWeapon.weaponItem.CreateAttribute("Pretty", 0f);
        // }
    }
}
