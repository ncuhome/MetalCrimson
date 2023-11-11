using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputSystem : MonoBehaviour
{
    #region 单例封装

    private static MouseInputSystem instance;

    public static MouseInputSystem Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion 单例封装

    public bool mouse0, mouse0down, mouse0up;
    public bool mouse1, mouse1down, mouse1up;
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
        mouse0 = Input.GetMouseButton(0);
        mouse0down = Input.GetMouseButtonDown(0);
        mouse0up = Input.GetMouseButtonUp(0);
        mouse1 = Input.GetMouseButton(1);
        mouse1down = Input.GetMouseButtonDown(1);
        mouse1up = Input.GetMouseButtonUp(1);
    }
}
