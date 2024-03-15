using ER;
using ER.Resource;
using ER.Template;
using System;
using System.Security.Policy;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// 游戏初始化类:
/// 配置 GR 加载器
/// </summary>
public class GameInit_0 : MonoBehaviour,MonoInit
{
    public void Init()
    {
        GR.AddLoader(new SpriteLoader());
        GR.AddLoader(new TextLoader());
        GR.AddLoader(new AudioLoader());

        //配置文件加载器(由 TextLoader 改)
        TextLoader configLoader = new TextLoader();
        configLoader.Head = "config";
        GR.AddLoader(configLoader);

        MonoLoader.InitCallback();
    }
}