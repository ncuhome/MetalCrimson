using ER;
using ER.Resource;
using ER.Template;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
/// <summary>
/// 仅次于 GameInit_0 后 初始化
/// </summary>
public class GameSettings : MonoSingleton<GameSettings>,ISettings,MonoInit
{
    private Dictionary<string, object> configs;

    public object GetSettings(string registryName)
    {
        throw new System.NotImplementedException();
    }

    public void Init()
    {
        GR.LoadForce(LoadCustom, ERinbone.DefSettingsRegistryName);
    }

    private void LoadCustom()
    {
        
    }
    private void InitUpdate()
    {

    }

    public void Save()
    {
        throw new System.NotImplementedException();
    }

    public void SetSettings(string registryName, object settings)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateSettings()
    {
        
    }
}