using ER;
using ER.Parser;
using ER.Resource;
using ER.Template;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
/// <summary>
/// 配置信息都是INI储存的 Dictionary<string,string>
/// 仅次于 GameInit_0 后 初始化
/// </summary>
public class GameSettings : MonoSingleton<GameSettings>,ISettings,MonoInit
{
    private INIParser configs;
    private INIHandler changes;

    public object GetSettings(string registryName)
    {
        if(changes.Contains(registryName))
        {
            return changes.GetSection(registryName);
        }
        else
        {
            return configs.GetSection(registryName);
        }
    }
    
    public void Init()
    {
        ConsolePanel.Print("[GameSettings]: 初始化中");
        Debug.Log("[GameSettings]: 初始化中");
        configs = new INIParser();
        changes = new INIHandler();
        GR.LoadForce(LoadCustom,ERinbone.DefSettingsRegistryName);
    }

    private void LoadCustom()
    {
        ConsolePanel.Print("[GameSettings]: 完成本地配置加载");
        Debug.Log("[GameSettings]: 完成本地配置加载");
        GR.LoadWithPathForce(InitUpdate, "config",'@'+ERinbone.CustomSettingsPath);
    }
    private void InitUpdate()//加载完资源后开始注入配置信息
    {
        ConsolePanel.Print("[GameSettings]: 完成外部配置加载");
        Debug.Log("[GameSettings]: 完成外部配置加载");
        string def_txt = GR.Get<TextResource>($"config:erinbone:{ERinbone.DefSettingsAddress}").Value;
        string cus_txt = GR.Get<TextResource>($"config:erinbone:{ERinbone.CustomSettingsPath}").Value;
        configs.ParseINIText(def_txt);
        changes.ParseINIText(cus_txt);
        //初始化完毕
        MonoLoader.InitCallback();
    }

    public void Save()
    {
        changes.Save(ERinbone.CustomSettingsPath);
    }

    public void SetSettings(string registryName, object settings)
    {
        changes.DeleteSection(registryName);
        changes.AddSection(registryName);
        Dictionary<string, string> dc = settings as Dictionary<string, string>;
        foreach (var d in dc)
        {
            changes.AddPair(registryName, d.Key, d.Value);
        }
    }

    public void UpdateSettings()
    {
        Init();
    }
}