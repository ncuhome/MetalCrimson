
using ER.Common;
using ER.Items;
using ER.Save;
using Mod_Console;
using Mod_Rouge;
using System;
using UnityEngine;

namespace Mod_Common
{
    /// <summary>
    /// 项目配置器，用于对整个项目进行有序的初始化
    /// </summary>
    public class ProjectConfigure : MonoSingleton<ProjectConfigure>
    {
        [Tooltip("存档目录路径")]
        public string SavePath = @"Assets/StreamingAssets/Saves";

        [Tooltip("物品模板数据表路径")]
        public string DataPath = @"Assets/StreamingAssets/模具信息表.csv";

        [Tooltip("地图配置文件路径")]
        public string MapSettingsPath = @"Assets/StreamingAssets/mapsettings.ini";

        private void Settings()//设置预设
        {
            SettingsManager.Instance["Volume"] = 100 + "";
            SettingsManager.Instance["Move Up"] = (int)KeyCode.W + "";
            SettingsManager.Instance["Move Down"] = (int)KeyCode.S + "";
            SettingsManager.Instance["Move Left"] = (int)KeyCode.A + "";
            SettingsManager.Instance["Move Right"] = (int)KeyCode.D + "";
            SettingsManager.Instance["Attack"] = (int)KeyCode.J + "";
            SettingsManager.Instance["Debug"] = (int)KeyCode.F12 + "";
            ConsolePanel.Print("Loading Settings");
            SettingsManager.Instance.SaveSettings();
            ConsolePanel.Print("Saving Settings");
            ConsolePanel.Print($"音量大小：{SettingsManager.Instance["Volume"]}");
        }

        private void Start()
        {
            //启用预设设置选项
            //Settings();

            //更新地图配置
            RougeMap.Instance.LoadConfig(MapSettingsPath);
            //设置使用的指令解释器
            ConsolePanel.interpreter = new AInterpreter();
            //设置存档目录
            SaveManager.Instance.savePackPath = SavePath;
            //设置存档解析器为Json解析器
            SaveWrapper.Instance.interpreter = new JsonInterpreter();
            //初始化所有静态仓库
            TemplateStoreManager.Instance.Load();
        }

        private void Update()
        {
            KeyCode code = (KeyCode)Convert.ToInt32(SettingsManager.Instance["Debug"]);

            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(code))//开启/关闭控制台
                {
                    ConsolePanel.Instance.SwitchUsing();
                }
            }
        }
    }
}