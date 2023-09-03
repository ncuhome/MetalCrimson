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
        ItemStoreManager.Instance.Creat("weaponItemStore");
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
        Debug.Log("WeaponBuild");
    }

    public void RefreshWeaponInfo()
    {

    }

    public void FinishWeaponBuild()
    {
        if (ComponentSystem.Instance.outPort || ComponentSystem.Instance.inPort || (ComponentSystem.Instance.componentInAnvil.Count == 0)) { return; }
        weapons.Add(currentWeapon);
        currentWeapon.transform.SetParent(weaponPlaceTrans);
        ComponentSystem.Instance.FinishBuild();
        WeaponBuild();
    }
}
