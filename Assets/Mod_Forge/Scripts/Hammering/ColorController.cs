using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    #region 单例封装

    private static ColorController instance;

    public static ColorController Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion 单例封装

    public Color[] colors;
    public Material[] HDRcolors;

    void Awake()
    {
        //构筑单例，并初始化
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
}
