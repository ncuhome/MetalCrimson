using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeConfigure : MonoBehaviour
{
    #region 单例封装
    private static ForgeConfigure instance;
    public static ForgeConfigure Instance { get { return instance; } }
    #endregion

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

    public void InitForgeManager()
    {
        MaterialSystem.Instance.InitMaterialSystem();
        ComponentSystem.Instance.InitComponentSystem();
        TypeSystem.Instance.InitTypeSystem();
        UIInputManager.Instance.InitInputManager();
        WeaponSystem.Instance.InitWeaponSystem();
    }

    public void EnterMaterialProgressing()
    {
        UIManager.Instance.materialProgressing.gameObject.SetActive(true);
        UIManager.Instance.chooseMaterial.gameObject.SetActive(true);
        UIManager.Instance.chooseCraft.gameObject.SetActive(false);
        MaterialSystem.Instance.ShowMaterialPanel();
    }

    public void ExitMaterialProgressing()
    {
        UIManager.Instance.materialProgressing.gameObject.SetActive(false);
        UIManager.Instance.chooseMaterial.gameObject.SetActive(false);
        UIManager.Instance.chooseCraft.gameObject.SetActive(true);
    }
    public void EnterFoundry()
    {
        UIManager.Instance.foundry.gameObject.SetActive(true);
        UIManager.Instance.chooseMaterial.gameObject.SetActive(true);
        UIManager.Instance.chooseCraft.gameObject.SetActive(false);
    }

    public void ExitFoundry()
    {
        UIManager.Instance.foundry.gameObject.SetActive(false);
        UIManager.Instance.chooseMaterial.gameObject.SetActive(false);
        UIManager.Instance.chooseCraft.gameObject.SetActive(true);
    }

    public void EnterComponentSplicing()
    {
        UIManager.Instance.componentSplicing.gameObject.SetActive(true);
        UIManager.Instance.chooseCraft.gameObject.SetActive(false);
    }

    public void ExitComponentSplicing()
    {
        UIManager.Instance.componentSplicing.gameObject.SetActive(false);
        UIManager.Instance.chooseCraft.gameObject.SetActive(true);
    }
}
