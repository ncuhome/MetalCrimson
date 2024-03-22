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
    private LoadTask task;
    public void Init()
    {
        GR.LoadForce(() =>
        {
            Debug.Log("[GameInit_0]: 完成全局包配置加载");
            task = GR.Get<LoadTaskResource>("pack:mc:init/global").Value;
            GR.AddLoadTask(task);
            enabled = true;
            
        },"pack:mc:init/global");
        
    }
    private void Update()
    {
        if(task!= null && task.progress_load.done)
        {
            Debug.Log("[GameInit_0]: 所有加载包加载完毕");
            MonoLoader.InitCallback();
            enabled = false;
        }
    }
}