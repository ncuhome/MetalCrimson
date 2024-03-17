using ER;
using ER.Resource;
using ER.Template;
using Mod_Resource;
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
        GR.AddLoader(new LanguageLoader());
        GR.AddLoader(new LoadTaskLoader());

        //配置文件加载器(由 TextLoader 改)
        TextLoader configLoader = new TextLoader();
        configLoader.Head = "config";
        GR.AddLoader(configLoader);



        //其他游戏资源加载器
        GR.AddLoader(new RComponentLoader());
        GR.AddLoader(new RComponentMoldLoader());
        GR.AddLoader(new RComponentMoldTypeLoader());
        GR.AddLoader(new RLinkageEffectLoader());
        GR.AddLoader(new RMaterialLoader());

        MonoLoader.InitCallback();
    }
}