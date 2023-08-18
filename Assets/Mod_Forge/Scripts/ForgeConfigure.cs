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
    }
}
