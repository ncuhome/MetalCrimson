﻿using ER;
using ER.Parser;
using ER.Resource;
using ER.Save;
using ER.UTask;
using Mod_Rouge;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 指令解释器A
/// </summary>
public partial class AInterpreter : DefaultInterpreter,IUTaskSender
{
    #region 指令函数

    private Data CMD_creatmap(Data[] parameters)
    {
        int seed = 0;

        if (!parameters.IsEmpty())
        {
            if (parameters.IsMate(DataType.Integer))
            {
                seed = (int)parameters[0].Value;
            }
            else
            {
                PrintError($"无效参数：<{parameters[0]}>，此参数必须为整型");
            }
        }
        //Map map = Map.Create(seed);
        return Data.Empty;
    }

    private Data CMD_savestore(Data[] parameters)
    {
        string saveName = "新的存档";
        if (!parameters.IsEmpty())
        {
            saveName = parameters[0].ToString();
        }
        SaveManager.Instance.Save(saveName);
        return Data.Empty;
    }

    private Data CMD_savelist()
    {
        SaveManager.Instance.UpdateList();
        foreach (var save in SaveManager.Instance.saves)
        {
            Print(Path.GetFileNameWithoutExtension(save.Name));
        }
        return Data.Empty;
    }

    private Data CMD_saveload(Data[] parameters)
    {
        if (!parameters.IsEmpty())
        {
            SaveManager.Instance.Load(parameters[0].ToString());
            Print("存档加载完毕");
        }
        else
        {
            PrintError("缺少存档路径参数");
        }
        return Data.Empty;
    }

    private Data CMD_savedisplay()
    {
        if (SaveWrapper.Instance.Data != null)
        {
            SaveWrapper.Instance.Data.PrintInfo();
        }
        else
        {
            PrintError("当前存档为空");
        }
        return Data.Empty;
    }

    private Data CMD_mapconfig_load(Data[] parameters)
    {
        if (!parameters.IsEmpty())
        {
            Print("正在加载地图配置");
            string path = (string)parameters[0].Value;
            if (File.Exists(path))
            {
                RougeMap.Instance.LoadConfig(path);
                Print("加载地图配置完毕");
            }
            else
            {
                PrintError("指定路径不存在");
            }
        }
        PrintError("路径参数不可为空");
        return Data.Error;
    }

    private Data CMD_mapconfig()
    {
        Print($"地图最大层数: {RougeMap.Instance.level_settings}");
        Print($"地图最大长度: {RougeMap.Instance.maxLength_settings}");
        Print($"地图最小长度: {RougeMap.Instance.minLength_settings}");

        float[][] settings = RougeMap.Instance.probabilities_settings;//房间概率值
        Print("房间概率值:");
        if (settings == null) PrintError("配置未初始化");
        else
        {
            for (int i = 0; i < settings.Length; i++)
            {
                Print($"{(RoomType)i}:[", false);
                for (int k = 0; k < settings[i].Length; k++)
                {
                    Print($" {settings[i][k]} ", false);
                }
                Print("]");
            }
        }

        int[][] settings2 = RougeMap.Instance.maxCount_settings;//房间最大值
        Print("房间最大值:");
        if (settings == null) PrintError("配置未初始化");
        else
        {
            for (int i = 0; i < settings2.Length; i++)
            {
                Print($"{(RoomType)i}:[", false);
                for (int k = 0; k < settings2[i].Length; k++)
                {
                    Print($" {settings2[i][k]} ", false);
                }
                Print("]");
            }
        }

        int[][] settings3 = RougeMap.Instance.minCount_settings;//房间最小值
        Print("房间最小值:");
        if (settings == null) PrintError("配置未初始化");
        else
        {
            for (int i = 0; i < settings3.Length; i++)
            {
                Print($"{(RoomType)i}:[", false);
                for (int k = 0; k < settings3[i].Length; k++)
                {
                    Print($" {settings3[i][k]} ", false);
                }
                Print("]");
            }
        }

        return Data.Empty;
    }

    private Data CMD_mapinfo()
    {
        RougeMap.Instance.PrintInfo();
        return Data.Empty;
    }

    private Data CMD_map_start()
    {
        Print("------------------------------------------");
        Room room = RougeMap.Instance.Start();
        room.Print();
        Print("Exit:");
        RougeMap.Instance.PrintExits();
        RougeMap.Instance.PrintInfo();
        Print("------------------------------------------");

        return Data.Empty;
    }

    private Data CMD_map_select(Data[] parameters)
    {
        if (parameters.IsMate(DataType.Integer))
        {
            Print("------------------------------------------");
            int index = (int)parameters[0].Value;
            Room room = RougeMap.Instance.SelectRoom(index);
            if (room != null)
            {
                room.Print();
                Print("Exit:");
                RougeMap.Instance.PrintExits();
                RougeMap.Instance.PrintInfo();
            }
            Print("------------------------------------------");
        }
        return Data.Empty;
    }

    private Data CMD_forge_addItem(Data[] parameters)
    {
        if (parameters.IsMate(DataType.Integer))
        {
            int index = (int)parameters[0].Value;//材料在仓库中的索引
            bool state = false;
            if (HammeringSystem.Instance.AddMaterialJudgement(MaterialSystem.Instance.GetMaterialScript(index), out state))
            {
                Print("添加材料成功");
            }
            else
            {
                PrintError("添加材料失败");
            }
            if (!state)
            {
                PrintError("指定索引物品不存在");
            }
        }
        return Data.Empty;
    }

    private Data CMD_forge_removeItem(Data[] parameters)
    {
        if (parameters.IsMate(DataType.Integer))
        {
            int index = (int)parameters[0].Value;//材料在炉子中的索引
            if (index >= 0 && index <= 2)
            {
                if (!HammeringSystem.Instance.MoveBackMaterial(index))
                {
                    PrintError("指定索引物品不存在");
                }
            }
            else
            {
                PrintError($"索引参数错误：{index}");
                PrintError("索引必须在0~2");
            }
        }
        return Data.Empty;
    }

    private Data CMD_forge_temperature()
    {
        Print($"当前炉子的温度：{HammeringSystem.Instance.temperature}");
        return Data.Empty;
    }

    private Data CMD_forge_temperature_set(Data[] parameters)
    {
        if (parameters.IsMate(DataType.Double))
        {
            //Print(parameters[0].Type.ToString());
            //Print(parameters[0].Value.ToString());
            float tmp = (float)(parameters[0].Value);
            //Print($"temp:{tmp}");
            if (tmp < 0) { tmp = 0; }
            //Print($"temp:{tmp}");
            HammeringSystem.Instance.temperature = tmp;
        }
        else if (parameters.IsMate(DataType.Integer))
        {
            int tmp = (int)(parameters[0].Value);
            //Print($"temp:{tmp}");
            if (tmp < 0) { tmp = 0; }
            //Print($"temp:{tmp}");
            HammeringSystem.Instance.temperature = tmp;
        }
        return Data.Empty;
    }

    private Data CMD_forge_end()
    {
        QTE.Instance.FinishQTE();
        HammeringSystem.Instance.FinishHammering();
        return Data.Empty;
    }

    private Data CMD_forge(Data[] parameters)
    {
        if (parameters.IsMate(DataType.Integer))
        {
            int index = (int)parameters[0].Value;
            int times = 1;
            if (parameters.IsMate(DataType.Integer, DataType.Integer))
            {
                times = (int)parameters[1].Value;
                if (times <= 0) times = 1;
            }

            for (int i = 0; i < times; i++)
            {
                switch (index)
                {
                    case 1:
                        HammeringSystem.Instance.HammerMaterial();
                        break;

                    case 2:
                        HammeringSystem.Instance.HammerMaterial();
                        break;

                    case 3:
                        HammeringSystem.Instance.HammerMaterial();
                        break;
                }
            }
        }

        return Data.Empty;
    }

    private Data CMD_scene_load(Data[] parameters)
    {
        if (parameters.IsMate(DataType.Text))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene((string)parameters[0].Value);
        }
        return Data.Empty;
    }

    private Data CMD_forge_load_packs(Data[] parameters)
    {
        LoadTask[] tasks = new LoadTask[3];
        tasks[0] = GR.Get<LoadTaskResource>("pack:mc:all/cmt").Value;
        tasks[1] = GR.Get<LoadTaskResource>("pack:mc:all/compm").Value;
        tasks[2] = GR.Get<LoadTaskResource>("pack:mc:all/comp").Value;

        for(int i=0;i<tasks.Length;i++)
        {
            GR.AddLoadTask(tasks[i]);
        }
        UpdateTask utk = this.CreateTask(() =>
        {
            bool ok = true;
            for(int i=0;i<tasks.Length;i++)
            {
                ok = ok && tasks[i].progress_load.done && tasks[i].progress_load_force.done;
                if (!ok) break;
            }
            if(ok)
            {
                Debug.Log("[ConsolePanel]: 资源加载完毕");
                Print("[ConsolePanel]: 资源加载完毕");
                return true;
            }
            return false;
            
        });
        utk.Tag = "load_packs";
        utk.Start();
        return Data.Empty;
    }

    private Data CMD_forge_load_demand(Data[] parameters)
    {
        LoadTask[] tasks = new LoadTask[3];

        tasks[0] = GR.Get<LoadTaskResource>("pack:mc:all/cmt_demand").Value;
        tasks[1] = GR.Get<LoadTaskResource>("pack:mc:all/compm_demand").Value;
        tasks[2] = GR.Get<LoadTaskResource>("pack:mc:all/comp_demand").Value;

        for (int i = 0; i < tasks.Length; i++)
        {
            GR.AddLoadTask(tasks[i]);
        }
        UpdateTask utk = this.CreateTask(() =>
        {
            bool ok = true;
            for (int i = 0; i < tasks.Length; i++)
            {
                ok = ok && tasks[i].progress_load.done && tasks[i].progress_load_force.done;
                if (!ok) break;
            }
            if (ok)
            {
                Debug.Log("[ConsolePanel]: 前置资源加载完毕");
                Print("[ConsolePanel]: 前置资源加载完毕");
                return true;
            }
            return false;

        });
        utk.Tag = "load_packs_demand";
        utk.Start();
        return Data.Empty;
    }

    public void TaskCallback(TaskStatus status, string tag)
    {
        Debug.Log($"[ConsolePanel]: {tag} UTask : {status}");
    }

    #endregion 指令函数

    public override Data EffectuateSuper(string commandName, Data[] parameters)
    {
        switch (commandName)
        {
            case "creatmap"://创建肉鸽地图
                return CMD_creatmap(parameters);

            case "savestore"://保存存档
                return CMD_savestore(parameters);

            case "savelist"://显示当前存档文件夹下的所有存档
                return CMD_savelist();

            case "saveload"://加载指定存档文件
                return CMD_saveload(parameters);

            case "savedisplay"://显示当前存档信息
                return CMD_savedisplay();

            case "mapconfig_load"://加载地图生成配置
                return CMD_mapconfig_load(parameters);

            case "mapconfig"://打印地图配置信息
                return CMD_mapconfig();

            case "mapinfo"://打印当前肉鸽地图信息
                return CMD_mapinfo();

            case "map_start"://开始肉鸽关卡；初始化地图
                return CMD_map_start();

            case "map_select"://模拟肉鸽地图选择房间
                return CMD_map_select(parameters);

            case "forge_load_packs":
                return CMD_forge_load_packs(parameters);

            case "forge_load_demand":
                return CMD_forge_load_demand(parameters);

            default:
                return Data.Error;
        }
    }

}